using Server;
using System;
using Server.Spells;
using System.Text;
using System.Collections;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using System.Globalization;

namespace Server.Items
{
	public class SpellItemInfo
	{
		private MagicSpell m_MagicSpell;
		private int m_SpellID;
		private Type m_ScrollType;
		private string m_SpellName;
		private string m_Description;
		private string m_Circle;

		public MagicSpell MageSpell{ get{ return m_MagicSpell; } }
		public int SpellID{ get{ return m_SpellID; } }
		public Type ScrollType{ get{ return m_ScrollType; } }
		public string SpellName{ get{ return m_SpellName; } }
		public string Description{ get{ return m_Description; } }
		public string Circle{ get{ return m_Circle; } }

		public SpellItemInfo( MagicSpell magic, int id, Type scrollType, string name, string desc, string circle )
		{
			m_MagicSpell = magic;
			m_SpellID = id;
			m_ScrollType = scrollType;
			m_SpellName = name;
			m_Description = desc;
			m_Circle = circle;
		}
	}

	public class SpellItems
	{
		private static SpellItemInfo[] m_MagicInfo = new SpellItemInfo[]																														
		{
			new SpellItemInfo( MagicSpell.None, 0, typeof( BlankScroll ), "", "", "" ),																										
			new SpellItemInfo( MagicSpell.Clumsy, 0, typeof( ClumsyScroll ), "clumsy", "Temporarily reduces Target's Dexterity.", "First" ),
			new SpellItemInfo( MagicSpell.CreateFood, 1, typeof( CreateFoodScroll ), "create food", "Creates random food item in Caster’s backpack.", "First" ),
			new SpellItemInfo( MagicSpell.Feeblemind, 2, typeof( FeeblemindScroll ), "feeblemind", "Temporarily reduces Target’s Intelligence.", "First" ),
			new SpellItemInfo( MagicSpell.Heal, 3, typeof( HealScroll ), "heal", "Heals Target of a small amount of lost Hit Points.", "First" ),
			new SpellItemInfo( MagicSpell.MagicArrow, 4, typeof( MagicArrowScroll ), "magic arrow", "Shoots a magical arrow at Target, which deals Fire damage.", "First" ),
			new SpellItemInfo( MagicSpell.NightSight, 5, typeof( NightSightScroll ), "night sight", "Temporarily allows Target to see in darkness.", "First" ),
			new SpellItemInfo( MagicSpell.ReactiveArmor, 6, typeof( ReactiveArmorScroll ), "reactive armor", "Increases the Caster’s Physical Resistance while reducing their Elemental Resistances.  The Caster’s Inscription skill adds a bonus to the amount of Physical Resist applied. Active until spell is deactivated by re-casting the spell on the same Target.", "First" ),
			new SpellItemInfo( MagicSpell.Weaken, 7, typeof( WeakenScroll ), "weaken", "Temporarily reduces Target’s Strength.", "First" ),
			new SpellItemInfo( MagicSpell.Agility, 8, typeof( AgilityScroll ), "agility", "Temporarily increases Target’s Dexterity.", "Second" ),
			new SpellItemInfo( MagicSpell.Cunning, 9, typeof( CunningScroll ), "cunning", "Temporarily increases Target’s Intelligence.", "Second" ),
			new SpellItemInfo( MagicSpell.Cure, 10, typeof( CureScroll ), "cure", "Attempts to neutralize poisons affecting the Target.", "Second" ),
			new SpellItemInfo( MagicSpell.Harm, 11, typeof( HarmScroll ), "harm", "Affects the Target with a chilling effect, dealing Cold damage.  The closer the Target is to the Caster, the more damage is dealt.", "Second" ),
			new SpellItemInfo( MagicSpell.MagicTrap, 12, typeof( MagicTrapScroll ), "magic trap", "Places an explosive magic ward on a container that deals Fire damage to the next person to open it. You can also target the ground and place a random trap for the careless.", "Second" ),
			new SpellItemInfo( MagicSpell.RemoveTrap, 13, typeof( MagicUnTrapScroll ), "magic untrap", "Deactivates a magical trap on a container, or you can cast on yourself to summon an orb of trap detection. item orb would remain in your pack and help you avoid hidden traps.", "Second" ),
			new SpellItemInfo( MagicSpell.Protection, 14, typeof( ProtectionScroll ), "protection", "Prevents the Target from having their spells disrupted, but lowers their Physical Resistance and Magic Resistance.  Active until the spell is deactivated by recasting on the same Target.", "Second" ),
			new SpellItemInfo( MagicSpell.Strength, 15, typeof( StrengthScroll ), "strength", "Temporarily increases Target’s Strength.", "Second" ),
			new SpellItemInfo( MagicSpell.Bless, 16, typeof( BlessScroll ), "bless", "Temporarily increases Target’s Strength, Dexterity, and Intelligence.", "Third" ),
			new SpellItemInfo( MagicSpell.Fireball, 17, typeof( FireballScroll ), "fireball", "Shoots a ball of roiling flames at a Target, dealing Fire damage.", "Third" ),
			new SpellItemInfo( MagicSpell.MagicLock, 18, typeof( MagicLockScroll ), "magic lock", "Magically lock a container or dungeon door, but also lock a creatures soul in an iron flask.", "Third" ),
			new SpellItemInfo( MagicSpell.Poison, 19, typeof( PoisonScroll ), "poison", "The Target is afflicted by poison, of a strength determined by the Caster’s Magery and Poison skills, and the distance from the Target.", "Third" ),
			new SpellItemInfo( MagicSpell.Telekinesis, 20, typeof( TelekinisisScroll ), "telekinisis", "Allows the Caster to Use an item at a distance. You may also be able to grab smaller objects from a distance and put them in your pack.", "Third" ),
			new SpellItemInfo( MagicSpell.Teleport, 21, typeof( TeleportScroll ), "teleport", "Caster is transported to the Target Location.", "Third" ),
			new SpellItemInfo( MagicSpell.Unlock, 22, typeof( UnlockScroll ), "unlock", "Unlocks a magical lock or low level normal lock.", "Third" ),
			new SpellItemInfo( MagicSpell.WallOfStone, 23, typeof( WallOfStoneScroll ), "wall of stone", "Creates a temporary wall of stone that blocks movement.", "Third" ),
			new SpellItemInfo( MagicSpell.ArchCure, 24, typeof( ArchCureScroll ), "arch cure", "Neutralizes poisons on all characters within a small radius around the caster.", "Fourth" ),
			new SpellItemInfo( MagicSpell.ArchProtection, 25, typeof( ArchProtectionScroll ), "arch protection", "Applies the Protection spell to all valid targets within a small radius around the Target Location.", "Fourth" ),
			new SpellItemInfo( MagicSpell.Curse, 26, typeof( CurseScroll ), "curse", "Lowers the Strength, Dexterity, and Intelligence of the Target. When cast during Player vs. Player combat the spell also reduces the target's maximum resistance values.", "Fourth" ),
			new SpellItemInfo( MagicSpell.FireField, 27, typeof( FireFieldScroll ), "fire field", "Summons a wall of fire that deals Fire damage to all who walk through it", "Fourth" ),
			new SpellItemInfo( MagicSpell.GreaterHeal, 28, typeof( GreaterHealScroll ), "greater heal", "Heals the target of a medium amount of lost Hit Points.", "Fourth" ),
			new SpellItemInfo( MagicSpell.Lightning, 29, typeof( LightningScroll ), "lightning", "Strikes the Target with a bolt of lightning, which deals Energy damage.", "Fourth" ),
			new SpellItemInfo( MagicSpell.ManaDrain, 30, typeof( ManaDrainScroll ), "mana drain", "Temporarily removes an amount of mana from the Target, based on a comparison between the Caster’s Psychology skill and the Target’s Magic Resistance skill.", "Fourth" ),
			new SpellItemInfo( MagicSpell.Recall, 31, typeof( RecallScroll ), "recall", "Caster is transported to the location marked on the Target rune. If a ship key is target, Caster is transported to the boat the key opens.", "Fourth" ),
			new SpellItemInfo( MagicSpell.BladeSpirits, 32, typeof( BladeSpiritsScroll ), "blade spirits", "Summons a whirling pillar of blades that selects a Target to attack based off its combat strength and proximity.  The Blade Spirit disappears after a set amount of time.  Requires 2 pet control slots.", "Fifth" ),
			new SpellItemInfo( MagicSpell.DispelField, 33, typeof( DispelFieldScroll ), "dispel field", "Destroys one tile of a target Field spell.", "Fifth" ),
			new SpellItemInfo( MagicSpell.Incognito, 34, typeof( IncognitoScroll ), "incognito", "Disguises the Caster with a randomly generated appearance and name.", "Fifth" ),
			new SpellItemInfo( MagicSpell.MagicReflect, 35, typeof( MagicReflectScroll ), "magic reflect", "Causes the magery spells cast at you to be reflected back toward the one who cast it. The better your magery and evaulate intelligence, the more magic you can reflect back before the spell wears off. You will need a diamond to make item spell work, along with the reagents.", "Fifth" ),
			new SpellItemInfo( MagicSpell.MindBlast, 36, typeof( MindBlastScroll ), "mind blast", "Deals Cold damage to the Target based off Caster's Magery and Intelligence.", "Fifth" ),
			new SpellItemInfo( MagicSpell.Paralyze, 37, typeof( ParalyzeScroll ), "paralyze", "Immobilizes the Target for a brief amount of time.  The Target’s Magic Resistance skill affects the Duration of the immobilization.", "Fifth" ),
			new SpellItemInfo( MagicSpell.PoisonField, 38, typeof( PoisonFieldScroll ), "poison field", "Conjures a wall of poisonous vapor that poisons anything that walks through it.  The strength of the Poison is based off of the Caster’s Magery and Poison skills.", "Fifth" ),
			new SpellItemInfo( MagicSpell.SummonCreature, 39, typeof( SummonCreatureScroll ), "summon creature", "Summons a random creature as a Pet for a limited duration.  The strength of the summoned creature is based off of the Caster’s Magery skill.", "Fifth" ),
			new SpellItemInfo( MagicSpell.Dispel, 40, typeof( DispelScroll ), "dispel", "Attempts to Dispel a summoned creature, causing it to disappear from the world. The Dispel difficulty is affected by the Magery skill of the creature’s owner.", "Sixth" ),
			new SpellItemInfo( MagicSpell.EnergyBolt, 41, typeof( EnergyBoltScroll ), "energy bolt", "Fires a bolt of magical force at the Target, dealing Energy damage.", "Sixth" ),
			new SpellItemInfo( MagicSpell.Explosion, 42, typeof( ExplosionScroll ), "explosion", "Strikes the Target with an explosive blast of energy, dealing Fire damage.", "Sixth" ),
			new SpellItemInfo( MagicSpell.Invisibility, 43, typeof( InvisibilityScroll ), "invisibility", "Temporarily causes the Target to become invisible.", "Sixth" ),
			new SpellItemInfo( MagicSpell.Mark, 44, typeof( MarkScroll ), "mark", "Marks a rune to the Caster’s current Location. There are magic spells and abilities that can be used on the rune to teleport one to the location it is marked with.", "Sixth" ),
			new SpellItemInfo( MagicSpell.MassCurse, 45, typeof( MassCurseScroll ), "mass curse", "Casts the Curse spell on a Target, and any creatures within a two tile radius.", "Sixth" ),
			new SpellItemInfo( MagicSpell.ParalyzeField, 46, typeof( ParalyzeFieldScroll ), "paralyze field", "Conjures a field of paralyzing energy that affects any creature that enters it with the effects of the Paralyze spell.", "Sixth" ),
			new SpellItemInfo( MagicSpell.Reveal, 47, typeof( RevealScroll ), "reveal", "Reveals the presence of any invisible or hiding creatures or players within a radius around the targeted tile.", "Sixth" ),
			new SpellItemInfo( MagicSpell.ChainLightning, 48, typeof( ChainLightningScroll ), "chain lightning", "Damages nearby targets with a series of lightning bolts that deal Energy damage.", "Seventh" ),
			new SpellItemInfo( MagicSpell.EnergyField, 49, typeof( EnergyFieldScroll ), "energy field", "Conjures a temporary field of energy on the ground at the Target Location that blocks all movement.", "Seventh" ),
			new SpellItemInfo( MagicSpell.FlameStrike, 50, typeof( FlamestrikeScroll ), "flamestrike", "Envelopes the target in a column of magical flame the deals Fire damage.", "Seventh" ),
			new SpellItemInfo( MagicSpell.GateTravel, 51, typeof( GateTravelScroll ), "gate travel", "Targeting a rune marked with the Mark spell opens a temporary portal to the rune’s marked location.  The portal can be used by anyone to travel to that location.", "Seventh" ),
			new SpellItemInfo( MagicSpell.ManaVampire, 52, typeof( ManaVampireScroll ), "mana vampire", "Drains mana from the Target and transfers it to the Caster. The amount of mana drained is determined by a comparison between the Caster’s Psychology skill and the Target’s Magic Resistance skill.", "Seventh" ),
			new SpellItemInfo( MagicSpell.MassDispel, 53, typeof( MassDispelScroll ), "mass dispel", "Attempts to dispel any summoned creature within an eight tile radius.", "Seventh" ),
			new SpellItemInfo( MagicSpell.MeteorSwarm, 54, typeof( MeteorSwarmScroll ), "meteor swarm", "Summons a swarm of fiery meteors that strike all targets within a radius around the Target Location.  The total Fire damage dealt is split between all Targets of the spell.", "Seventh" ),
			new SpellItemInfo( MagicSpell.Polymorph, 55, typeof( PolymorphScroll ), "polymorph", "Temporarily transforms the Caster into a creature selected from a specified list.  While polymorphed, other players will see the Caster as a criminal.", "Seventh" ),
			new SpellItemInfo( MagicSpell.Earthquake, 56, typeof( EarthquakeScroll ), "earthquake", "Causes a violent shaking of the earth that damages all nearby creatures and characters.", "Eighth" ),
			new SpellItemInfo( MagicSpell.EnergyVortex, 57, typeof( EnergyVortexScroll ), "energy vortex", "Summons a spinning mass of energy that selects a Target to attack based off its intelligence and proximity.  The Energy Vortex disappears after a set amount of time. Requires 2 pet control slots.", "Eighth" ),
			new SpellItemInfo( MagicSpell.Resurrection, 58, typeof( ResurrectionScroll ), "resurrection", "Resurrects another or summons a magical item to resurrect yourself at a later time.", "Eighth" ),
			new SpellItemInfo( MagicSpell.AirElemental, 59, typeof( SummonAirElementalScroll ), "summon air elemental", "An air elemental is summoned to serve the Caster. Requires 2 pet control slots.", "Eighth" ),
			new SpellItemInfo( MagicSpell.SummonDaemon, 60, typeof( SummonDaemonScroll ), "summon daemon", "A daemon is summoned to serve the Caster. Results in a large Karma loss for the Caster. Requires 4 pet control slots.", "Eighth" ),
			new SpellItemInfo( MagicSpell.EarthElemental, 61, typeof( SummonEarthElementalScroll ), "summon earth elemental", "An earth elemental is summoned to serve the caster. Requires 2 pet control slots", "Eighth" ),
			new SpellItemInfo( MagicSpell.FireElemental, 62, typeof( SummonFireElementalScroll ), "summon fire elemental", "A fire elemental is summoned to serve the caster. Requires 4 pet control slots.", "Eighth" ),
			new SpellItemInfo( MagicSpell.WaterElemental, 63, typeof( SummonWaterElementalScroll ), "summon water elemental", "A water elemental is summoned to serve the caster. Requires 3 pet control slots.", "Eighth" ),
			new SpellItemInfo( MagicSpell.SummonSnakes, 700, typeof( BlankScroll ), "", "xxxx", "" ),
			new SpellItemInfo( MagicSpell.SummonDragon, 701, typeof( BlankScroll ), "", "xxxx", "" ),
			new SpellItemInfo( MagicSpell.SummonSkeleton, 704, typeof( BlankScroll ), "", "xxxx", "" ),
			new SpellItemInfo( MagicSpell.Identify, 705, typeof( BlankScroll ), "", "xxxx", "" ),
		};

