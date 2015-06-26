namespace Superscribe.Models
{
    public class Int : ParamNode<int>
    {
        public Int(string name)
            : base(name)
        {
        }

        public static explicit operator Int(string name)
        {
            return new Int(name);
        }

        public override bool TryParse(string value, out object obj)
        {
            int intObj;
            var results = int.TryParse(value, out intObj);
            obj = intObj;
            return results;
        }
    }

    public class Long : ParamNode<long>
    {
        public Long(string name)
            : base(name)
        {
        }

        public static explicit operator Long(string name)
        {
            return new Long(name);
        }

        public override bool TryParse(string value, out object obj)
        {
            long longObj;
            var results = long.TryParse(value, out longObj);
            obj = longObj;
            return results;
        }
    }

    public class Bool : ParamNode<bool>
    {
        public Bool(string name)
            : base(name)
        {
        }

        public static explicit operator Bool(string name)
        {
            return new Bool(name);
        }

        public override bool TryParse(string value, out object obj)
        {
            bool boolObj;
            var results = bool.TryParse(value, out boolObj);
            obj = boolObj;
            return results;
        }
    }

    public class String : ParamNode<string>
    {
        public String(string name)
            : base(name)
        {
        }

        public static explicit operator String(string name)
        {
            return new String(name);
        }

        public override bool TryParse(string value, out object obj)
        {
            obj = value;
            return true;
        }
    }

    public class Guid : ParamNode<Guid>
    {
        public Guid(string name)
            : base(name)
        {
        }

        public static explicit operator Guid(string name)
        {
            return new Guid(name);
        }

        public override bool TryParse(string value, out object obj)
        {
            System.Guid guidObj;
            var results = System.Guid.TryParse(value, out guidObj);
            obj = guidObj;
            return results;
        }
    }

}
