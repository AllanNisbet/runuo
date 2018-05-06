#region References

using System;
using System.Collections.Generic;
using System.Linq;
using Server.Items;
using Server.Mobiles;
using VitaNex.Targets;
using Server.Spells;
using Server.Items.MetaTalisman;
using Server.Items.MetaTalisman.Skills;
using Server.Items.MetaTalismanSkills;
using Server.Network;

#endregion

namespace Server.Items.MetaTalisman
{
    public class BaseMetaTalisman : BaseTalisman
    {
        public Dictionary<MetaTalismanSkillType, BaseMetaTalismanSkill> MetaTalismanskills;
			
        private int _EvoXp;

        private int _Stage;
		
		private BaseMetaTalisman tali { get; set; }
				
        [CommandProperty(AccessLevel.GameMaster)]
        public int EvoXp
        {
            get { return _EvoXp; }
            set
            {	
                _EvoXp = value;
                if (CanEvolve())
                {
                    _Stage++;
                    Evolve();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int NextEvolution { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxStage { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Stage
        {
            get { return _Stage; }
            set
            {
                if (value > 0 && value <= MaxStage && value != _Stage)
                {
                    _Stage = value;
                    Evolve();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxAbilities { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int CurrentAbilities { get; set; }

		[Constructable]
		public BaseMetaTalisman()
			: base(12123)
        {
			_Stage = 1;
            Name = "<BASEFONT COLOR=#ffffff>Quest Talisman";
			Hue = 0;
			Weight = 0.0;
			Stackable = false;
            LootType = LootType.Blessed;
			Layer = Layer.Talisman;
			NextEvolution = 25000;
            MaxStage = 7;
            MetaTalismanskills = new Dictionary<MetaTalismanSkillType, BaseMetaTalismanSkill>();
        }

        public BaseMetaTalisman(Serial serial)
            : base(serial)
        {}
		
		public virtual void Evolve()
        {
            //Effects.SendFlashEffect(this, (FlashType) 2);
        }
		
		public virtual void Addpoints(BaseCreature creature, int points)
        {		
            if (Stage < MaxStage)
            {
                EvoXp += points;
                if (CanEvolve())
                {
                    _Stage++;
                    Console.WriteLine("Your Talisman begins to pulsate with power!");
                    Timer.DelayCall(TimeSpan.FromSeconds(30), Evolve);
                }
            }
        }
		
		public virtual void DoAbility(MetaTalismanSkillType skill, BaseCreature target, Mobile attacker)
        {
			if (MetaTalismanskills != null && MetaTalismanskills.ContainsKey(skill))
            {
				//Console.WriteLine("doability test @ BaseMetaTalisman.cs : (" + target + ")");
                BaseMetaTalismanSkill metatalismanskill = MetaTalismanskills[skill];
                metatalismanskill.FindAbility(target, this, attacker); 
            }
        }
		
		public virtual bool CanEvolve()
        {
            return false;
        }
		
		public void GetTargetRelic(Mobile user)
        {
            if (user != null && !user.Deleted)
            {
                user.Target = new ItemSelectTarget<Item>(ConsumeRelic, m => { });
            }
        }
		
        public void ConsumeRelic(Mobile m, Item item)
        {
			Item tali = m.FindItemOnLayer( Layer.Talisman );
			//Console.WriteLine("Testing tali: (" + tali + ")");
            if (!item.IsChildOf(m.Backpack))
            {
                m.SendMessage(54, "The relic must be in your backpack for you to use it!");
                return;
            }
            if (item is BaseMetaTalismanRelic)
            {
                var relic = item as BaseMetaTalismanRelic;
                relic.GetMetaTalisman(m, tali);
            }
            else
            {
                m.SendMessage(54, "You must use this on a relic!");
            }
        }
		
		public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
			writer.Write(_Stage);
            writer.Write(_EvoXp);
            writer.Write(NextEvolution);
            writer.Write(MaxStage);
            writer.Write(MaxAbilities);
            writer.Write(CurrentAbilities); 
			
			writer.WriteDictionary(
                MetaTalismanskills,
                (t, s) =>
                {
                    writer.Write((int)t);

                    s.Serialize(writer);
                });
        }

        public override void Deserialize(GenericReader reader)
        {
            MetaTalismanskills = new Dictionary<MetaTalismanSkillType, BaseMetaTalismanSkill>();
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                {
                    _Stage = reader.ReadInt();
                    _EvoXp = reader.ReadInt();
                    NextEvolution = reader.ReadInt();
                    MaxStage = reader.ReadInt();
                    MaxAbilities = reader.ReadInt();
                    CurrentAbilities = reader.ReadInt();
					
					MetaTalismanskills = reader.ReadDictionary(
                        () =>
                        {
                            var c = (MetaTalismanSkillType) reader.ReadInt();

                            var s = new BaseMetaTalismanSkill(reader);

                            return new KeyValuePair<MetaTalismanSkillType, BaseMetaTalismanSkill>(c, s);
                        });
                }
                    break;
            }
        }
    }
}

