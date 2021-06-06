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

    class TeamListAppWindowWriter : IWritable
    {
        private string TeamName;
        private List<TeamPhoneApplicationPlayer> Players;
        private bool IsLeader;

        public TeamListAppWindowWriter(string teamName, List<TeamPhoneApplicationPlayer> players, bool isLeader)
        {
            TeamName = teamName;
            Players = players;
            IsLeader = isLeader;
        }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();

            writer.Name("name");
            writer.Value(TeamName);

            writer.Name("members");
            writer.BeginArray();
            foreach (var player in Players)
            {
                writer.BeginObject();

                writer.Name("id");
                writer.Value(player.Id);

                writer.Name("name");
                writer.Value(player.Name);

                writer.Name("online");
                writer.Value(player.Online);

                writer.EndObject();
            }
            writer.EndArray();

            writer.Name("isLeader");
            writer.Value(IsLeader);

            writer.EndObject();
        }
    }
}
