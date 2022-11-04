using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
namespace UGame_Local
{
    public interface ILRuntimeRegister
    {
        void Register(AppDomain appdomain);
    }

}