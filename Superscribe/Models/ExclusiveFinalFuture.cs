﻿namespace Superscribe.Models
{
    using System;

    public class ExclusiveFinalFuture
    {
        public GraphNode Node { get; set; }

        public static ExclusiveFinalFuture operator *(GraphNode node, ExclusiveFinalFuture final)
        {
            final.Node = node;
            return final;
        }
        
        public static GraphNode operator *(ExclusiveFinalFuture future, Func<object, object> final)
        {
            future.Node.FinalFunctions.Add(new ExclusiveFinalFunction { Function = o => o.Response = final(o) });
            return future.Node;
        }
    }

    public static class Final
    {
        public static ExclusiveFinalFuture Exclusive { get { return new ExclusiveFinalFuture(); } }
    }
}
