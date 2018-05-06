#region References
using System;

using Server.Factions;
using Server.Mobiles;
using Server.Items.MetaTalisman;
using Server.Items.MetaTalisman.Skills;
using Server.Items.MetaTalismanSkills;
using Server.Spells;
using Server.Spells.Fifth;
using Server.Spells.Seventh;
using VitaNex.Targets;

#endregion

namespace Server.Items
{
    public class InfectiousWoundsTalismanRelic : BaseMetaTalismanRelic
	{
		[Constructable]
	    public InfectiousWoundsTalismanRelic()
            {
			Name = "a infectious wounds talisman relic";
            Hue = 1271;
			Weight = 2.0;
			Stackable = false;
            MetaTalismanSkillType = MetaTalismanSkillType.InfectiousWounds;
            MetaTalismanSkillName = "Infectious Wounds";
            NextLevelExperience = 200;
            MaxLevel = 10;
            ApplicationChance = 0.2;
            CoolDown = TimeSpan.FromSeconds(30);
            SkillMulti = 1;
	    }

        public InfectiousWoundsTalismanRelic(Serial serial)
			: base(serial)
		{ }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

            int version = writer.SetVersion(0);

            switch (version)
            {
                case 0:
                    break;
            }
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

            int version = reader.GetVersion();


            switch (version)
            {
                case 0:
                    break;
            }
		}
	}
}