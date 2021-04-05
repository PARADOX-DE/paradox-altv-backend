using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Game.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Command : Attribute
    {
        public string Name { get; }

        public bool GreedyArg { get; }

        public string[] Aliases { get; }

        public Command(string name = null, bool greedyArg = false, string[] aliases = null)
        {
            Name = name;
            GreedyArg = greedyArg;
            Aliases = aliases;
        }
    }
}
