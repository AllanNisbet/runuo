#region References

using System;
using System.Collections.Generic;
using System.Linq;
using Server.Commands.Generic;
using Server.Items;
using Server.Items.MetaTalisman;
using Server.Items.MetaTalismanSkills;
using Server.Spells;
using Server.Spells.Ninjitsu;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Targeting;
using VitaNex;
using VitaNex.FX;
using VitaNex.Network;

#endregion

namespace Server.Items.MetaTalisman.Skills
{
    public class BaseMetaTalismanSkill : PropertyObject
    {
        public int _level;
        public virtual int Level { get { return _level; } set { _level = value; } }

        public string Name { get; set; }
		private Mobile _Owner;
        public int Experience { get; set; }
        public int NextLevelExperience { get; set; }
        public int MaxLevel { get; set; }

        public MetaTalismanSkillType MetaTalismanSkillType { get; set; }
        public double ChanceToActivate { get; set; }
        public TimeSpan CoolDown { get; set; }
        public double AbilityMultiplier { get; set; }

        public DateTime NextUse { get; set; }
		
        public BaseMetaTalismanSkill(MetaTalismanSkillType skilltype, string name, int nle, int maxlevel, double chance, TimeSpan cooldown,
            double multi)
        {
            Level = 1;
            Name = name;
            MetaTalismanSkillType = skilltype;
            Experience = 0;
            NextLevelExperience = nle;
            MaxLevel = maxlevel;
            ChanceToActivate = chance;
            CoolDown = cooldown;
            AbilityMultiplier = multi;
        }

        public BaseMetaTalismanSkill(GenericReader reader)
            : base(reader)
        {}

        public override void Reset()
        {}

        public override void Clear()
        {}

        public virtual void SetLevel()
        {}

        public virtual void FindAbility(BaseCreature target, BaseMetaTalisman talisman, Mobile attacker)
        {
            if (MetaTalismanSkillType.DoubleStrike == MetaTalismanSkillType && DateTime.UtcNow >= NextUse &&
            Utility.RandomDouble() <= ChanceToActivate)
            {
                DoAbilityDoubleStrike(target, attacker);
				//Console.WriteLine("Testing DoubleStrike trigger");
            }

            if (MetaTalismanSkillType.InfectiousWounds == MetaTalismanSkillType && DateTime.UtcNow >= NextUse &&
            Utility.RandomDouble() <= ChanceToActivate)
            {
                DoAbilityInfectiousWounds(target, attacker);
				//Console.WriteLine("Testing InfectiousWounds trigger");
            }

            if (MetaTalismanSkillType.PhaseShift == MetaTalismanSkillType && DateTime.UtcNow >= NextUse &&
            Utility.RandomDouble() <= ChanceToActivate)
            {
                DoAbilityPhaseShift(target, attacker);
				Console.WriteLine("Testing PhaseShift trigger");
            }

            if (MetaTalismanSkillType.Exsanguinate == MetaTalismanSkillType && DateTime.UtcNow >= NextUse &&
            Utility.RandomDouble() <= ChanceToActivate)
            {
                DoAbilityExsanguinate(target, attacker);
				//Console.WriteLine("Testing Exsanguinate trigger");
            }
        }

        #region DoubleStrike
        public void DoAbilityDoubleStrike(BaseCreature target, Mobile attacker)
        {          
			attacker.PublicOverheadMessage(MessageType.Label, 34, true, "Power flows through your veins.");
			
			IWeapon weapon = attacker.Weapon;

			target.PlaySound(0x3BB);
			target.FixedEffect(0x37B9, 244, 25);

			// Swing again:

			// If no combatant, wrong map, one of us is a ghost, or cannot see, or deleted, then stop combat
			if (target.Deleted || attacker.Deleted || target.Map != attacker.Map || !target.Alive ||
				!attacker.Alive || !attacker.CanSee(target))
			{
				attacker.Combatant = null;
				return;
			}

			if (weapon == null)
			{
				return;
			}

			if (!attacker.InRange(target, weapon.MaxRange))
			{
				return;
			}
			
			if (attacker.InLOS(target))
			{
				BaseWeapon.InDoubleStrike = true;
				attacker.RevealingAction();
				attacker.NextCombatTime = Core.TickCount + (int)weapon.OnSwing(attacker, target).TotalMilliseconds;
				BaseWeapon.InDoubleStrike = false;
			}
			
			Experience++;
			if (Experience >= NextLevelExperience)
			{
				LevelUpDoubleStrike(_Owner);
			}

			NextUse = DateTime.UtcNow + CoolDown;
        }

