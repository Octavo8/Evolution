using System;
using System.Runtime.Serialization;

namespace Evolution
{
    /// <summary>
    /// A decision holds references to two memory spaces and a comparator.
    /// </summary>
    [DataContract, Serializable]
    public class Decision
    {
        public int MemorySpaceA { get; set; }
        public int MemorySpaceB { get; set; }
        public int Comparator { get; set; }

    }
}
