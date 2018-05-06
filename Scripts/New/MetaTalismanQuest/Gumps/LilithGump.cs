using System; 
using Server; 
using Server.Gumps; 
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Commands;
using System.Collections;
using System.IO;

namespace Server.Gumps
{ 
   public class LilithGump : Gump 
   { 
      public static void Initialize() 
      { 
         //Commands.Register( "LilithGump", AccessLevel.GameMaster, new CommandEventHandler( LilithGump_OnCommand ) ); 
		 CommandSystem.Register( "LilithGump", AccessLevel.Administrator, new CommandEventHandler( LilithGump_OnCommand ) );
      } 

      private static void LilithGump_OnCommand( CommandEventArgs e ) 
      { 
         e.Mobile.SendGump( new LilithGump( e.Mobile ) ); 
      } 

      public LilithGump( Mobile owner ) : base( 50,50 ) 
      { 
//----------------------------------------------------------------------------------------------------

				AddPage( 0 );
			AddImageTiled(  54, 33, 369, 400, 2624 );
			AddAlphaRegion( 54, 33, 369, 400 );

			AddImageTiled( 416, 39, 44, 389, 203 );
//--------------------------------------Window size bar--------------------------------------------
			
			AddImage( 97, 49, 9005 );
			AddImageTiled( 58, 39, 29, 390, 10460 );
			AddImageTiled( 412, 37, 31, 389, 10460 );
			AddLabel( 140, 60, 0x34, "Ettins Slauter" );
			

			AddHtml( 107, 140, 300, 230, "<BODY>" +
//----------------------/----------------------------------------------/
"<BASEFONT COLOR=YELLOW>*Lilith glances up.*<BR><BR>Great warrior - I am the first of my kind from the Lost Lands. I have a thirst that cannot be quenched.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>If you bring me back a full jar of blood, I will reward you with untold power<BR>" +
"<BASEFONT COLOR=YELLOW>So what you must do is slay 40 Daemon's and drain their blood into this jar.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>Find them, Kill them, Drain them.<BR><BR>" +
"<BASEFONT COLOR=YELLOW>Bring back the blood as fast as you can.<BR><BR>" +

						     "</BODY>", false, true);
			
//			<BASEFONT COLOR=#7B6D20>			

			AddImage( 430, 9, 10441);
			AddImageTiled( 40, 38, 17, 391, 9263 );
			AddImage( 6, 25, 10421 );
			AddImage( 34, 12, 10420 );
			AddImageTiled( 94, 25, 342, 15, 10304 );
			AddImageTiled( 40, 427, 415, 16, 10304 );
			AddImage( -10, 314, 10402 );
			AddImage( 56, 150, 10411 );
			AddImage( 155, 120, 2103 );
			AddImage( 136, 84, 96 );

			AddButton( 225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0 ); 

//--------------------------------------------------------------------------------------------------------------
      } 

      public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons 
      { 
         Mobile from = state.Mobile; 

         switch ( info.ButtonID ) 
         { 
            case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0 
            { 
               //Cancel 
               from.SendMessage( "Bring me back a full Jar, and I will reward you with untold power." );
               break; 
            } 

         }
      }
   }
}