using System;
using Server;

namespace Server.Items
{
	   
	public class FullJarOfBlood : Item
	{
		[Constructable]
		public FullJarOfBlood() : this( null )
		{
		}

		[Constructable]
		public FullJarOfBlood( string name ) : base( 0x1006 )
		{
			Name = "a full jar of blood";
			Weight = 5.0;
			Hue = 0x485;
		}

		public FullJarOfBlood( Serial serial ) : base( serial )
		{
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