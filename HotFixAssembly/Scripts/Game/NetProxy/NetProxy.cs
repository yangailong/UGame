using System.Reflection;

namespace UGame_Remove
{

    public partial class NetProxy : Singleton<NetProxy>
    {
        /// <summary>
        /// 注册所有网络通信消息
        /// </summary>
        public void Register()
        {
            MethodInfo[] methohs = typeof(NetProxy).GetMethods();
            foreach (var meth in methohs)
            {
                var attribute = meth.GetCustomAttributes(typeof(NetProxyAttribute), false);
                foreach (var attr in attribute)
                {
                    if (attr is NetProxyAttribute
                    && (attr as NetProxyAttribute).Auton == NetProxyAttribute.AutonType.AutoRegister)
                    {
                        meth.Invoke(NetProxy.Instance, null);
                    }
                }
            }
        }


        /// <summary>
        /// 取消注册所有网络通信消息
        /// </summary>
        public void Unregister()
        {
            MethodInfo[] methohs = typeof(NetProxy).GetMethods();
            foreach (var meth in methohs)
            {
                var attribute = meth.GetCustomAttributes(typeof(NetProxyAttribute), false);
                foreach (var attr in attribute)
                {
                    if (attr is NetProxyAttribute
                    && (attr as NetProxyAttribute).Auton == NetProxyAttribute.AutonType.AutoUnregister)
                    {
                        meth.Invoke(NetProxy.Instance, null);
                    }
                }
            }
        }


    }
}
