using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Server.Engines.Harvest
{
    public class OreVeinEngine
    {
        public const int EXECUTE_INTERVAL_HOURS = 1;
        public const string UNKNOWN_REGION_NAME = "$None";
        private static readonly Dictionary<Serial, OreVeinSpawner> _spawnRegistry = new Dictionary<Serial, OreVeinSpawner>();
        private static readonly Dictionary<Map, Dictionary<string, OreVeinConfig>> _configByMapByRegion = new Dictionary<Map, Dictionary<string, OreVeinConfig>>();
        private static OreVeinEngine m_Engine;
        private InternalTimer _timer;

        private OreVeinEngine()
        {
        }

        public static OreVeinEngine Instance
        {
            get
            {
                if (m_Engine == null)
                    m_Engine = new OreVeinEngine();

                return m_Engine;
            }
        }

        public static void Configure()
        {
            EventSink.WorldLoad += OnWorldLoad;
        }

        public void AddConfig(Map map, OreVeinConfig config)
        {
            // GetOrCreate lookup
            Dictionary<string, OreVeinConfig> configByRegion;
            if (!_configByMapByRegion.TryGetValue(map, out configByRegion)) _configByMapByRegion[map] = configByRegion = new Dictionary<string, OreVeinConfig>();

            OreVeinConfig existing;
            if (configByRegion.TryGetValue(config.Region, out existing))
            {
                Console.WriteLine("Warning: Replacing configuration for '{0}' on map '{1}'", config.Region, map.Name);

                // Purge existing
                foreach (var spawner in _spawnRegistry.Values.Where(x => x.AutomaticCleanup && x.Map == map).ToList())
                {
                    var regionName = OreVeinUtilities.GetRegionName(map, spawner.Location);
                    if (regionName != config.Region) continue;

                    spawner.Delete();
                }
            }

            configByRegion[config.Region] = config;
        }

        public bool AddSpawn(OreVeinSpawner spawn)
        {
            if (_spawnRegistry.ContainsKey(spawn.Serial)) return false;

            _spawnRegistry.Add(spawn.Serial, spawn);
            return true;
        }

        public bool RemoveSpawn(OreVeinSpawner spawn)
        {
            if (!_spawnRegistry.ContainsKey(spawn.Serial)) return false;

            _spawnRegistry.Remove(spawn.Serial);
            return true;
        }

        public void StartTimer(int delaySeconds)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }

            var nodeDef = OreVeinMining.System.RareNodeDefinition;
            var interval = Utility.RandomMinMax((int)nodeDef.MinRespawn.TotalMilliseconds, (int)nodeDef.MaxRespawn.TotalMilliseconds);
            _timer = new InternalTimer(TimeSpan.FromSeconds(delaySeconds), TimeSpan.FromMilliseconds(interval));
            _timer.Start();
        }

        private static void OnWorldLoad()
        {
            // Always start fresh
            OreVeinCommands.OnClear(new Commands.CommandEventArgs(null, null, null, null));
            OreVeinCommands.OnLoadData(new Commands.CommandEventArgs(null, null, null, new string[] { }));

            Instance.StartTimer(30);
        }

        private class InternalTimer : Timer
        {
            public InternalTimer(TimeSpan delay, TimeSpan interval) : base(delay, interval)
            {
            }

            protected override void OnTick()
            {
                var stopwatch = Stopwatch.StartNew();

                DeleteExpired(_spawnRegistry.Values);
                var spawnersByMapByRegion = GetExistingSpawnersLookup(_spawnRegistry.Values);
                DoSpawn(_configByMapByRegion, spawnersByMapByRegion);

                stopwatch.Stop();

                Console.WriteLine("Processing all nodes took {0} ms", stopwatch.ElapsedMilliseconds);
            }

            private void DeleteExpired(IEnumerable<OreVeinSpawner> spawners)
            {
                var nowTimestampUtc = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var expiredSpawns = spawners.Where(spawn => spawn.IsExpired(nowTimestampUtc)).ToList();
                foreach (var spawn in expiredSpawns)
                {
                    spawn.Delete();
                }
            }

            private void DoSpawn(Dictionary<Map, Dictionary<string, OreVeinConfig>> configByMapByRegion, Dictionary<Map, Dictionary<string, List<OreVeinSpawner>>> spawnersByMapByRegion)
            {
                // Ensure manually placed spawners are populated
                var nowTimestampUtc = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var manualSpawners = spawnersByMapByRegion.Values.SelectMany(collection => collection.Values).SelectMany(collection => collection.Where(s => s is OreVeinSpawnerPersistent).Cast<OreVeinSpawnerPersistent>());
                foreach (var spawner in manualSpawners)
                {
                    if (!spawner.IsExpired(nowTimestampUtc)) continue;

                    spawner.EnsureNodeCreated(nowTimestampUtc);
                }

                // Process the dynamic nodes
                foreach (var map in configByMapByRegion.Keys)
                {
                    // GetOrCreate lookup
                    Dictionary<string, List<OreVeinSpawner>> existingByRegion;
                    if (!spawnersByMapByRegion.TryGetValue(map, out existingByRegion)) spawnersByMapByRegion[map] = existingByRegion = new Dictionary<string, List<OreVeinSpawner>>();

                    var configByRegion = configByMapByRegion[map];
                    foreach (var region in configByRegion.Keys)
                    {
                        // GetOrCreate lookup
                        List<OreVeinSpawner> existingSpawns;
                        if (!existingByRegion.TryGetValue(region, out existingSpawns)) existingByRegion[region] = existingSpawns = new List<OreVeinSpawner>();

                        var config = configByRegion[region];
                        var maxSpawnSlots = config.MaxNodes - existingSpawns.Count;
                        if (maxSpawnSlots < 1) continue; // Already at max capacity

                        // Make sure we have any locations left to check
                        var spawnsByLocation = existingSpawns.ToDictionary(x => x.Location);
                        var unspawnedCandidates = config.Candidates.Where(c => !spawnsByLocation.ContainsKey(c)).ToList();
                        if (!unspawnedCandidates.Any()) continue;

                        // Randomly pick from the remainder, up to the max amount of spawns
                        for (var i = 0; 0 < unspawnedCandidates.Count && i < maxSpawnSlots; i++)
                        {
                            var rand = Utility.Random(unspawnedCandidates.Count);
                            var locationCandidate = unspawnedCandidates[rand];

                            // Range check
                            if (existingSpawns.Any(spawn => Utility.RangeCheck(spawn.Location, locationCandidate, config.MinDistance))) continue;

                            var spawner = new OreVeinSpawner(true);
                            spawner.OnAfterSpawn();
                            spawner.MoveToWorld(locationCandidate, map);
                            spawner.EnsureNodeCreated(nowTimestampUtc);
                            existingSpawns.Add(spawner);
                        }
                    }
                }
            }

            private Dictionary<Map, Dictionary<string, List<OreVeinSpawner>>> GetExistingSpawnersLookup(IEnumerable<OreVeinSpawner> spawners)
            {
                var spawnersByMapByRegion = new Dictionary<Map, Dictionary<string, List<OreVeinSpawner>>>();

                foreach (var spawner in spawners)
                {
                    Dictionary<string, List<OreVeinSpawner>> existingByRegion;
                    if (!spawnersByMapByRegion.TryGetValue(spawner.Map, out existingByRegion)) spawnersByMapByRegion[spawner.Map] = existingByRegion = new Dictionary<string, List<OreVeinSpawner>>();

                    var regionName = OreVeinUtilities.GetRegionName(spawner.Map, spawner.Location);
                    List<OreVeinSpawner> existingSpawns;
                    if (!existingByRegion.TryGetValue(regionName, out existingSpawns)) existingByRegion[regionName] = existingSpawns = new List<OreVeinSpawner>();

                    existingSpawns.Add(spawner);
                }

                return spawnersByMapByRegion;
            }
        }
    }
}