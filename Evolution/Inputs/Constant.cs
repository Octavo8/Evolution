using System;
using System.Runtime.Serialization;

namespace Evolution
{
    [Serializable, DataContract, KnownType(typeof(IInput))]
    public class Constant : IInput
    {
        public Constant()
        {

        }
        public string Name { get => "Const"; set => Name = value; }

        public double Value { get; set; }

        public InputTypes InputType => InputTypes.Constant;
    }

}