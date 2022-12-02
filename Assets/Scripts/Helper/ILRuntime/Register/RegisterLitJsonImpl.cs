using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
namespace UGame_Local
{
    public class RegisterLitJsonImpl :ILRuntimeRegister
    {
        public void Register(AppDomain appdomain)
        {
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
        }
    }
}