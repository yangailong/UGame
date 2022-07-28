using ILRuntime.Runtime.Enviorment;
public class ILRuntimeRegister
{
    private static ILRuntimeRegister instance = null;

    public static void HelperRegisters(AppDomain appDomain)
    {
        if (instance == null)
        {
            instance = new ILRuntimeRegister();
        }
        instance.Register(appDomain);
    }



    protected  virtual void Register(AppDomain appDomain)
    {
       
    }

}

