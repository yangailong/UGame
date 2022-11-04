using System;
using Google.Protobuf;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

public class IMessageAdapt : CrossBindingAdaptor
{

    public override Type BaseCLRType => typeof(IMessage);


    public override Type AdaptorType => typeof(Adaptor);


    public override object CreateCLRInstance(AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }


    public class Adaptor : CrossBindingAdaptorType, IMessage
    {
        private ILTypeInstance instance;

        private AppDomain appdomain;


        public Adaptor() { }


        public Adaptor(AppDomain appdomain, ILTypeInstance instance)
        {
            this.appdomain = appdomain;
            this.instance = instance;
        }


        public ILTypeInstance ILInstance { get { return instance; } set { instance = value; } }


        public AppDomain AppDomain { get { return appdomain; } set { appdomain = value; } }



        private ILTypeExpander.ParamMethod[] methods
        {
            get
            {
                ILTypeExpander.ParamMethod[] methods =
                {
                 new ILTypeExpander.ParamMethod {Name = "MergeFrom", ParamCount = 1},
                 new ILTypeExpander.ParamMethod {Name = "WriteTo", ParamCount = 1},
                 new ILTypeExpander.ParamMethod {Name = "CalculateSize", ParamCount = 0},
                };

                return methods;
            }
        }


        private object Invoke(int index, params object[] p)
        {
            var m = instance.Type.GetMethod(methods[index]);

            return AppDomain.Invoke(m, instance, p);
        }


        public void MergeFrom(CodedInputStream input)
        {
            Invoke(0, input);
        }


        public void WriteTo(CodedOutputStream output)
        {
            Invoke(1, output);
        }


        public int CalculateSize()
        {
            return (int)Invoke(2);
        }


    }



}


