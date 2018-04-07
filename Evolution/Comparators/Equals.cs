using System;
using System.Runtime.Serialization;

namespace Evolution
{
    /// <summary>
    /// This class implements an equals comparison.
    /// </summary>
    [Serializable, DataContract, KnownType(typeof(IComparator))]
    public class Equals : IComparator
    {
        public string Name => "=";

        public bool Invoke(double value1, double value2)
        {
            if (value1 == value2)
                return true;
            return false;
        }
    }

}