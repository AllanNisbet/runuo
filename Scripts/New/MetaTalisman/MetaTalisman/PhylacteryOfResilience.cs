using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Items.MetaTalisman;
using Server.Items.MetaTalisman.Skills;
using Server.Items.MetaTalismanSkills;
using VitaNex.SuperGumps;
using VitaNex.SuperGumps.UI;

namespace Server.Items
{
	public class PhylacteryOfResilience: BaseMetaTalisman
	{
        private Mobile m_Owner;

		[Constructable]
		public PhylacteryOfResilience()
		
        {
            Name = "<BASEFONT COLOR=#0099CC>Phylactery of Resilience";
			Hue = 1266;
			Weight = 0.0;
			Stackable = false;
            LootType = LootType.Blessed;
			Layer = Layer.Talisman;
			NextEvolution = 25000;
            MaxStage = 7;
            MetaTalismanskills = new Dictionary<MetaTalismanSkillType, BaseMetaTalismanSkill>();
            MaxAbilities = 0;
            CurrentAbilities = 0;
        }

        public PhylacteryOfResilience(Serial serial)
            : base(serial)
        {}
				
		public override void OnDoubleClick( Mobile from )
        {
            PlayerMobile player = (PlayerMobile)from;
			Item item = player.FindItemOnLayer( Layer.Talisman );
			new MetaTalismanUI(from as PlayerMobile, item as BaseMetaTalisman).Send();
            player.SendMessage("You examine the Talisman more closely.");
        }
		
		public override bool OnEquip(Mobile from)
        {
            // set owner if not already set -- this is only done the first time.
            if (m_Owner == null)
            {
                m_Owner = from;
                from.SendMessage("You feel the Talisman grow fond of you.");
                return base.OnEquip(from);
            }
            else
            {
                if (m_Owner != from)
                {
                    from.SendMessage("Sorry but this Talisman does not belong to you.");
                    return false;
                }

                return base.OnEquip(from);
            }
        }
		
		public override void Addpoints(BaseCreature creature, int points)
        {
            if (Stage < MaxStage)
            {				
                EvoXp += points;
                if (CanEvolve())
                {
                    Stage++;
                    Console.WriteLine("Your Talisman begins to pulsate with power!");
                    Timer.DelayCall(TimeSpan.FromSeconds(30), Evolve);
                }
            }
        }		
		
		public override void Evolve()
        {
            base.Evolve();
            switch (Stage)
            {
                case 1:
                {
              
                    NextEvolution = 25000;
					EvoXp = 0;
                    break;
                }
                case 2:
                {

                    NextEvolution = 75000;
                    EvoXp = 0;
                    MaxAbilities = 1;
					break;
                }
                case 3:
                {
                    
                    NextEvolution = 250000;
                    EvoXp = 0;
                    MaxAbilities = 1;
					break;
                }
                case 4:
                {
                    
                    NextEvolution = 750000;
                    EvoXp = 0;
                    MaxAbilities = 2;
					break;
                }
                case 5:
                {
                    
                    NextEvolution = 1500000;
                    EvoXp = 0;
                    MaxAbilities = 2;
					break;
                }
                case 6:
                {
                   
                    NextEvolution = 3000000;
                    EvoXp = 0;
                    MaxAbilities = 3;
					break;
                }
                case 7:
                {
                    
                    NextEvolution = 0;
                    EvoXp = 0;
                    MaxAbilities = 4;
					break;
                }
            }
        }
		
		public override bool CanEvolve()
        {
            if (Stage < 7 && EvoXp >= NextEvolution)
            {
				return true;	
            }
            return false;
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);				
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                {
							
				}
                    break;
            }
        }
    }
}