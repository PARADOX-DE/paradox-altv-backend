using AltV.Net.Async;
using PARADOX_RP.Core.Database;
using PARADOX_RP.Core.Database.Models;
using PARADOX_RP.Core.Events;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Misc.Progressbar.Extensions;
using PARADOX_RP.Utils;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PARADOX_RP.Game.Easteregg
{
    class EasterEggModule : Module<EasterEggModule>, IEventKeyPressed, IEventPlayerConnect
    {
        private Dictionary<int, Eastereggs> _easterEggs = new Dictionary<int, Eastereggs>();
        public EasterEggModule(PXContext pxContext) : base("EasterEgg")
        {
            LoadDatabaseTable(pxContext.Eastereggs, (Eastereggs easterEgg) =>
            {
                _easterEggs.Add(easterEgg.Id, easterEgg);
            });
        }

        public void OnPlayerConnect(PXPlayer player)
        {
            if (Configuration.Instance.DevMode)
            {
                _easterEggs.ForEach((e) =>
                {
                    player.AddBlips($"EasterEgg #{e.Key}", e.Value.Position, 515, 0, 1, true);
                });
            }
        }

        public async Task<bool> OnKeyPress(PXPlayer player, KeyEnumeration key)
        {
            if (key == KeyEnumeration.E)
            {
                Eastereggs easterEgg = _easterEggs.FirstOrDefault(e => e.Value.Position.Distance(player.Position) <= 3).Value;
                if (easterEgg == null) return await Task.FromResult(false);

                await player.PlayAnimation(easterEgg.AnimationDictionary, easterEgg.AnimationName);
                await player.RunProgressBar(async () =>
                {
                    await player.AddMoney(easterEgg.Money);
                    player.SendNotification(easterEgg.Name, "Danke, hat echt Spaß gemacht. Hier nimm das.", NotificationTypes.SUCCESS);
                    
                    await player.StopAnimation();
                }, easterEgg.Name, "Easter-Egg Auftrag", easterEgg.Duration);

                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
    }
}
