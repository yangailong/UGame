using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
namespace UGame_Local
{
    public class RegisterMethodDelegateImpl : Singleton<RegisterMethodDelegateImpl>, ILRuntimeRegister
    {
        public void Register(AppDomain appdomain)
        {
              appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.ScriptableObject>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Component>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Object, System.EventArgs>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Object, SuperSocket.ClientEngine.ErrorEventArgs>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Object, WebSocket4Net.MessageReceivedEventArgs>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Object, WebSocket4Net.DataReceivedEventArgs>();


        }
    }


}