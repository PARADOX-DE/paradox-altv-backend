using AltV.Net;
using PARADOX_RP.Game.Phone.Content.Team.Models;
using PARADOX_RP.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows.Phone.Applications.Team
{
    class TeamListAppWindow : Window
    {
        public TeamListAppWindow() : base("TeamListApp") { }
    }

    class TeamListAppWindowMemberWriter : IWritable
    {
        private List<TeamPhoneApplicationPlayer> Players;

        public TeamListAppWindowMemberWriter(List<TeamPhoneApplicationPlayer> players)
        {
            Players = players;
        }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginArray();
            foreach (var player in Players)
            {
                writer.Name("id");
                writer.Value(player.Id);

                writer.Name("name");
                writer.Value(player.Name);

                writer.Name("online");
                writer.Value(player.Online);
            }
            writer.EndArray();
        }
    }
}
