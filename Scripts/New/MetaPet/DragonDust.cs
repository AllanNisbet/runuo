using System; 
using Server.Items; 
using Server.Mobiles;

namespace Server.Items 
{ 
   	public class DragonDust: Item 
   	{ 
		[Constructable]
		public DragonDust() : this( 1 )
		{
		}

		[Constructable]
		public DragonDust( int amount ) : base( 0x26B8 )
		{
			Stackable = true;
  			//Weight = ;
			Amount = amount;
			Name = "dragon's dust";
			Hue = 89;
		}

            	public DragonDust( Serial serial ) : base ( serial ) 
            	{             
           	} 

		//public override Item Dupe( int amount )
		//{
			//return base.Dupe( new DragonDust( amount ), amount );
		//}


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
        } 
}