		public static void setSpell( int level, Item item )
		{
			if ( level > 1000 ) // SPECIFIC WAND
			{
				level = level - 1000;
				item.Enchanted = ( MagicSpell )( level );
			}
			else
			{
				if ( level < 1 )
					level = Utility.RandomMinMax(1,8);

				if ( level > 8 )
					level = 8;

				item.Enchanted = ( MagicSpell )( ( ( level * 8 ) - 8 ) + Utility.RandomMinMax( 1, 8 ) );
			}
		}

		public static void Cast( Spell spell, Mobile caster )
		{
			bool m = caster.CantWalk;
			caster.CantWalk = false;
			spell.Cast();
			caster.CantWalk = m;
		}

		public static void ChangeMagicSpell( MagicSpell spell, Item item, bool chargeable )
		{
			if ( spell == MagicSpell.None )
			{
				item.InfoData = null;
				item.InfoText2 = null;
				item.EnchantUsesMax = 0;
				item.EnchantUses = 0;
			}
			else
			{
				int level = SpellItems.GetLevel( (int)spell );
				item.EnchantUsesMax = 90 - ( level * 10 );
				item.EnchantUses = item.EnchantUsesMax;

				if ( !chargeable )
					item.EnchantUsesMax = 0;

				item.InfoData = "This can cast the " + SpellItems.GetName( item.Enchanted ) + " spell. " + SpellItems.GetData( item.Enchanted ) + " It must be equipped to cast spells, where mana is usually required. Once the charges deplete, the magic will be gone. To cast the enchanted spell, single click the item and select 'Magic'.";
			}
		}

