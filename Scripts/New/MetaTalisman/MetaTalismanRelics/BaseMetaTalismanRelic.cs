#region References
using System;
using System.Diagnostics;
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
	public class BaseMetaTalismanRelic : Item
	{
        [CommandProperty(AccessLevel.GameMaster)]
        public MetaTalismanSkillType MetaTalismanSkillType { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public string MetaTalismanSkillName { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int NextLevelExperience { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxLevel { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public double ApplicationChance { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan CoolDown { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public double SkillMulti { get; set; }

        [Constructable]
        public BaseMetaTalismanRelic()
			: base(10916)
		{
			Name = "a relic";
            Hue = 1368;
			Weight = 2.0;
			Stackable = false;
		}

        public BaseMetaTalismanRelic(Serial serial)
			: base(serial)
		{ }

	    public override void OnDoubleClick(Mobile from)
	    {
	        if (IsChildOf(from.Backpack) && from is PlayerMobile)
	        {
                from.SendMessage(54, "Perhaps if you had a talisman you could harness the raw energy of this relic..");
	        }
	        else
	        {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
	        }
	    }

        public virtual void GetMetaTalisman(Mobile User, Item target)
        {
            var metatalisman = target as BaseMetaTalisman;
			Console.WriteLine("Testing user: (" + User + ")");
			Console.WriteLine("Testing target: (" + target + ")");
            if (metatalisman != null)
            {
                if (metatalisman.MaxAbilities < 0 || metatalisman.CurrentAbilities >= metatalisman.MaxAbilities)
                {
                    User.SendMessage(54, "Your talisman does not have any free slots to add this ability to.");
                    return;
                }
                if (metatalisman.MetaTalismanskills != null && !metatalisman.MetaTalismanskills.ContainsKey(MetaTalismanSkillType))
                {
                    metatalisman.CurrentAbilities++;
                    metatalisman.MetaTalismanskills.Add(MetaTalismanSkillType, new BaseMetaTalismanSkill(MetaTalismanSkillType, MetaTalismanSkillName, NextLevelExperience, MaxLevel, ApplicationChance, CoolDown, SkillMulti));
                    User.SendMessage(54, "You have successfully applied this relic to your talisman.");
                    Consume();
                }
                else
                {
                    User.SendMessage(54, "Your talisman already has this skill!");                   
                }
            }
            else
            {
                User.SendMessage(54, "This can only be used on a talisman that you own!");
            }
        }

        #region GetRandomRelic
        public static BaseMetaTalismanRelic GetRandomRelic()
	    {
	        BaseMetaTalismanRelic relic = null;

            Array values = Enum.GetValues(typeof(MetaTalismanSkillType));
            Random random = new Random();
            MetaTalismanSkillType RandomSkill = (MetaTalismanSkillType)values.GetValue(random.Next(values.Length));

	        switch (RandomSkill)
	        {
                case MetaTalismanSkillType.DoubleStrike:
                {
                    relic = new DoubleStrikeTalismanRelic();
                    break;
                }
				case MetaTalismanSkillType.InfectiousWounds:
				{
					relic = new InfectiousWoundsTalismanRelic();
					break;
				}
                case MetaTalismanSkillType.PhaseShift:
                {
                    relic = new PhaseShiftTalismanRelic();
                    break;
                }
                case MetaTalismanSkillType.Exsanguinate:
                {
                    relic = new ExsanguinateTalismanRelic();
                    break;
                }
	        }
	        return relic;
	    }
        #endregion

        public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

            int version = writer.SetVersion(0);

            switch (version)
            {
                case 0:
                {
                    writer.Write((int)MetaTalismanSkillType);
                    writer.Write(MetaTalismanSkillName);
                    writer.Write(NextLevelExperience);
                    writer.Write(MaxLevel);
                    writer.Write(ApplicationChance);
                    writer.Write(CoolDown);
                    writer.Write(SkillMulti);                 
                }
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
                {
                    MetaTalismanSkillType = (MetaTalismanSkillType) reader.ReadInt();
                    MetaTalismanSkillName = reader.ReadString();
                    NextLevelExperience = reader.ReadInt();
                    MaxLevel = reader.ReadInt();
                    ApplicationChance = reader.ReadDouble();
                    CoolDown = reader.ReadTimeSpan();
                    SkillMulti = reader.ReadDouble();
                }
                    break;
            }
		}
	}
}