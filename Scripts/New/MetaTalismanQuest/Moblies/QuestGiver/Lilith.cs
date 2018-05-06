using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Network;
using Server.Spells;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "Lilith's Corpse" )]
	public class Lilith : Mobile
	{
        public virtual bool IsInvulnerable{ get{ return true; } }
		[Constructable]
		public Lilith()
		{
			Name = "Lilith the Vampire";

			Body = 0x191;
			CantWalk = true;
			Hue = 0x38A;
			AddItem( new Server.Items.Skirt( 1175 ) );
			AddItem( new Server.Items.Boots( 1175 ) );
			AddItem( new Server.Items.Shirt( 1175 ) );
			AddItem( new BlackStaff() );
            int hairHue = 1157;
			switch ( Utility.Random( 1 ) )
			{
				case 0: AddItem( new LongHair( hairHue ) ); break;
			} 
			
			Blessed = true;
			
		}

		public Lilith( Serial serial ) : base( serial )
		{
		}

		public override void GetContextMenuEntries( Mobile from,  List<ContextMenuEntry> list ) 
		{ 
				base.GetContextMenuEntries( from, list ); 
				list.Add( new LilithEntry( from, this ) ); 
		} 

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public class LilithEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private Mobile m_Giver;
			
			public LilithEntry( Mobile from, Mobile giver ) : base( 6146, 3 )
			{
				m_Mobile = from;
				m_Giver = giver;
			}

			public override void OnClick()
			{
				if( !( m_Mobile is PlayerMobile ) )
				return;
				
				PlayerMobile mobile = (PlayerMobile) m_Mobile;

				{
					if ( ! mobile.HasGump( typeof( LilithGump ) ) )
					{
						mobile.SendGump( new LilithGump( mobile ));
						mobile.AddToBackpack( new JarOfBlood() );				
					} 
				}
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{          		
         	Mobile m = from;
			PlayerMobile mobile = m as PlayerMobile;

			if ( mobile != null)
			{
				if( dropped is FullJarOfBlood )
         		{
         			if(dropped.Amount!=1)
         			{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "No, that's not it...", mobile.NetState );
         				return false;
         			}

					dropped.Delete(); 
					mobile.AddToBackpack( new Gold( 5000 ));
					mobile.SendGump( new LilithFinishGump());
				switch (Utility.Random( 3 ))
				{
					case 0: mobile.AddToBackpack( new TotemOfTheBerserker() ); break;
					case 1: mobile.AddToBackpack( new IdolOfTheMagi() ); break;
					case 2: mobile.AddToBackpack( new PhylacteryOfResilience() ); break;
				}
					return true;
         		}
				else if ( dropped is FullJarOfBlood)
				{
				this.PrivateOverheadMessage( MessageType.Regular, 1153, 1054071, mobile.NetState );
         			return false;
				}
         		else
         		{
				this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "Oh no, I have no need of this!", mobile.NetState );
     			}
			}
			return false;
		}
	}
}