        public void LevelUpDoubleStrike(Mobile m)
        {
            if (Level < MaxLevel)
            {
                Level++;
                NextLevelExperience = Level * 200;
                Experience = 0;
                CoolDown = CoolDown.Subtract(TimeSpan.FromSeconds(1));
                ChanceToActivate += 0.01;

                if (m != null)
                {
                    m.SendMessage(54,
                        "Your Talisman's Double Strike ability has leveled up.  It is now Level: " + Level + ".");
                }
            }
        }
        #endregion

        #region InfectiousWounds
		public void DoAbilityInfectiousWounds(BaseCreature target, Mobile attacker)
        {
			attacker.SendMessage(54, "Infection floods from your talisman");
			attacker.PublicOverheadMessage(MessageType.Label, 34, true, "Infection floods from your talisman.");
            int range = Utility.RandomMinMax(3, 5);
            int zOffset = 10;

            Point3D src = attacker.Location.Clone3D(0, 0, zOffset);
            Point3D[] points = src.GetAllPointsInRange(attacker.Map, range, range);

            Effects.PlaySound(attacker.Location, attacker.Map, 0x19C);

            Timer.DelayCall(
                TimeSpan.FromMilliseconds(100),
                () =>
                {
                    foreach (Point3D trg in points)
                    {
                        int bloodID = Utility.RandomMinMax(4650, 4655);

                        new MovingEffectInfo(src, trg.Clone3D(0, 0, 2), attacker.Map, bloodID, 1367).MovingImpact(
                            info =>
                            {
                                var blood = new Blood
                                {
                                    ItemID = bloodID,
                                    Hue = 1368
                                };
                                blood.MoveToWorld(info.Target.Location, info.Map);

                                Effects.PlaySound(info.Target, info.Map, 0x028);
                            });
                    }
                    foreach (
                        Mobile mobile in
                            attacker.Location.GetMobilesInRange(attacker.Map, range)
                                .Where(m => m is BaseCreature && !m.IsControlled()).Take(4))
                    {
                        var num = (int) Math.Floor((float) Level / 2);
                        if (num == 5)
                            num = 4;
                        mobile.ApplyPoison(attacker, Poison.GetPoison(num));
                    }
                    Experience ++;
                    if (Experience >= NextLevelExperience)
                    {
                        LevelUpInfectiousWounds(_Owner);
                    }
                });
            NextUse = DateTime.UtcNow + CoolDown;
        }	
        
        public void LevelUpInfectiousWounds(Mobile m)
        {
            if (Level < MaxLevel)
            {
                Level++;
                NextLevelExperience = Level * 200;
                Experience = 0;
                CoolDown = CoolDown.Subtract(TimeSpan.FromSeconds(1));
                ChanceToActivate += 0.01;
                AbilityMultiplier += 1.0;

                if (m != null)
                {
                    m.SendMessage(54,
                        "Your Talisman's Infectious Wounds ability has leveled up.  It is now Level: " + Level + ".");
                } 
            }
        }
        #endregion

        #region PhaseShift

        public void DoAbilityPhaseShift(BaseCreature target, Mobile attacker)
        {
			Console.WriteLine("Testing PhaseShift ability - target: (" + target + ")");
			Console.WriteLine("Testing PhaseShift ability - talisman: (" + attacker + ")");
			attacker.PublicOverheadMessage(MessageType.Label, 34, true, "*PhaseShift*");
			
			//attacker.SendLocalizedMessage(1063088); // You prepare to perform a Shadowjump.
			
			IPoint3D p = target as IPoint3D;

			/* if (p != null)
				attacker.Target(p); */
				
            Target(p, attacker);
			
			Experience++;

			if (Experience >= NextLevelExperience)
			{
				LevelUpPhaseShift(_Owner);
			}
			NextUse = DateTime.UtcNow + CoolDown;
		}
		
