using AltV.Net;
using AltV.Net.Async;
using Newtonsoft.Json;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.UI.Models;
using PARADOX_RP.UI.Windows.NativeMenu.Interface;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.UI.Windows.NativeMenu
{
    class NativeMenuWindow : Window
    {
        private readonly Dictionary<Type, INativeMenu> _nativeMenus = new Dictionary<Type, INativeMenu>();

        public NativeMenuWindow(IEnumerable<INativeMenu> nativeMenus) : base("NativeMenu")
        {
            nativeMenus.ForEach(menu =>
            {
                _nativeMenus[menu.GetType()] = menu;
            });
            AltAsync.Log("[+] Initializing >> Successfully initialized static native menus!");
        }

        public void DisplayMenu<T>(PXPlayer player) where T : INativeMenu
        {
            if (!_nativeMenus.TryGetValue(typeof(T), out var component)) return;
            player.CurrentNativeMenu = component;

            Show(player, new NativeMenuWindowWriter(component.Title, component.Description, component.Items));
            AltAsync.Log(component.Title);
        }
    }

    class NativeMenuWindowWriter : IWritable
    {
        public NativeMenuWindowWriter(string title, string description, List<NativeMenuItem> items)
        {
            Title = title;
            Description = description;
            Items = items;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public List<NativeMenuItem> Items { get; set; }

        public void OnWrite(IMValueWriter writer)
        {
            writer.BeginObject();

            writer.Name("Title");
            writer.Value(Title);

            writer.Name("Description");
            writer.Value(Description);

            writer.Name("Items");
            writer.BeginArray();

            foreach (NativeMenuItem Item in Items)
            {
                writer.BeginObject();

                writer.Name("Text");
                writer.Value(Item.Text);

                writer.Name("Type");
                writer.Value(Enum.GetName(typeof(NativeMenuItemTypes), Item.Type));

                writer.Name("Selected");
                writer.Value(Item.Selected);

                writer.EndObject();
            }

            writer.EndArray();

            writer.EndObject();
        }
    }
}
