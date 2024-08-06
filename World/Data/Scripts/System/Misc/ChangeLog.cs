using System;
using Server;
using System.Collections;
using Server.Misc;
using Server.Network;
using Server.Commands;
using Server.Commands.Generic;
using Server.Mobiles;
using Server.Accounting;
using Server.Regions;
using Server.Targeting;
using System.Collections.Generic;
using Server.Items;
using Server.Spells.Fifth;
using System.IO;
using System.Xml;

namespace Server.Misc
{
    class ChangeLog
    {
		public static string Version()
		{
			return "Version: Magician (16 July 2024)";
		}

		public static string Versions()
        {
			string versionTEXT = ""


				///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

				+ "Inception - 4 August 2024<br>"

				+ "<br>"

				+ sepLine()

				///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

				+ "Necromancer - 26 July 2024<br>"

				+ "<br>"

				+ "- Ultima: Memento begins using this Adventurers of Akalabeth patch<br>"
				+ "<br>"

			+ "";

			return versionTEXT;
		}

		public static string sepLine()
		{
			return "---------------------------------------------------------------------------------<BR><BR>";
		}
	}
}