		public static void CastEnchantment( Mobile from, Item item )
		{
			if ( item.Parent != from )
				from.SendMessage("That must be equipped to use.");
			else if ( item.EnchantUses > 0 )
				SpellItems.Cast( SpellRegistry.NewSpell( ((int)(item.Enchanted)-1), from, item ), from );
			else
				from.SendLocalizedMessage( 1019073 ); // This item is out of charges.
		}

		public static SpellItemInfo GetInfo( MagicSpell magicspell )
		{
			SpellItemInfo[] list = m_MagicInfo;

			int index = GetIndex( magicspell );

			if ( index >= 0 && index < list.Length )
				return list[index];

			return null;
		}

		public static int GetIndex( MagicSpell magicspell )
		{
			if ( magicspell == MagicSpell.None )
				return 0;

			return (int)(magicspell);
		}

		public static string GetCircle( MagicSpell magicspell )
		{
			SpellItemInfo info = GetInfo( magicspell );

			if ( info == null )
				return null;

			if ( info.Circle == "" )
				return null;

			return info.Circle + " Circle";
		}

		public static string GetData( MagicSpell magicspell )
		{
			SpellItemInfo info = GetInfo( magicspell );

			return ( info == null ? null : info.Description );
		}

		public static string GetName( MagicSpell magicspell )
		{
			SpellItemInfo info = GetInfo( magicspell );

			return ( info == null ? null : info.SpellName );
		}

