using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Misc;

namespace Server.Items
{
	public class StealMetalBox : WoodenBox
	{
		public int BoxColor;
		public string BoxName;
		public string BoxMarkings;

		[CommandProperty(AccessLevel.Owner)]
		public int Box_Color { get { return BoxColor; } set { BoxColor = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public string Box_Name { get { return BoxName; } set { BoxName = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public string Box_Markings { get { return BoxMarkings; } set { BoxMarkings = value; InvalidateProperties(); } }

		[Constructable]
		public StealMetalBox()
		{
			Hue = BoxColor;
			Name = BoxName;
			Weight = 15.0;
			GumpID = 0x4B;
		}

		public override void Open( Mobile from )
		{
			if ( this.Weight > 10.0 )
			{
				int nStolen = Utility.RandomMinMax( 1, 3 );

				if ( nStolen == 1 )
				{
					Item i = Loot.RandomArty();
					DropItem(i);
				}
				else if ( nStolen == 2 )
				{
					Item i = Loot.RandomSArty( Server.LootPackEntry.playOrient( from ), from );
					DropItem(i);
				}
				else if ( nStolen == 3 )
				{
					Item i = Loot.RandomRelic( from );
					DropItem(i);
				}
				else
				{
					DropItem( Loot.RandomRare( Utility.RandomMinMax(6,12), from ) );
				}

				int money = Utility.RandomMinMax( 1000, 4000 );

					double w = money * (MyServerSettings.GetGoldCutRate() * .01);
					money = (int)w;
					Item g = new Gold( money );
					DropItem( g );

				this.Weight = 10.0;
			}

			base.Open( from );
		}
		public StealMetalBox( Serial serial ) : base( serial )
		{
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, BoxMarkings );
        }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
            writer.Write( BoxColor );
            writer.Write( BoxName );
            writer.Write( BoxMarkings );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
            BoxColor = reader.ReadInt();
            BoxName = reader.ReadString();
            BoxMarkings = reader.ReadString();
		}
	}
}