#region Header
//   Vorspire    _,-'/-'/  Utility.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2016  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
using System.Drawing;
using System.Text;

using Server;
using Server.Commands;
#endregion

namespace VitaNex.Modules.TrashCollection
{
	public static class TrashCollectionGumpUtility
	{
		public static StringBuilder GetHelpText(Mobile m)
		{
			var help = new StringBuilder();

			help.AppendFormat("<basefont color=#{0:X6}>", Color.SkyBlue.ToArgb());
			help.AppendLine("The Trash Collection service allows you to trash items in exchange for currency (usually tokens).");

			if (TrashCollection.CMOptions.DailyLimit > 0)
			{
				help.AppendLine();
				help.AppendFormat("<basefont color=#{0:X6}>", Color.Orange.ToArgb());
				help.AppendLine(
					String.Format(
						"There is a daily limit of {0} tokens, when you reach this limit you can still trash items, but will not receive any tokens.",
						TrashCollection.CMOptions.DailyLimit.ToString("#,#")));
			}

			help.AppendLine();
			help.AppendFormat("<basefont color=#{0:X6}>", Color.Yellow.ToArgb());
			help.AppendLine("Everything you successfully trash will be logged to your personal trash profile.");
			help.AppendLine("These logs are separated by day and can be viewed at any time.");

			if (!String.IsNullOrWhiteSpace(TrashCollection.CMOptions.ProfilesCommand))
			{
				help.AppendLine();
				help.AppendFormat("<basefont color=#{0:X6}>", Color.YellowGreen.ToArgb());
				help.AppendLine(
					String.Format(
						"To view trash profiles, use the <big>{0}{1}</big> command.",
						CommandSystem.Prefix,
						TrashCollection.CMOptions.ProfilesCommand));

				if (m.AccessLevel >= TrashCollection.Access)
				{
					help.AppendLine("You have access to administrate the trash system, you can also manage profiles.");
				}
			}

			if (m.AccessLevel >= TrashCollection.Access)
			{
				if (!String.IsNullOrWhiteSpace(TrashCollection.CMOptions.AdminCommand))
				{
					help.AppendLine();
					help.AppendFormat("<basefont color=#{0:X6}>", Color.LimeGreen.ToArgb());
					help.AppendLine(
						String.Format(
							"To administrate the trash system, use the <big>{0}{1}</big> command.",
							CommandSystem.Prefix,
							TrashCollection.CMOptions.AdminCommand));
				}
			}

			return help;
		}
	}
}