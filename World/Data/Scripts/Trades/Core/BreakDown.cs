using System;
using Server;
using Server.Targeting;
using Server.Items;

namespace Server.Engines.Craft
{
	public enum BreakDownResult
	{
		Success,
		Invalid,
		NoSkill
	}

	public class BreakDown
	{
		public BreakDown()
		{
		}

		public static void Do( Mobile from, CraftSystem craftSystem, BaseTool tool )
		{
			int num = craftSystem.CanCraft( from, tool, null );

			if ( num > 0 && num != 1044267 )
			{
				from.SendGump( new CraftGump( from, craftSystem, tool, num ) );
			}
			else
			{
				from.Target = new InternalTarget( craftSystem, tool );
				from.SendLocalizedMessage( 1044273 ); // Target an item to recycle.
			}
		}

		private class InternalTarget : Target
		{
			private CraftSystem m_CraftSystem;
			private BaseTool m_Tool;

			public InternalTarget( CraftSystem craftSystem, BaseTool tool ) :  base ( 2, false, TargetFlags.None )
			{
				m_CraftSystem = craftSystem;
				m_Tool = tool;
			}

			private BreakDownResult BreakDown( Mobile from, Item item, CraftResource resource )
			{
				try
				{
					bool canBreakDown = false;
					bool extraCloth = false;

					if ( Item.IsStandardResource( item.Resource ) && !Item.IsStandardResource( item.SubResource ) )
					{
						resource = item.SubResource;
						if ( CraftResources.GetType( resource ) == CraftResourceType.Fabric )
							extraCloth = true;
					}

					if ( CraftResources.GetType( resource ) == m_CraftSystem.BreakDownType )
						canBreakDown = true;

					if ( CraftResources.GetType( resource ) == m_CraftSystem.BreakDownTypeAlt && m_CraftSystem.BreakDownTypeAlt != CraftResourceType.None )
						canBreakDown = true;

					if ( !canBreakDown )
						return BreakDownResult.Invalid;

					CraftResourceInfo info = CraftResources.GetInfo( resource );

					if ( info == null || info.ResourceTypes.Length == 0 )
						return BreakDownResult.Invalid;

					double difficulty = CraftResources.GetSkill( resource );

					if ( difficulty < 50.0 )
						difficulty = 50.0;

					if ( difficulty > from.Skills[ m_CraftSystem.MainSkill ].Value )
						return BreakDownResult.NoSkill;

					Type resourceType = info.ResourceTypes[0];
					Item resc = (Item)Activator.CreateInstance( resourceType );

					resc.Amount = (int)(item.Weight);
						if ( resc.Amount < 1 ){ resc.Amount = 1; }

					if ( extraCloth )
						resc.Amount = resc.Amount * 10;

					if ( item is BaseTrinket && item.Catalog == Catalogs.Jewelry && ((BaseTrinket)item).GemType != GemType.None )
					{
						Item gem = null;
						if ( ((BaseTrinket)item).GemType == GemType.StarSapphire )
							gem = new StarSapphire();
						else if ( ((BaseTrinket)item).GemType == GemType.Emerald )
							gem = new Emerald();
						else if ( ((BaseTrinket)item).GemType == GemType.Sapphire )
							gem = new Sapphire();
						else if ( ((BaseTrinket)item).GemType == GemType.Ruby )
							gem = new Ruby();
						else if ( ((BaseTrinket)item).GemType == GemType.Citrine )
							gem = new Citrine();
						else if ( ((BaseTrinket)item).GemType == GemType.Amethyst )
							gem = new Amethyst();
						else if ( ((BaseTrinket)item).GemType == GemType.Tourmaline )
							gem = new Tourmaline();
						else if ( ((BaseTrinket)item).GemType == GemType.Amber )
							gem = new Amber();
						else if ( ((BaseTrinket)item).GemType == GemType.Diamond )
							gem = new Diamond();
						else if ( ((BaseTrinket)item).GemType == GemType.Pearl )
							gem = new Oyster();

						if ( gem != null )
							BaseContainer.PutStuffInContainer( from, 2, gem );

					}

					item.Delete();
					BaseContainer.PutStuffInContainer( from, 2, resc );

					m_CraftSystem.PlayCraftEffect( from );
					return BreakDownResult.Success;
				}
				catch
				{
				}

				return BreakDownResult.Invalid;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Item )
				{
					if ( !((Item)targeted).IsChildOf( from.Backpack ) ) 
					{
						from.SendMessage( "You can only do this with items in your pack." );
						return;
					}

					int num = m_CraftSystem.CanCraft( from, m_Tool, null );

					if ( num > 0 )
					{
						if ( num == 1044267 )
						{
							bool anvil, forge;
				
							DefBlacksmithy.CheckAnvilAndForge( from, 2, out anvil, out forge );

							if ( !anvil )
								num = 1044266; // You must be near an anvil
							else if ( !forge )
								num = 1044265; // You must be near a forge.
						}
						
						from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, num ) );
					}
					else
					{
						BreakDownResult result = BreakDownResult.Invalid;
						int message;

						result = BreakDown( from, (Item)targeted, ((Item)targeted).Resource );

						switch ( result )
						{
							default:
							case BreakDownResult.Invalid: message = 1044272; break; // You can't seem to break that item down.
							case BreakDownResult.NoSkill: message = 1044149; break; // You have no idea how to break this item down.
							case BreakDownResult.Success: message = 1044148; break; // You break the item down into ordinary resources.
						}
						
						from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, message ) );
					}
				}
			}
		}
	}
}