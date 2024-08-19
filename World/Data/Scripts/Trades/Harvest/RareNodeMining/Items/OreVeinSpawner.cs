using System;

namespace Server.Engines.Harvest
{
    public class OreVeinSpawner : Item
    {
        private const long EXPIRATION_MILLISECONDS = 1000 * 60 * 60; // One hour

        private OreVeinMineable _node;

        public OreVeinSpawner() : base(0x0ED4)
        {
            Name = "Ore Vein Spawner";
            Visible = false;
            Movable = false;
        }

        public OreVeinSpawner(bool autoCleanup) : this()
        {
            AutomaticCleanup = autoCleanup;
        }

        public OreVeinSpawner(Serial serial) : base(serial)
        {
        }

        [CommandProperty(AccessLevel.Counselor)]
        public bool AutomaticCleanup { get; protected set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public long LastSpawnUtc { get; set; }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            AutomaticCleanup = reader.ReadBool();
            LastSpawnUtc = reader.ReadLong();

            OreVeinEngine.Instance.AddSpawn(this);
        }

        public void EnsureNodeCreated(long nowTimestampUtc)
        {
            if (_node != null && !_node.Deleted) return;

            var node = new OreVeinMineable(this);
            node.MoveToWorld(Location, Map);
            _node = node;
            LastSpawnUtc = nowTimestampUtc;
        }

        public bool IsExpired(long nowTimestampUtc)
        {
            return LastSpawnUtc + EXPIRATION_MILLISECONDS < nowTimestampUtc;
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (_node != null)
            {
                _node.Delete();
            }
        }

        public override void OnAfterSpawn()
        {
            OreVeinEngine.Instance.AddSpawn(this);
        }

        public override void OnDelete()
        {
            if (Deleted) return;

            OreVeinEngine.Instance.RemoveSpawn(this);

            base.OnDelete();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel < AccessLevel.GameMaster) { return; }

            var nowTimestampUtc = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            EnsureNodeCreated(nowTimestampUtc);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(AutomaticCleanup);
            writer.Write(LastSpawnUtc);
        }
    }
}