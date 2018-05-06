using System;
using Server;
using Server.Targeting; 
using Server.Mobiles; 

namespace Server.Items
{
	   
	public class JarOfBlood : Item
	{

		[Constructable]
		public JarOfBlood() : this( null )
		{
		}

		[Constructable]
		public JarOfBlood( string name ) : base( 0x1005 )
		{
			Name = "an empty jar";
			Weight = 1.0;
			Hue = 0;
		}

		public JarOfBlood( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.

			else
				from.Target = new ClaimDaemonTarget( this );

		}

		public class ClaimDaemonTarget : Target
		{
			private JarOfBlood m_BloodyJar;

			public ClaimDaemonTarget( Mobile from ) : base( 6 /* RANGE */, false /* ALLOWGROUND */, TargetFlags.None /* BENEFICAL / HARMFUL / NONE */ )
			{
			}
			
			public ClaimDaemonTarget( JarOfBlood jar ) : base( 6, false, TargetFlags.None )
			{
				m_BloodyJar = jar;
			}

			protected override void OnTarget( Mobile from /* WHO TARGETED */, object target /* WHAT'S BEEN TARGETED */ )
			{
				if ( target is Corpse )
				{
					Corpse c = (Corpse)target;

					if ( c.Owner is Daemon )//If you want this script to claim some other creature change "Ettin" in this line.
					{
						if ( c.Channeled == false )
						{
							c.Channeled = true;
							c.Hue = 0x835;
							m_BloodyJar.Weight +=  0.1; //I guess if you want it to go up in higher increments change the 0.1 just be careful.
							m_BloodyJar.InvalidateProperties();
							if ( m_BloodyJar.Weight == 1.1 )
							{
							m_BloodyJar.Name = "a jar of blood";
							m_BloodyJar.Hue = 1157;
							m_BloodyJar.ItemID = 4103;
							from.SendMessage("You notice as you drain blood from the daemon's corpse the Jar's weight increases.");
							}
							else if ( m_BloodyJar.Weight <= 1.9 ){}
							else if ( m_BloodyJar.Weight <= 2.0 )
							{
							from.SendMessage("The scarlet liquid continues to rise as you add more blood to the jar.");
							}
							else if ( m_BloodyJar.Weight <= 2.9 ){}
							else if ( m_BloodyJar.Weight <= 3.0 )
							{
							from.SendMessage("You notice the jar is nearly half-way full.");//You can change any of the messages easy, and better to what you like.
							}
							else if ( m_BloodyJar.Weight <= 3.9 ){}
							else if ( m_BloodyJar.Weight <= 4.0 )
							{
							from.SendMessage("As you drain blood from the daemon's corpse you notice it is almost full.");
							}
							else if ( m_BloodyJar.Weight <= 4.9 ){}//Always keep this number 0.1 right below the following.
							else if ( m_BloodyJar.Weight >= 5.0 )//If you want your players to kill more ettins make the 5.0 higher.
							{
							from.SendMessage("You notice as you drain the daemon the jar becomes full.");
							from.AddToBackpack( new FullJarOfBlood() );
							m_BloodyJar.Delete();
							}
						}

						else
						from.SendMessage("This body has already been drained of all its blood.");
					}
				}

				else
				from.SendMessage("You may only use this item on corpses.");
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

		}
	}
}