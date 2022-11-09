using System;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGame_Local
{
    public class RegisterDelegateConvertorImpl :ILRuntimeRegister
    {
        public void Register(AppDomain appdomain)
        {
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.Type>>((act) =>
            {
                return new System.Predicate<System.Type>((obj) =>
                {
                    return ((Func<System.Type, System.Boolean>)act)(obj);
                });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<System.String>>((act) =>
            {
                return new System.Predicate<System.String>((obj) =>
                {
                    return ((Func<System.String, System.Boolean>)act)(obj);
                });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
            {
                return new UnityEngine.Events.UnityAction(() =>
                {
                    ((Action)act)();
                });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<System.EventHandler>((act) =>
            {
                return new System.EventHandler((sender, e) =>
                {
                    ((Action<System.Object, System.EventArgs>)act)(sender, e);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>>((act) =>
            {
                return new System.EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>((sender, e) =>
                {
                    ((Action<System.Object, SuperSocket.ClientEngine.ErrorEventArgs>)act)(sender, e);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>>((act) =>
            {
                return new System.EventHandler<WebSocket4Net.MessageReceivedEventArgs>((sender, e) =>
                {
                    ((Action<System.Object, WebSocket4Net.MessageReceivedEventArgs>)act)(sender, e);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<System.EventHandler<WebSocket4Net.DataReceivedEventArgs>>((act) =>
            {
                return new System.EventHandler<WebSocket4Net.DataReceivedEventArgs>((sender, e) =>
                {
                    ((Action<System.Object, WebSocket4Net.DataReceivedEventArgs>)act)(sender, e);
                });
            });


        }

    }

}