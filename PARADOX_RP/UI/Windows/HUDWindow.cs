using AltV.Net;
using PARADOX_RP.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows
{
    class HUDWindow : Window
    {
        public HUDWindow() : base("HUD") { }
    }

    class HUDWindowWriter : IWritable
    {
        private int _charId;
        private string _charName;
        private int _money;

        public HUDWindowWriter(int charId, string charName, int money)
        {
            _charId = charId;
            _charName = charName;
            _money = money;
        }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();
            writer.Name("id");
            writer.Value(_charId);
            writer.Name("charName");
            writer.Value(_charName);
            writer.Name("money");
            writer.Value(_money);
            writer.EndObject();
        }
    }
}
