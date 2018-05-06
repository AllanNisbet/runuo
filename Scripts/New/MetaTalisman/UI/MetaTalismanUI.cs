using System;
using System.Collections.Generic;
//using Server.Engines.EventScores;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Items.MetaTalisman.Skills;
using Server.Items.MetaTalismanSkills;
using VitaNex.SuperGumps;
using VitaNex.SuperGumps.UI;

namespace Server.Items.MetaTalisman
{
    public sealed class MetaTalismanUI : ListGump<BaseMetaTalismanSkill>
    {
		public BaseMetaTalisman MetaTalisman { get; set; }
		
        public MetaTalismanUI(PlayerMobile user, BaseMetaTalisman tali, Gump parent = null)
            : base( user, parent )
        {
            CanDispose = true;
            CanMove = true;
            EntriesPerPage = 4;
            //MetaPet = pet;
            MetaTalisman = tali;
            AutoRefresh = true;
            ForceRecompile = true;
            AutoRefreshRate = TimeSpan.FromSeconds(5.0);
            CanResize = false;
            CanClose = true;
        }
		
		protected override void CompileList(List<BaseMetaTalismanSkill> list)
        {
            list.Clear();
            list.AddRange(MetaTalisman.MetaTalismanskills.Values);
            base.CompileList(list);
        }
		
        protected override void CompileLayout(SuperGumpLayout layout)
        {
            layout.Add(
                "background",
                () =>
                {
                    AddBackground(0, 0, 426, 338, 9380);
                    AddBackground(15, 30, 397, 283, 3500);
                });

            layout.Add(
                "TalismanIcon",
                () =>
                {
                    AddLabel(40, 50, 2059, @"Talisman");
					AddItem(40, 70, 12123, MetaTalisman.Hue);
                });

            layout.Add(
                "Stage",
                () =>
                {
                    AddLabel(40, 115, 0, @"Stage");
                    AddLabel(40, 135, 0, MetaTalisman.Stage + "/" + MetaTalisman.MaxStage);

                    const int width = 109;

                    var wOffset = (int) Math.Ceiling(width * (MetaTalisman.Stage / (double) MetaTalisman.MaxStage));

                    if (wOffset >= width)
                    {
                        AddImageTiled(35, 155, width, 17, 2062);
                    }
                    else
                    {
                        AddImageTiled(35, 155, width, 17, 2061);

                        if (wOffset > 0)
                        {
                            AddImageTiled(35, 155, wOffset, 17, 2062);
                        }
                    }
                });

            layout.Add(
                "Progress",
                () =>
                {
                    AddLabel(40, 180, 0, @"Current Progress");
                    if (MetaTalisman.Stage != MetaTalisman.MaxStage)
                    {
                        AddLabel(40, 200, 0, MetaTalisman.EvoXp + "/" + MetaTalisman.NextEvolution);

                        const int width = 109;

                        var wOffset =
                            (int) Math.Ceiling(width * (MetaTalisman.EvoXp / (double) MetaTalisman.NextEvolution));

                        if (wOffset >= width)
                        {
                            AddImageTiled(35, 220, width, 17, 2062);
                        }
                        else
                        {
                            AddImageTiled(35, 220, width, 17, 2061);

                            if (wOffset > 0)
                            {
                                AddImageTiled(35, 220, wOffset, 17, 2062);
                            }
                        }
                    }
                    else
                    {
                        AddLabel(40, 200, 0, "MAX");
                        AddImageTiled(35, 220, 109, 17, 2062);
                    }
                });

            layout.Add(
                "AbilitySlots",
                () =>
                {
                    AddLabel(40, 245, 0, @"Ability Slots");
                    if (MetaTalisman.MaxAbilities > 0)
                    {
                        AddLabel(40, 265, 0, MetaTalisman.CurrentAbilities + "/" + MetaTalisman.MaxAbilities);
                    }
                    else
                    {
                        AddLabel(40, 265, 0, "No ability slots available.");
                    }
                });

            layout.Add(
                "Skills",
                () =>
                {
                    AddLabel(205, 50, 2059, @"Talisman-Skills");
                    AddLabel(355, 49, 2059, @"Level");
                    AddLabel(235, 283, 1258, @"Apply Relic?");
                    AddButton(319, 282, 247, 248, b =>
                    {
                        MetaTalisman.GetTargetRelic(User);
                        Refresh(true);
                    });

                    if (MetaTalisman.MetaTalismanskills.Count == 0)
                    {
                        AddLabel(205, 80, 0, "No learned skills.");
                    }
                });

            Dictionary<int, BaseMetaTalismanSkill> range = GetListRange();


            if (range.Count > 0)
            {
                CompileEntryLayout(layout, range);
            }
        }

        protected override void CompileEntryLayout(
            SuperGumpLayout layout, int length, int index, int pIndex, int yOffset, BaseMetaTalismanSkill entry)
        {
            yOffset = 80 + pIndex * 50;

            layout.Add(
                "entry" + index,
                () =>
                {
                    AddLabel(205, yOffset, 0, entry.Name);
                    AddButton(309, yOffset, 56, 56, b => new DialogGump(User, this, 0, 0, null, null, 7004, a =>
                    {
                        if (MetaTalisman != null && MetaTalisman.MetaTalismanskills.ContainsKey(entry.MetaTalismanSkillType))
                        {
                            MetaTalisman.MetaTalismanskills.Remove(entry.MetaTalismanSkillType);
                            MetaTalisman.CurrentAbilities--;
                            Refresh(true);
                        }
                    }, c => { Refresh(true); })
                    {
                        Title = "Confirm Skill Deletion",
                        Html = "Are you sure you wish to delete this skill?  All progress in the skill will be lost.",
                        HtmlColor = DefaultHtmlColor
                    }.Send());
                    AddLabel(355, yOffset, 0, entry.Level + "/" + entry.MaxLevel);
                    AddImageTiled(215, yOffset + 20, 111, 11, 2053);

                    const int width = 111;

                    var wOffset = (int) Math.Ceiling(width * (entry.Experience / (double) entry.NextLevelExperience));

                    if (wOffset >= width)
                    {
                        AddImageTiled(215, yOffset + 20, width, 11, 2057);
                    }
                    else
                    {
                        AddImageTiled(215, yOffset + 20, width, 11, 2053);

                        if (wOffset > 0)
                        {
                            AddImageTiled(215, yOffset + 20, wOffset, 11, 2056);
                        }
                    }
                });
        }   
    }
}
