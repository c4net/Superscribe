﻿namespace Superscribe.Models
{
    using System;

    public class NodeFuture
    {
        public SuperscribeNode Parent { get; set; }

        public Func<RouteData, string, bool> ActivationFunction { get; set; }

        public static SuperscribeNode operator *(NodeFuture future, string constant)
        {
            var node = ʃ.Constant(constant);
            node.ActivationFunction = future.ActivationFunction;

            return future.Parent.Slash(node);
        }
    }
}