		public void Target(IPoint3D p, Mobile attacker)
        {
            IPoint3D orig = p;
            Map map = attacker.Map;

            SpellHelper.GetSurfaceTop(ref p);

            Point3D from = attacker.Location;
            Point3D to = new Point3D(p);

            PlayerMobile pm = attacker as PlayerMobile; // IsStealthing should be moved to Server.Mobiles

            /* if (Server.Misc.WeightOverloading.IsOverloaded(this.attacker))
            {
                this.attacker.SendLocalizedMessage(502359, "", 0x22); // Thou art too encumbered to move.
            }
            else if (!SpellHelper.CheckTravel(this.attacker, TravelCheckType.TeleportFrom) || !SpellHelper.CheckTravel(this.attacker, map, to, TravelCheckType.TeleportTo))
            {
            }
            else if (map == null || !map.CanSpawnMobile(p.X, p.Y, p.Z))
            {
                this.attacker.SendLocalizedMessage(502831); // Cannot teleport to that spot.
            }
            else if (SpellHelper.CheckMulti(to, map, true, 5))
            {
                this.attacker.SendLocalizedMessage(502831); // Cannot teleport to that spot.
            }
            else if (Region.Find(to, map).GetRegion(typeof(HouseRegion)) != null)
            {
                this.attacker.SendLocalizedMessage(502829); // Cannot teleport to that spot.
            }
            else if (this.CheckSequence())
            { */
                SpellHelper.Turn(attacker, orig);

                Mobile m = attacker;

                m.Location = to;
                m.ProcessDelta();

                Effects.SendLocationParticles(EffectItem.Create(from, m.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);

                m.PlaySound(0x512);
				
                //Server.SkillHandlers.Stealth.OnUse(m); // stealth check after the a jump
            /* } */

            //this.FinishSequence();
        }

        /* public class InternalTarget : Target
        {
            private readonly Shadowjump m_Owner;
            public InternalTarget(Shadowjump owner, Mobile attacker)
                : base(11, true, TargetFlags.None)
            {
                this.m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                IPoint3D p = o as IPoint3D;

                if (p != null)
                    this.m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                this.m_Owner.FinishSequence();
            }
        }
 */
        public void LevelUpPhaseShift(Mobile m)
        {
            if (Level < MaxLevel)
            {
                Level++;
                NextLevelExperience = Level * 200;
                Experience = 0;
                CoolDown = CoolDown.Subtract(TimeSpan.FromSeconds(1));
                ChanceToActivate += 0.01;
                AbilityMultiplier += 1.0;

                if (m != null)
                {
                    m.SendMessage(54,
                        "Your Talisman's Phase Shift ability has leveled up.  It is now Level: " + Level + ".");
                }
            }
        }

        #endregion

        #region Exsanguinate

        public void DoAbilityExsanguinate(BaseCreature target, Mobile attacker)
        {
		    attacker.SendMessage(54, "You drain the blood of your victim");
			var timer = new InternalBleedTimer(attacker, target, Level);
            timer.Start();
            
			Experience++;
			if (Experience >= NextLevelExperience)
			{
				LevelUpExsanguinate(_Owner);
			}

			NextUse = DateTime.UtcNow + CoolDown;
        }

        private class InternalBleedTimer : Timer
        {
            private Mobile _tali;
            private BaseCreature _Target;
            private int _Count;
            private int _Level;

            public InternalBleedTimer(Mobile m, BaseCreature target, int level)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(0.5))
            {
                _tali = m;
                _Target = target;
                _Count = 0;
                _Level = level;
            }

            protected override void OnTick()
            {		
                if (_Count < 10)
                {
                    CreateBlood(_tali.Location, _tali.Map, false, _tali);
                    _Target.Damage(1 * _Level, _tali);
                    _Count++;
                }
                else
                {
                    Stop();
                }
            }
        }

        public static void CreateBlood(Point3D loc, Map map, bool delayed, Mobile m)
        {
            int bloodID = Utility.RandomMinMax(4650, 4655);
            new Blood(bloodID).MoveToWorld(new Point3D(BloodOffset(loc.X), BloodOffset(loc.Y), loc.Z), map);
        }

        public static int BloodOffset(int coord)
        {
            return coord + Utility.RandomMinMax(-1, 1);
        }


        public void LevelUpExsanguinate(Mobile m)
        {
            if (Level < MaxLevel)
            {
                Level++;
                NextLevelExperience = Level * 200;
                Experience = 0;
                CoolDown = CoolDown.Subtract(TimeSpan.FromSeconds(1));
                ChanceToActivate += 0.01;

                if (m != null)
                {
                    m.SendMessage(54,
                        "Your Talisman's Exsanguinate ability has leveled up.  It is now Level: " + Level + ".");
                }
            }
        }

        #endregion

        public override void Serialize(GenericWriter writer)
        {
            int version = writer.SetVersion(0);

            switch (version)
            {
                case 0:
                {
                    writer.Write(_level);
                    writer.Write(Name);
                    writer.Write((int) MetaTalismanSkillType);
                    writer.Write(ChanceToActivate);
                    writer.Write(CoolDown);
                    writer.Write(AbilityMultiplier);
                    writer.Write(Experience);
                    writer.Write(NextLevelExperience);
                    writer.Write(MaxLevel);
                }
                    break;
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            int version = reader.GetVersion();

            switch (version)
            {
                case 0:
                {
                    _level = reader.ReadInt();
                    Name = reader.ReadString();
                    MetaTalismanSkillType = (MetaTalismanSkillType) reader.ReadInt();
                    ChanceToActivate = reader.ReadDouble();
                    CoolDown = reader.ReadTimeSpan();
                    AbilityMultiplier = reader.ReadDouble();
                    Experience = reader.ReadInt();
                    NextLevelExperience = reader.ReadInt();
                    MaxLevel = reader.ReadInt();
                }
                    break;
            }
        }
    }
}