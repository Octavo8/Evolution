using System;
using System.Runtime.Serialization;

namespace Evolution
{
    [Serializable, DataContract, KnownType(typeof(IInput))]
    public class MyY : IInput
    {
        public MyY()
        {

        }
        public string Name { get => "MyY"; set => Name = value; }

        public double Value { get; set; }

        public InputTypes InputType => InputTypes.MyY;
    }

}