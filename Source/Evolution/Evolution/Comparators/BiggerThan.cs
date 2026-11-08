using System;
using System.Runtime.Serialization;

namespace Evolution
{
    /// <summary>
    /// This class implements a bigger than comparison.
    /// </summary>
    [Serializable, DataContract, KnownType(typeof(IComparator))]
    public class BiggerThan : IComparator
    {
        public string Name => ">";

        public bool Invoke(double value1, double value2)
        {
            if (value1 > value2)
                return true;
            return false;
        }
    }

}