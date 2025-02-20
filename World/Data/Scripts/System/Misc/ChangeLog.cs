using System.Text;

namespace Server.Misc
{
    class ChangeLog
    {
		public static string Version()
		{
			return "Version: Hegran (DD MMM YYYY)";
		}

		public static string Versions()
        {
			const string SEPARATOR_LINE = "<BR>---------------------------------------------------------------------------------<BR><BR>";
			var builder = new StringBuilder();

			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

			builder.Append(Version());
			builder.Append("<br>");

			builder.Append("<br>");
			builder.Append("Changes<br>");
			builder.Append("- Craft - Redesigned crafting gump + multi crafting now uses an input box<br>");
			builder.Append("- Craft - Using non-basic resources can now yield multiple skill gains<br>");
			builder.Append("- Craft - Resist bonus for exceptional hats is now a function of Arms Lore skill level (reduces bonus from 15 to 7)<br>");
			builder.Append("- Craft - Bulk crafting no longer resets Stat Gain cooldown<br>");
			builder.Append("- Craft - Add ability to break down all items in a container<br>");
			builder.Append("- Craft - Items can only be enhanced if they are basic resources<br>");
			builder.Append("- Craft - Artifacts can now be enhanced<br>");
			builder.Append("- Craft - Breakdown no longer requires 50 skill<br>");
			builder.Append("- Craft - Cutting regular cloth/leather no longer requires 50 Tailoring<br>");
			builder.Append("- Enchantment - Lower Mana Cost (LMC) is now capped at 8%<br>");
			builder.Append("- Enchantment - Lower Reagent Cost (LRC) is now capped at 20%<br>");
			builder.Append("- Gather - Add 'Glistening Ore Vein' to give Mining a more active playstyle<br>");
			builder.Append("- Gather - Dwarven ore can now only be acquired via 'Glistening Ore Vein'<br>");
			builder.Append("- Gather - Increased Nepturite spawn rate<br>");
			builder.Append("- Gather - Sawing logs now automatically checks against each log individually<br>");
			builder.Append("- Gather - Smelting ore now automatically checks against each ore individually<br>");
			builder.Append("- Gather - Removed ore size variations<br>");
			builder.Append("- Gather - Increase automatic harvest distance for crops to 1 tile<br>");
			builder.Append("- Gather - Increase log weight to 1 stone each<br>");
			builder.Append("- Gather - Increase ore weight to 2 stones each<br>");
			builder.Append("- Gather - Reduce mining vein size from 10-34 to 5-17<br>");
			builder.Append("- Gather - Smelting iron ore no longer requires 50 Mining<br>");
			builder.Append("- Gather - Ore/Logs can be used while on a Pack Animal<br>");
			builder.Append("- Item - Increased the cost of all Powerscrolls<br>");
			builder.Append("- Item - The luck bonus for Magic horse shoe is now 100 per item<br>");
			builder.Append("- Item - Artifact enchantment points have been reduced by half (min is still 50)<br>");
			builder.Append("- Item - Dungeon Chests now allow for multiple Stealing attempts<br>");
			builder.Append("- Misc - Add smooth boat movement<br>");
			builder.Append("- Misc - Add Secondary Skills that do not impact total skill cap<br>");
			builder.Append("        Alchemy, Blacksmith, Bowcraft, Carpentry, Cooking, Inscription, Tailoring, Tinkering<br>");
			builder.Append("        Forensics, Lumberjacking, Mining<br>");
			builder.Append("- Misc - Reduce packhorse max weight to 2400 (down from 65k)<br>");
			builder.Append("- Quest - Sage Artifact quest now always costs 10,000 gold<br>");
			builder.Append("- Settings - Lower Mana Cost (LMC) is now capped at 40%<br>");
			builder.Append("- Settings - Lower Reagent Cost (LRC) is now capped at 100%<br>");
			builder.Append("- Settings - Added a setting to require eating Powerscrolls in order<br>");
			builder.Append("- Skill - Healing is now an activatable* skill that can remove poison/mortal wound or heal you<br>"); // TODO: Update documentation, Make usable in client files
			builder.Append("- Skill - Hiding cooldown is increased to 4 seconds (up from 1s and 2s)<br>");
			builder.Append("- Spell - Players must be friend or higher to use any spell in a house<br>");
			builder.Append("- Stats - Mana Regen is now defaultly capped at 18<br>");

			builder.Append("<br>");
			builder.Append("Fixes<br>");
			builder.Append("- Gather - The 'Resources' server setting is now limited by the amount of resources in the harvest bank<br>");
			builder.Append("- Gather - More tiles are now mineable<br>");
			builder.Append("- Gump - Alien Players who use Tithe to pay for a res from another player are no longer double penalized<br>");
			builder.Append("- Gather - Added system message when digging up Dwarven ore/granite<br>");
			builder.Append("- Gather - Drop harvested items to ground when backpack is full<br>");
			builder.Append("- Item - Magic horse shoe now work on Instruments and Quivers<br>");
			builder.Append("- Item - Fixed an issue where DragonLamp would fail to load<br>");
			builder.Append("- Item - SpellChanneling no longer boosts sell price for Throwing/Pugilist gloves<br>");
			builder.Append("- Misc - Buffs/Debuffs could end on the Client before they finished on the Server<br>");
			builder.Append("- Misc - Set Map when [scan players<br>");
			builder.Append("- Misc - Stop deleting an item when it's stacked with itself<br>");
			builder.Append("- Misc - Aliens no longer start with gold<br>");
			builder.Append("- Misc - Monster races now get configured starting gold<br>");
			builder.Append("- Spell - BloodOath could linger up to 1s too long on the Server<br>");
			builder.Append(SEPARATOR_LINE);

			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

			builder.Append("Core update - 20 August 2024<br>");
			builder.Append("- Update to Adventurers of Akalabeth version 'Knight - 16 August 2024'<br>");
			builder.Append(SEPARATOR_LINE);

			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

			builder.Append("Inception - 4 August 2024<br>");
			builder.Append(SEPARATOR_LINE);

			///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			
			builder.Append("Necromancer - 26 July 2024<br>");
			builder.Append("<br>");
			builder.Append("- Ultima: Memento begins using this Adventurers of Akalabeth patch<br>");
			builder.Append("<br>");

			return builder.ToString();
		}
	}
}