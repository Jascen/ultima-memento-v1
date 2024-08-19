using Server.Engines.Harvest;

namespace Server.Items.Abstractions
{
    public interface IPickaxe
    {
        HarvestSystem HarvestSystem { get; }
    }
}