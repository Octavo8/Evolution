using System;
using System.Runtime.Serialization;

namespace Evolution
{
    [Serializable, DataContract, KnownType(typeof(IInput))]
    public class MemorySpace : IInput
    {
        public MemorySpace()
        {
            Value = 0;
            name = "MemSpc";
        }
        public string Name { get { return name; } set { name = value; } }
        public string name;

        public double Value { get; set; }

        public InputTypes InputType => InputTypes.MemorySpace;
    }

}