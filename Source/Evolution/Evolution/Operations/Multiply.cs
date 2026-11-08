using System;
using System.Runtime.Serialization;

namespace Evolution
{
    [Serializable, DataContract, KnownType(typeof(IOperator))]
    public class Multiply : IOperator
    {
        public string Name => "Multiply";

        public double Invoke(double value1, double value2)
        {
            return value1 * value2;
        }
    }

}