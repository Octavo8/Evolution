using System;
using System.Runtime.Serialization;

namespace Evolution
{
    [Serializable, DataContract, KnownType(typeof(IOperator))]
    public class Add : IOperator
    {
        public string Name => "Add";

        public double Invoke(double value1, double value2)
        {
            return value1 + value2;
        }
    }

}