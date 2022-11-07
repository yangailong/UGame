using System;
namespace UGame_Remove
{
    public partial class NetProxy
    {
        [NetProxy(NetProxyAttribute.AutonType.AutoRegister)]
        public void Register_Shop()
        {
          
        }

        [NetProxy(NetProxyAttribute.AutonType.AutoUnregister)]
        public void Unregister_Shop()
        {
           
        }


    }
}
