using AltV.Net;
using AltV.Net.Async;
using AltV.Net.FunctionParser;
using PARADOX_RP.Controllers.Event.Interface;
using PARADOX_RP.Core.Extensions;
using PARADOX_RP.Core.Factories;
using PARADOX_RP.Core.Interface;
using PARADOX_RP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PARADOX_RP.Controllers.Event
{
    class EventController : IEventController
    {
        private List<string> _whitelistedEvents = new List<string>();

        public EventController() { }

        [ScriptEvent(ScriptEventType.PlayerEvent)]
        public void EventReceived(PXPlayer player, string eventName, params object[] args)
        {
            string targetEvent = _whitelistedEvents.FirstOrDefault(e => e == eventName);
            if (targetEvent != null) return;

            AltAsync.Log($"[EventController] Received unknown / unregistered event ({eventName})");
            // add logger
        }

        public void WhitelistEvent(string eventName)
        {
            _whitelistedEvents.Add(eventName);
        }

        public void OnClient(string eventName, Action action)
        {
            AltAsync.OnClient(eventName, action);
            WhitelistEvent(eventName);
        }

        public void OnClient<T1>(string eventName, Action<T1> action)
        {
            AltAsync.OnClient(eventName, action);
            WhitelistEvent(eventName);
        }

        public void OnClient<T1, T2>(string eventName, Action<T1, T2> action)
        {
            AltAsync.OnClient(eventName, action);
            WhitelistEvent(eventName);
        }

        public void OnClient<T1, T2>(string eventName, Action<T1, T2> action, ClientEventParser<Action<T1, T2>> parser)
        {
            Alt.OnClient(eventName, action, parser);
            WhitelistEvent(eventName);
        }

        public void OnClient<T1, T2, T3>(string eventName, Action<T1, T2, T3> action)
        {
            AltAsync.OnClient(eventName, action);
            WhitelistEvent(eventName);
        }

        public void OnClient<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> action)
        {
            AltAsync.OnClient(eventName, action);
            WhitelistEvent(eventName);
        }

        public void OnClient<T1, T2, T3, T4, T5>(string eventName, Action<T1, T2, T3, T4, T5> action)
        {
            AltAsync.OnClient(eventName, action);
            WhitelistEvent(eventName);
        }

        public void OnClient<T1, T2, T3, T4, T5, T6>(string eventName, Action<T1, T2, T3, T4, T5, T6> action)
        {
            AltAsync.OnClient(eventName, action);
            WhitelistEvent(eventName);
        }

        public void OnClient<T1, T2, T3, T4, T5, T6, T7>(string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            AltAsync.OnClient(eventName, action);
            WhitelistEvent(eventName);
        }

        public void OnClient<T1, T2, T3, T4, T5, T6, T7, T8>(string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            AltAsync.OnClient(eventName, action);
            WhitelistEvent(eventName);
        }

        public void OnClient<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            AltAsync.OnClient(eventName, action);
            WhitelistEvent(eventName);
        }

        public void OnClient<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            AltAsync.OnClient(eventName, action);
            WhitelistEvent(eventName);
        }

        public void OnClient<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        {
            AltAsync.OnClient(eventName, action);
            WhitelistEvent(eventName);
        }

        public void OnClient<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        {
            AltAsync.OnClient(eventName, action);
            WhitelistEvent(eventName);
        }
    }
}
