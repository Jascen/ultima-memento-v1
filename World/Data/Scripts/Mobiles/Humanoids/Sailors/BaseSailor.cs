using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server;
using System.Collections.Generic;
using Server.Targeting;
using Server.Multis;

namespace Server.Mobiles
{
	public class BaseSailor : BaseCreature
	{
        private BaseBoat boat;
        private bool boatspawn;
		private DateTime m_NextPickup;
		public int level;

		[Constructable]
		public BaseSailor() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomTalkHue();
			Female = Utility.RandomBool();

			level = Utility.RandomMinMax( 150, 400 );

			SetDamage( ((int)level/20), ((int)level/10) );

			SetSkill( SkillName.Marksmanship, (level/3) );
			SetSkill( SkillName.FistFighting, (level/3) );
			SetSkill( SkillName.MagicResist, (level/3) );
			SetSkill( SkillName.Tactics, (level/3) );
			SetSkill( SkillName.Psychology, (level/3) );
			SetSkill( SkillName.Magery, (level/3) );

			Fame = (int)(level*10);
			Karma = -(int)(level*10);

			if ( Female )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new Skirt( Utility.RandomColor(0) ) );
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomColor(0) ) );
			}

			Hue = Utility.RandomSkinColor();
			HairHue = Utility.RandomHairHue();
			FacialHairHue = HairHue;

            AddItem( new ElvenBoots( 0x6F8 ) );
			AddItem( new FancyShirt( Utility.RandomColor(0) ) );	

            switch ( Utility.Random( 2 ))
			{
				case 0: AddItem( new LongPants ( 0xBB4 ) ); break;
				case 1: AddItem( new ShortPants ( 0xBB4 ) ); break;
			}

			switch ( Utility.Random( 2 ))
			{
				case 0: AddItem( new Bandana ( 0x846 ) ); break;
				case 1: AddItem( new SkullCap ( 0x846 ) ); break;
			}

			Harpoon spear = new Harpoon();
			spear.LootType = LootType.Blessed;
			spear.Attributes.SpellChanneling = 1;
			AddItem( spear );
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override int TreasureMapLevel{ get{ return Utility.RandomMinMax( 1, 3 ); } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override bool BleedImmune{ get{ return true; } }
		public override bool DeleteCorpseOnDeath{ get{ return true; } }

		public override void OnThink()
		{
  			if( boatspawn == false )
  			{
				Map map = this.Map;
				
  				if ( map == null )
  					return;
  					
				boat = new TinyBoat();
				EmoteHue = boat.Serial;
				Point3D loc = this.Location;
				Point3D loccrew = this.Location;
				loc = new Point3D( this.X, this.Y-1, this.Z );
				this.Z = 0;
				boat.MoveToWorld(loc, map);
				boatspawn = true;
				if ( Server.Multis.BaseBoat.IsNearOtherShip( this ) ){ this.Delete(); }
				else if ( Worlds.TestShore( Map, X, Y, 8 ) ){ this.Delete(); }
			}

        	if ( boat == null )
			{
				return;
			} 

			if ( DateTime.Now >= m_NextPickup && ( this is BoatSailorBard || this is BoatPirateBard || this is ElfBoatSailorBard || this is ElfBoatPirateBard ) )
			{
				m_NextPickup = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 10, 20 ) );

				switch( Utility.RandomMinMax( 0, 3 ) )
				{
					case 0:	Peace( Combatant ); break;
					case 1:	Undress( Combatant ); break;
					case 2:	Suppress( Combatant ); break;
					case 3:	Provoke( Combatant ); break;
				}
			}
			base.OnThink();
		}
		
		public override void OnDelete()
		{
			Server.Multis.BaseBoat.SinkShip( boat, this );
			base.OnDelete();
		}

		public override bool OnBeforeDeath()
		{
			Server.Multis.BaseBoat.SinkShip( boat, this );
			Point3D wreck = new Point3D((this.X+3), (this.Y+3), 0);
			SunkenShip ShipWreck = Server.Multis.BaseBoat.CreateSunkenShip( this, this.LastKiller );
			ShipWreck.DropItem( new HarpoonRope( Utility.RandomMinMax( 10, 30 ) ) );

			return base.OnBeforeDeath();   
		}

		public BaseSailor( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( (Item)boat );
			writer.Write( (bool)boatspawn );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
            boat = reader.ReadItem() as BaseBoat;
            boatspawn = reader.ReadBool();
		}

		// ------------------------------------------------------------------------------------------------------------------------------------------

		private DateTime m_NextPeace;
		public void Peace( Mobile target )
		{
			if ( target == null || Deleted || !Alive || m_NextPeace > DateTime.Now )
				return;

			PlayerMobile p = target as PlayerMobile;

			if ( p != null && p.PeacedUntil < DateTime.Now && !p.Hidden && CanBeHarmful( p ) )
			{
				p.PeacedUntil = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 20, 45 ) );
				p.SendLocalizedMessage( 500616 ); // You hear lovely music, and forget to continue battling!
				p.FixedParticles( 0x376A, 1, 32, 0x15BD, EffectLayer.Waist );
				p.Combatant = null;
				target.Warmode = false;
				UndressItem( target, Layer.OneHanded );
				UndressItem( target, Layer.TwoHanded );

				PlaySound( SpeechHue );
			}

			m_NextPeace = DateTime.Now + TimeSpan.FromSeconds( 50 );
		}

		// ------------------------------------------------------------------------------------------------------------------------------------------

		private static Dictionary<Mobile, Timer> m_Suppressed = new Dictionary<Mobile, Timer>();
		private DateTime m_NextSuppress;
		public void Suppress( Mobile target )
		{
			if ( target == null || m_Suppressed.ContainsKey( target ) || Deleted || !Alive || m_NextSuppress > DateTime.Now )
				return;

			TimeSpan delay = TimeSpan.FromSeconds( Utility.RandomMinMax( 20, 80 ) );

			if ( !target.Hidden && CanBeHarmful( target ) )
			{
				target.SendMessage("You hear jarring music, suppressing your abilities.");

				for ( int i = 0; i < target.Skills.Length; i++ )
				{
					Skill s = target.Skills[ i ];

					if ( s.Base > 0 ){ target.AddSkillMod( new TimedSkillMod( s.SkillName, true, s.Base * -0.28, delay ) ); }
				}

				int count = (int) Math.Round( delay.TotalSeconds / 1.25 );
				Timer timer = new AnimateTimer( target, count );
				m_Suppressed.Add( target, timer );
				timer.Start();

				PlaySound( SpeechHue );
			}

			m_NextSuppress = DateTime.Now + TimeSpan.FromSeconds( 90 );
		}

		public static void SuppressRemove( Mobile target )
		{
			if ( target != null && m_Suppressed.ContainsKey( target ) )
			{
				Timer timer = m_Suppressed[ target ];

				if ( timer != null || timer.Running )
					timer.Stop();

				m_Suppressed.Remove( target );
			}
		}

		private class AnimateTimer : Timer
		{
			private Mobile m_Owner;
			private int m_Count;

			public AnimateTimer( Mobile owner, int count ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 1.25 ) )
			{
				m_Owner = owner;
				m_Count = count;
			}

			protected override void OnTick()
			{
				if ( m_Owner.Deleted || !m_Owner.Alive || m_Count-- < 0 )
				{
					SuppressRemove( m_Owner );
				}
				else
					m_Owner.FixedParticles( 0x376A, 1, 32, 0x15BD, EffectLayer.Waist );
			}
		}

		// ------------------------------------------------------------------------------------------------------------------------------------------

		private DateTime m_NextUndress;
		public void Undress( Mobile target )
		{
			if ( target == null || Deleted || !Alive || m_NextUndress > DateTime.Now )
				return;

			if ( target.Player && target.Female && !target.Hidden && CanBeHarmful( target ) )
			{
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.OuterTorso ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.InnerTorso ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.MiddleTorso ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.Pants ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.Shirt ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.Ring ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.Helm ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.Arms ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.OuterLegs ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.Neck ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.Gloves ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.Trinket ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.Shoes ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.Cloak ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.InnerLegs ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.Earrings ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.Waist ); }
				if ( Utility.RandomBool() ){ UndressItem( target, Layer.Bracelet ); }

				target.SendMessage("The music is hypnotic, making you remove your worn items.");
				PlaySound( SpeechHue );
			}

			m_NextUndress = DateTime.Now + TimeSpan.FromSeconds( 20 );
		}

		public void UndressItem( Mobile m, Layer layer )
		{
			Item item = m.FindItemOnLayer( layer );

			if ( item != null && item.Movable )
				m.PlaceInBackpack( item );
		}

		// ------------------------------------------------------------------------------------------------------------------------------------------

		private DateTime m_NextProvoke;
		public void Provoke( Mobile target )
		{
			if ( target == null || Deleted || !Alive || m_NextProvoke > DateTime.Now )
				return;

			foreach ( Mobile m in GetMobilesInRange( RangePerception ) )
			{
				if ( m is BaseCreature )
				{
					BaseCreature c = (BaseCreature) m;

					if ( c == this || c == target || c.Unprovokable || c.IsParagon || c.BardProvoked || c.AccessLevel != AccessLevel.Player || !c.CanBeHarmful( target ) )
						continue;

					c.Provoke( this, target, true );

					if ( target.Player )
						target.SendLocalizedMessage( 1072062 ); // You hear angry music, and start to fight.

					PlaySound( SpeechHue );
					break;
				}
			}

			m_NextProvoke = DateTime.Now + TimeSpan.FromSeconds( 10 );
		}

		// ------------------------------------------------------------------------------------------------------------------------------------------
	}
}