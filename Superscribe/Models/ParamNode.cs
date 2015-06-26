namespace Superscribe.Models
{
    using System;
    
    public abstract class ParamNode : GraphNode
    {
        private object value;

        protected ParamNode()
        {
            this.ActionFunctions.Add(string.Format("Set_param_{0}", this.Name), (data, segment) => data.Parameters.Add(this.Name, this.value));

            this.activationFunction = (data, segment) =>
            {
                if (!this.TryParse(segment, out this.value))
                {
                    return false;
                }

                return true;
            };
        }

        public abstract bool TryParse(string value, out object obj);

        public abstract Type Type { get; }

        public string Name { get; set; }
    }

    public abstract class ParamNode<T> : ParamNode
    {
        protected ParamNode(string name)
        {
            Name = name;
        }

        public override Type Type
        {
            get
            {
                return typeof(T);
            }
        }
    }
}
