using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Prompts;
using System.Net;
using Server.Accounting;
using Server.Mobiles;
using Server.Commands;
using Server.Regions;
using Server.Spells;
using Server.Gumps;
using Server.Targeting;

namespace Server.Items
{
	[Flipable(0x577B, 0x577C)]
	public class SearchBoard : Item
	{
		[Constructable]
		public SearchBoard( ) : base( 0x577B )
		{
			Weight = 1.0;
			Name = "Sage Advice";
			Hue = 0x986;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( "The Search for Artifacts" );
		}

		public override void OnDoubleClick( Mobile e )
		{
			if ( e.InRange( this.GetWorldLocation(), 4 ) )
			{
				e.CloseGump( typeof( BoardGump ) );
				e.SendGump( new BoardGump( e, "SAGE ADVICE", "If you have a grand quest to unearth an artifact, you can seek the advice of sages in your journey. Their advice is not cheap, they charge 10,000 gold for the guidance. To begin your quest, visit one of the many sages in the land and give them enough gold for their advice. They will give you an artifact encyclopedia from which you can search for the first clues on the whereabouts of your artifact.<br><br>Sages are never able to give you absolute accurate information on the location of an artifact. Once you receive your encyclopedia, open it up and choose an artifact from its many pages. If you are not sure what artifact you seek, simply look through the Sage's wares for sale. At the end of their inventory, you will see research replicas of these artifacts priced at zero gold. You can hover over these artifacts to see what they may offer you, but you cannot buy them. Artifacts such as books, quivers, and instruments will be shown with some common and random qualities, where finding the actual artifact will have somewhat different properties. The remaining items have set qualities as well as a number of Enchantment Points that you can spend to make the artifact more customized for yourself. When you find these artifacts, single click them and select the Enchant option to spend the points on the additional attributes you want. After selecting an artifact from the book, you will tear out the appropriate page and toss out the remainder of the book. This page will give you your first clue on where to search. Areas the artifact may be in could span many different lands or worlds, where some you may have never been or heard of. You will be provided with the coordinates of the place you seek, so make sure you have a sextant with you.<br><br><br><br>Throughout history, many people kept these artifacts stored on blocks of crafted stone. These crafted stones are often decorated with a symbol on the surface, where a metal chest rests and the item may be inside. Some treasure hunters find the chests empty, realizing the legends were false. If nothing else, you may find a large sum of gold to cover some of your expenses on this journey. Some may provide a new clue on where the artifact is, and you will update your notes when these clues are found. The most disappointing search may yield a fake artifact. These turn out to be useless items that simply look like the artifact you were searching for. <br><br><br><br>These quests are quite involved and you may only participate in one such quest at a time. If you have not finished a quest, and try to seek a sage for another, you will find that the page of your prior quest will have gone missing. It would have been surely lost somewhere. If you finish a quest, either with success or failure, a sage will not have any new advice for you for quite some time so you will have to wait until then to begin a new quest. So good luck treasure hunter, and may the gods aid you in your journey.", "#d3d307", true ) );
			}
			else
			{
				e.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public SearchBoard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}