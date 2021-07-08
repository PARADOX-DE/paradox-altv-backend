using AltV.Net.Async;
using AltV.Net.Data;
using EntityStreamer;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Game.Inventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Inventory.Content
{
    class ExampleItemScript : IItemScript
    {
        public string ScriptName => "example_itemscript";

        public Task<bool> UseItem(PXPlayer player) 
        {
            /*
             * Diese Funktion wird ausgeführt, wenn ein Item mit diesem ItemScript 
             * benutzt wird!
             */

            // Funktion Examples:

            /* => Progressbar:
                bool finishedProgressbar = await player.RunProgressBar(async () =>
                {
                    await player.SetArmorAsync(100);
                    // Das passiert wenn die 4 Sekunden vorbei sind.
                }, "Schutzweste", "Du ziehst nun eine Schutzweste...", 4 * 1000); // 4 Sekunden
                // finishedProgressbar gibt einen Boolean zurück, ob die Progressbar durchgezogen wurde, oder abgebrochen (true = finished) (false = abgebrochen)
             */

            /* => Object Streamer
                PropStreamer.Create("object_model", new Position(100.0f, 4045.45f, 34.000f), new Vector3(0, 0, 0));
            */
            return Task.FromResult(false); // false => Item wird nicht entfernt beim benutzen | true => wird beim benutzen entfernt.
        }
    }
}
