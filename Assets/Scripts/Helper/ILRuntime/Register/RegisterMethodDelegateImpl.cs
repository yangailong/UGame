using ILRuntime.Runtime.Enviorment;
namespace UGame_Local
{
    public class RegisterMethodDelegateImpl : Singleton<RegisterMethodDelegateImpl>, ILRuntimeRegister
    {
        public void Register(AppDomain appdomain)
        {
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>>();
        }
    }

}