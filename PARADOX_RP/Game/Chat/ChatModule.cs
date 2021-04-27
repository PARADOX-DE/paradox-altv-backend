using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Administration;
using PARADOX_RP.Game.MiniGames;
using PARADOX_RP.Game.MiniGames.Models;
using AltV.Net.Enums;
using PARADOX_RP.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using AltV.Net.Data;
using PARADOX_RP.Core.Extensions;
using System.Linq;
using PARADOX_RP.Utils;
using PARADOX_RP.Game.Team;
using System.Runtime.InteropServices;
using AltV.Net.Elements.Args;
using AltV.Net.FunctionParser;
using PARADOX_RP.Game.Commands.Attributes;
using PARADOX_RP.Controllers.Event.Interface;

namespace PARADOX_RP.Game.Commands
{
    class ChatModule : ModuleBase<ChatModule>
    {

        public ChatModule(IEnumerable<ICommand> commands, IEventController eventController) : base("Chat")
        {
            foreach (var commandModule in commands)
            {
                RegisterEvents(commandModule);
            }

            eventController.OnClient<IPlayer, string>("chat:message", OnChatMessage, OnChatMessageParser);
        }

        private delegate void CommandDelegate(IPlayer player, string[] arguments);

        private static readonly LinkedList<Function> Functions = new LinkedList<Function>();

        private static readonly LinkedList<GCHandle> Handles = new LinkedList<GCHandle>();

        private readonly IDictionary<string, LinkedList<CommandDelegate>> commandDelegates =
            new Dictionary<string, LinkedList<CommandDelegate>>();

        private static readonly string[] EmptyArgs = new string[0];

        private static void OnChatMessageParser(IPlayer player, MValueConst[] mValueArray,
            Action<IPlayer, string> action)
        {
            if (mValueArray.Length != 1) return;
            var arg = mValueArray[0];
            if (arg.type != MValueConst.Type.String) return;
            action(player, arg.GetString());
        }

        private void OnChatMessage(IPlayer player, string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            if (!message.StartsWith("/")) return;
            message = message.Trim().Remove(0, 1);
            if (message.Length > 0)
            {
                var args = message.Split(' ');
                var argsLength = args.Length;
                if (argsLength < 1) return;
                var cmd = args[0];
                LinkedList<CommandDelegate> delegates;
                if (argsLength < 2)
                {
                    if (commandDelegates.TryGetValue(cmd, out delegates) && delegates.Count > 0)
                    {
                        foreach (var commandDelegate in delegates)
                        {
                            commandDelegate(player, EmptyArgs);
                        }
                    }
                    else
                    {
                        foreach (var doesNotExistsDelegate in AltChat.CommandDoesNotExistsDelegates)
                        {
                            doesNotExistsDelegate(player, cmd);
                        }
                    }

                    return;
                }

                var argsArray = new string[argsLength - 1];
                Array.Copy(args, 1, argsArray, 0, argsLength - 1);
                if (commandDelegates.TryGetValue(cmd, out delegates) && delegates.Count > 0)
                {
                    foreach (var commandDelegate in delegates)
                    {
                        commandDelegate(player, argsArray);
                    }
                }
                else
                {
                    foreach (var doesNotExistsDelegate in AltChat.CommandDoesNotExistsDelegates)
                    {
                        doesNotExistsDelegate(player, cmd);
                    }
                }
            }
        }

        public void OnStop()
        {
            Functions.Clear();

            foreach (var handle in Handles)
            {
                handle.Free();
            }

            Handles.Clear();
        }

        private void RegisterEvents(object target)
        {
            ModuleScriptMethodIndexer.Index(target, new[] { typeof(Command), typeof(CommandEvent) },
                (baseEvent, eventMethod, eventMethodDelegate) =>
                {
                    if (baseEvent is Command command)
                    {
                        var commandName = command.Name ?? eventMethod.Name;
                        Handles.AddLast(GCHandle.Alloc(eventMethodDelegate));
                        var function = Function.Create(eventMethodDelegate);
                        if (function == null)
                        {
                            Alt.Log("Unsupported Command method: " + eventMethod);
                            return;
                        }

                        Functions.AddLast(function);

                        LinkedList<CommandDelegate> delegates;
                        if (!commandDelegates.TryGetValue(commandName, out delegates))
                        {
                            delegates = new LinkedList<CommandDelegate>();
                            commandDelegates[commandName] = delegates;
                        }

                        if (command.GreedyArg)
                        {
                            delegates.AddLast((player, arguments) =>
                            {
                                function.Call(player, new[] { string.Join(" ", arguments) });
                            });
                        }
                        else
                        {
                            delegates.AddLast((player, arguments) => { function.Call(player, arguments); });
                        }

                        var aliases = command.Aliases;
                        if (aliases != null)
                        {
                            foreach (var alias in aliases)
                            {
                                if (!commandDelegates.TryGetValue(alias, out delegates))
                                {
                                    delegates = new LinkedList<CommandDelegate>();
                                    commandDelegates[alias] = delegates;
                                }

                                if (command.GreedyArg)
                                {
                                    delegates.AddLast((player, arguments) =>
                                    {
                                        function.Call(player, new[] { string.Join(" ", arguments) });
                                    });
                                }
                                else
                                {
                                    delegates.AddLast((player, arguments) => { function.Call(player, arguments); });
                                }
                            }
                        }
                    }
                    else if (baseEvent is CommandEvent commandEvent)
                    {
                        var commandEventType = commandEvent.EventType;
                        ScriptFunction scriptFunction;
                        switch (commandEventType)
                        {
                            case CommandEventType.CommandNotFound:
                                scriptFunction = ScriptFunction.Create(eventMethodDelegate,
                                    new[] { typeof(IPlayer), typeof(string) });
                                if (scriptFunction == null) return;
                                AltChat.OnCommandDoesNotExists += (player, commandName) =>
                                {
                                    scriptFunction.Set(player);
                                    scriptFunction.Set(commandName);
                                    scriptFunction.Call();
                                };
                                break;
                        }
                    }
                });
        }
    }

    public static class AltChat
    {
        internal static readonly HashSet<CommandDoesNotExistsDelegate> CommandDoesNotExistsDelegates =
            new HashSet<CommandDoesNotExistsDelegate>();

        public delegate void CommandDoesNotExistsDelegate(IPlayer player, string command);

        public static event CommandDoesNotExistsDelegate OnCommandDoesNotExists
        {
            add => CommandDoesNotExistsDelegates.Add(value);
            remove => CommandDoesNotExistsDelegates.Remove(value);
        }

        public static void SendBroadcast(string message)
        {
            Alt.EmitAllClients("chat:message", null, message);
        }
    }
}
