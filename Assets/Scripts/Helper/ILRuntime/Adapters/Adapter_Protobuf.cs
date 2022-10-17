using System;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.Method;
using System.Collections.Generic;
using System.Collections;

namespace Assets.Scripts
{
    public class Adapter_Protobuf : CrossBindingAdaptor
    {

        public override Type BaseCLRType => null;


        public override Type[] BaseCLRTypes => new Type[] { typeof(IEquatable<ILTypeInstance>), typeof(IComparable<ILTypeInstance>), typeof(IEnumerable<byte>) };


        public override Type AdaptorType => typeof(Adaptor);


        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adaptor(appdomain, instance);
        }



        internal class Adaptor : IEquatable<ILTypeInstance>, IComparable<ILTypeInstance>, IEnumerable<byte>, CrossBindingAdaptorType
        {

            ILTypeInstance instance;

            ILRuntime.Runtime.Enviorment.AppDomain appdomain;


            public ILTypeInstance ILInstance => instance;


            public Adaptor()
            {

            }

            public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }


            public object[] data1 = new object[1];

            IMethod mEquals = null;
            bool mEqualsGot = false;



            public bool Equals(ILTypeInstance other)
            {
                if (!mEqualsGot)
                {
                    mEquals = instance.Type.GetMethod("Equals", 1);

                    if (mEquals == null)
                    {
                        mEquals = instance.Type.GetMethod("System.IEquatable.Equals", 1);
                    }
                    mEqualsGot = true;
                }

                if (mEquals != null)
                {
                    data1[0] = other;

                    return (bool)appdomain.Invoke(mEquals, instance, data1);
                }
                return false;
            }


            IMethod mCompareTo = null;
            bool mCompareToGot = false;

            public int CompareTo(ILTypeInstance other)
            {
                if (!mCompareToGot)
                {
                    mCompareTo = instance.Type.GetMethod("CompareTo", 1);
                    if (mCompareTo == null)
                    {
                        mCompareTo = instance.Type.GetMethod("System.IComparable.CompareTo", 1);
                    }
                    mCompareToGot = true;
                }
                if (mCompareTo != null)
                {
                    data1[0] = other;
                    return (int)appdomain.Invoke(mCompareTo, instance, data1);
                }
                return 0;
            }



            public IEnumerator<byte> GetEnumerator()
            {
                IMethod method = null;
                method = instance.Type.GetMethod("GetEnumerator", 0);
                if (method == null)
                {
                    method = instance.Type.GetMethod("System.Collections.IEnumerable.GetEnumerator", 0);
                }
                if (method != null)
                {
                    var res = appdomain.Invoke(method, instance, null);
                    return (IEnumerator<byte>)res;
                }
                return null;
            }



            IEnumerator IEnumerable.GetEnumerator()
            {
                IMethod method = null;
                method = instance.Type.GetMethod("GetEnumerator", 0);
                if (method == null)
                {
                    method = instance.Type.GetMethod("System.Collections.IEnumerable.GetEnumerator", 0);
                }
                if (method != null)
                {
                    var res = appdomain.Invoke(method, instance, null);
                    return (IEnumerator)res;
                }
                return null;
            }
        }
    }
}