		public static string GetNameUpper( MagicSpell magicspell )
		{
			SpellItemInfo info = GetInfo( magicspell );
			TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;

			return ( info == null ? null : cultInfo.ToTitleCase( info.SpellName ) );
		}

		public static Type GetScroll( MagicSpell magicspell )
		{
			SpellItemInfo info = GetInfo( magicspell );

			return ( info == null ? null : info.ScrollType );
		}

		public static MagicSpell GetID( Type itemtype )
		{
			SpellItemInfo[] list = m_MagicInfo;
			int entries = list.Length;
			int val = 0;

			while ( entries > 0 )
			{
				if ( list[val].ScrollType == itemtype )
					entries = 0;
				else
					val++;

				entries--;
			}

			return (MagicSpell)val;
		}

		public static int GetWand( string name )
		{
			SpellItemInfo[] list = m_MagicInfo;
			int entries = list.Length;
			int val = 0;

			while ( entries > 0 )
			{
				if ( list[val].SpellName == name )
					entries = 0;
				else
					val++;

				entries--;
			}

			return 1000+val;
		}

		public static int GetLevel( int level )
		{
			if ( level >= 57 )
				level = 8;
			else if ( level >= 49 )
				level = 7;
			else if ( level >= 41 )
				level = 6;
			else if ( level >= 33 )
				level = 5;
			else if ( level >= 25 )
				level = 4;
			else if ( level >= 17 )
				level = 3;
			else if ( level >= 9 )
				level = 2;
			else
				level = 1;

			return level;
		}
	}
}