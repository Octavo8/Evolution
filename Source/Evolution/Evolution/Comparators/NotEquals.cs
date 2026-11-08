using System;
using System.Runtime.Serialization;

namespace Evolution
{
    /// <summary>
    /// This class implements a not equals comparison.
    /// </summary>
    [Serializable, DataContract, KnownType(typeof(IComparator))]
    public class NotEquals : IComparator
    {
        public string Name => "!=";

        public bool Invoke(double value1, double value2)
        {
            if (value1 != value2)
                return true;
            return false;
        }
    }

}