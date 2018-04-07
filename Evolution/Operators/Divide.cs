using System;
using System.Runtime.Serialization;

namespace Evolution
{
    [Serializable, DataContract, KnownType(typeof(IOperator))]
    public class Divide : IOperator
    {
        public string Name => "Divide";

        public double Invoke(double value1, double value2)
        {
            if (value2 == 0) return 0;
            return value1 / value2;
        }
    }

}