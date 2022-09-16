using System;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGame_Local
{
    public class RegisterDelegateConvertorImpl : Singleton<RegisterDelegateConvertorImpl>, ILRuntimeRegister
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

        }

    }

}