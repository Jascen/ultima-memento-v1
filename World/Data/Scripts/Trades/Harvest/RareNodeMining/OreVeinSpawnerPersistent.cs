namespace Server.Engines.Harvest
{
    public class OreVeinSpawnerPersistent : OreVeinSpawner
    {
        [Constructable]
        public OreVeinSpawnerPersistent() : base()
        {
            Name = "Ore Vein Spawner (Persistent)";
        }

        public OreVeinSpawnerPersistent(Serial serial) : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }
    }
}