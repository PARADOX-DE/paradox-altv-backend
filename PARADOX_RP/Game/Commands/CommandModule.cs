using AltV.Net;
using AltV.Net.Elements.Args;
using AltV.Net.Elements.Entities;
using AltV.Net.FunctionParser;
using PARADOX_RP.Core.Module;
using PARADOX_RP.Game.Commands.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace PARADOX_RP.Game.Commands
{
    class CommandModule : ModuleBase<CommandModule>, IModule
    {
        public CommandModule() : base("Command") { }

		public delegate void CommandDoesNotExistsDelegate(IPlayer player, string command);

		internal static readonly HashSet<CommandDoesNotExistsDelegate> CommandDoesNotExistsDelegates = new HashSet<CommandDoesNotExistsDelegate>();

		public static event CommandDoesNotExistsDelegate OnCommandDoesNotExists
		{
			add
			{
				CommandDoesNotExistsDelegates.Add(value);
			}
			remove
			{
				CommandDoesNotExistsDelegates.Remove(value);
			}
		}

		private delegate void CommandDelegate(IPlayer player, string[] arguments);

		private static readonly LinkedList<Function> Functions = new LinkedList<Function>();

		private static readonly LinkedList<GCHandle> Handles = new LinkedList<GCHandle>();

		private readonly IDictionary<string, LinkedList<CommandDelegate>> commandDelegates = new Dictionary<string, LinkedList<CommandDelegate>>();

		private static readonly string[] EmptyArgs = new string[0];

		public void OnScriptsStarted(IScript[] scripts)
		{
			foreach (IScript target in scripts)
			{
				RegisterEvents(target);
			}
			Alt.OnClient<IPlayer, string>("chat:message", OnChatMessage, OnChatMessageParser);
		}

		private static void OnChatMessageParser(IPlayer player, MValueConst[] mValueArray, Action<IPlayer, string> action)
		{
			if (mValueArray.Length == 1)
			{
				MValueConst mValueConst = mValueArray[0];
				if (mValueConst.type == MValueConst.Type.String)
				{
					action(player, mValueConst.GetString());
				}
			}
		}

        private void OnChatMessage(IPlayer player, string message)
		{
			if (string.IsNullOrEmpty(message) || !message.StartsWith("/"))
			{
				return;
			}
			message = message.Trim().Remove(0, 1);
			if (message.Length <= 0)
			{
				return;
			}
			string[] array = message.Split(' ');
			int num = array.Length;
			if (num < 1)
			{
				return;
			}
			string text = array[0];
			LinkedList<CommandDelegate> value;
			if (num < 2)
			{
				if (commandDelegates.TryGetValue(text, out value) && value.Count > 0)
				{
					foreach (CommandDelegate item in value)
					{
						item(player, EmptyArgs);
					}
				}
				else
				{
					foreach (CommandDoesNotExistsDelegate commandDoesNotExistsDelegate in CommandDoesNotExistsDelegates)
					{
						commandDoesNotExistsDelegate(player, text);
					}
				}
				return;
			}
			string[] array2 = new string[num - 1];
			Array.Copy(array, 1, array2, 0, num - 1);
			if (commandDelegates.TryGetValue(text, out value) && value.Count > 0)
			{
				foreach (CommandDelegate item2 in value)
				{
					item2(player, array2);
				}
			}
			else
			{
				foreach (CommandDoesNotExistsDelegate commandDoesNotExistsDelegate2 in CommandDoesNotExistsDelegates)
				{
					commandDoesNotExistsDelegate2(player, text);
				}
			}
		}

        public void OnStop()
		{
			Functions.Clear();
			foreach (GCHandle handle in Handles)
			{
				handle.Free();
			}
			Handles.Clear();
		}

		private void RegisterEvents(object target)
		{
			ModuleScriptMethodIndexer.Index(target, new Type[2]
			{
			typeof(Command),
			typeof(CommandEvent)
			}, delegate (object baseEvent, MethodInfo eventMethod, Delegate eventMethodDelegate)
            {
                Command command = baseEvent as Command;
                if (command != null)
                {
                    string key = command.Name ?? eventMethod.Name;
                    Handles.AddLast(GCHandle.Alloc(eventMethodDelegate));
                    Function function = Function.Create(eventMethodDelegate);
                    if (function == null)
                    {
                        Alt.Log("Unsupported Command method: " + eventMethod?.ToString());
                    }
                    else
                    {
                        Functions.AddLast(function);
                        if (!commandDelegates.TryGetValue(key, out LinkedList<CommandDelegate> value))
                        {
                            value = new LinkedList<CommandDelegate>();
                            commandDelegates[key] = value;
                        }
                        if (command.GreedyArg)
                        {
                            value.AddLast(delegate (IPlayer player, string[] arguments)
                            {
                                function.Call(player, new string[1]
                                {
                                string.Join(" ", arguments)
                                });
                            });
                        }
                        else
                        {
                            value.AddLast(delegate (IPlayer player, string[] arguments)
                            {
                                function.Call(player, arguments);
                            });
                        }
                        string[] aliases = command.Aliases;
                        if (aliases != null)
                        {
                            string[] array = aliases;
                            foreach (string key2 in array)
                            {
                                if (!commandDelegates.TryGetValue(key2, out value))
                                {
                                    value = new LinkedList<CommandDelegate>();
                                    commandDelegates[key2] = value;
                                }
                                if (command.GreedyArg)
                                {
                                    value.AddLast((CommandDelegate)delegate (IPlayer player, string[] arguments)
                                    {
                                        function.Call(player, new string[1]
                                        {
                                        string.Join(" ", arguments)
                                        });
                                    });
                                }
                                else
                                {
                                    value.AddLast((CommandDelegate)delegate (IPlayer player, string[] arguments)
                                    {
                                        function.Call(player, arguments);
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    CommandEvent commandEvent = baseEvent as CommandEvent;
                    if (commandEvent != null)
                    {
                        if (commandEvent.EventType == CommandEventType.CommandNotFound)
                        {
                            ScriptFunction scriptFunction = ScriptFunction.Create(eventMethodDelegate, new Type[2]
                            {
                            typeof(IPlayer),
                            typeof(string)
                            });
                            if (scriptFunction != null)
                            {
                                OnCommandDoesNotExists += delegate (IPlayer player, string commandName)
                                {
                                    scriptFunction.Set(player);
                                    scriptFunction.Set(commandName);
                                    scriptFunction.Call();
                                };
                            }
                        }
                    }
                }
            });
		}
	}
}
