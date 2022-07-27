using ILRuntime.Runtime.Enviorment;
using UnityEngine;

  /// <summary> 说明</summary>
public class DelegateRegister 
{
  private  AppDomain appdomain;

    public DelegateRegister(AppDomain appdomain)
    {
        this.appdomain = appdomain;
    }

    public void Register()
    {
        appdomain.DelegateManager.RegisterMethodDelegate<int>();
        //TODO...

    }


}

