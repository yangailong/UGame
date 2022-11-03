using System;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;


public static class ILTypeExpander
{

    public class ParamMethod
    {
        public string Name;
        public int ParamCount;
        public IMethod Method;
    }


    public static IMethod GetMethod(this ILType type, ParamMethod m)
    {
        if (m.Method != null) return m.Method;

        m.Method = type.GetMethod(m.Name, m.ParamCount);

        if (m.Method == null)
        {
            string baseClass = "";
            if (type.FirstCLRBaseType != null)
            {
                baseClass = type.FirstCLRBaseType.FullName;
            }
            else if (type.FirstCLRInterface != null)
            {
                baseClass = type.FirstCLRInterface.FullName;
            }

            throw new Exception($"can't find the method:{type.FullName}.{m.Name}:{baseClass}, paramCount={m.ParamCount}");
        }

        return m.Method;
    }
}

