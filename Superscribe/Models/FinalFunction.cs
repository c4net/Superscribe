﻿
using Superscribe.Engine;

namespace Superscribe.Models
{
    using System;

    public class ExclusiveFinalFunction : FinalFunction
    {
        public ExclusiveFinalFunction()
        {
        }

        public ExclusiveFinalFunction(string method, Func<IModuleRouteData, object> func)
            : base(method, func)
        {
        }

        public override bool IsExclusive
        {
            get
            {
                return true;
            }
        }
    }

    public class FinalFunction
    {
        public class ExecuteAndContinue
        {
        }

        public FinalFunction()
        {
        }

        public FinalFunction(string method, Func<IModuleRouteData, object> func)
        {
            this.Method = method;
            this.Function = func;
        }

        public virtual bool IsExclusive
        {
            get
            {
                return false;
            }
        }

        public string Method { get; set; }

        public Func<IModuleRouteData, object> Function { get; set; }
        
        public static FinalFunctionList operator |(FinalFunction function, FinalFunction other)
        {
            return new FinalFunctionList { function, other };
        }

        public static FinalFunctionList operator |(FinalFunctionList functions, FinalFunction other)
        {
            functions.Add(other);
            return functions;
        }
    }
}
