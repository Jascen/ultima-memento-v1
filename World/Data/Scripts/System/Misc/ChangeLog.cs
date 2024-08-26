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

			builder.Append("Hegran<br>");
			builder.Append("- Craft - Redesigned crafting gump + multi crafting now uses an input box<br>");
			builder.Append("- Craft - Using non-basic resources can now yield multiple skill gains<br>");
			builder.Append("- Enchantment - Lower Mana Cost (LMC) is now capped at 8%<br>");
			builder.Append("- Enchantment - Lower Reagent Cost (LRC) is now capped at 20%<br>");
			builder.Append("- Gather - (bug?) The 'Resources' server setting is now limited by the amount of resources in the harvest bank<br>");
			builder.Append("- Gather - (bug) More tiles are now mineable<br>");
			builder.Append("- Gather - Add 'Glistening Ore Vein' to give Mining a more active playstyle<br>");
			builder.Append("- Gather - Dwarven ore can now only be acquired via 'Glistening Ore Vein'<br>");
			builder.Append("- Gather - Increased Nepturite spawn rate<br>");
			builder.Append("- Gather - Sawing logs now automatically checks against each log individually<br>");
			builder.Append("- Gather - Smelting ore now automatically checks against each ore individually<br>");
			builder.Append("- Gather - Removed ore size variations<br>");
			builder.Append("- Gather - (bug) Added system message when digging up Dwarven ore/granite<br>");
			builder.Append("- Item - Increased the cost of all Powerscrolls<br>");
			builder.Append("- Item - (bug?) Magic horse shoe now work on Instruments and Quivers<br>");
			builder.Append("- Item - The luck bonus for Magic horse shoe is now 100 per item<br>");
			builder.Append("- Item - Artifact enchantment points have been reduced by half (min is still 50) <br>");
			builder.Append("- Misc - (bug) (De)buffs could end on the Client before they finished on the Server<br>");
			builder.Append("- Quest - Sage Artifact quest now always costs 10,000 gold<br>");
			builder.Append("- Settings - Lower Mana Cost (LMC) is now capped at 40%<br>");
			builder.Append("- Settings - Lower Reagent Cost (LRC) is now capped at 100%<br>");
			builder.Append("- Skill - Healing is now an activatable* skill that can remove poison/mortal wound or heal you<br>"); // TODO: Update documentation, Make usable in client files
			builder.Append("- Spell - (bug) BloodOath could linger up to 1s too long on the Server<br>");
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