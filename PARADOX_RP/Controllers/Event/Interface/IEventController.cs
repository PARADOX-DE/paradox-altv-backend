using PARADOX_RP.Core.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PARADOX_RP.Controllers.Event.Interface
{
    public interface IEventController
    {
        void WhitelistEvent(string eventName);
        void EventReceived(PXPlayer player, string eventName, params object[] args);

        void OnClient(string eventName, Action action);

        void OnClient<T1>(string eventName, Action<T1> action);

        void OnClient<T1, T2>(string eventName, Action<T1, T2> action);

        void OnClient<T1, T2, T3>(string eventName, Action<T1, T2, T3> action);

        void OnClient<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> action);

        void OnClient<T1, T2, T3, T4, T5>(string eventName, Action<T1, T2, T3, T4, T5> action);

        void OnClient<T1, T2, T3, T4, T5, T6>(string eventName, Action<T1, T2, T3, T4, T5, T6> action);

        void OnClient<T1, T2, T3, T4, T5, T6, T7>(string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7> action);

        void OnClient<T1, T2, T3, T4, T5, T6, T7, T8>(string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action);

        void OnClient<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action);

        void OnClient<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action);

        void OnClient<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action);

        void OnClient<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string eventName,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action);
    }
}
