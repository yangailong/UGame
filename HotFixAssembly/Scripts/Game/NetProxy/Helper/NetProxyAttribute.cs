using System;

namespace UGame_Remove
{
    public sealed class NetProxyAttribute : Attribute
    {
        public AutonType Auton = AutonType.None;


        public NetProxyAttribute(AutonType autonType)
        {
            this.Auton = autonType;
        }


        public enum AutonType
        {
            /// <summary>
            ///不进行自动注册，需手动注册
            /// </summary>
            None,

            /// <summary>
            /// 自动注册,无需手动调用
            /// </summary>
            AutoRegister,

            /// <summary>
            /// 自动取消注册,
            /// </summary>
            AutoUnregister
        }
    }
}
