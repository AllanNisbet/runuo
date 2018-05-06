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
    public class DoubleStrikeTalismanRelic : BaseMetaTalismanRelic
	{
		[Constructable]
	    public DoubleStrikeTalismanRelic()
            {
			Name = "a double strike talisman relic";
            Hue = 2955;
			Weight = 2.0;
			Stackable = false;
            MetaTalismanSkillType = MetaTalismanSkillType.DoubleStrike;
            MetaTalismanSkillName = "Double Strike";
            NextLevelExperience = 200;
            MaxLevel = 10;
            ApplicationChance = 0.2;
            CoolDown = TimeSpan.FromSeconds(30);
            SkillMulti = 1;
	    }

        public DoubleStrikeTalismanRelic(Serial serial)
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