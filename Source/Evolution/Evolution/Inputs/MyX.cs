using System;
using System.Runtime.Serialization;

namespace Evolution
{
    [Serializable, DataContract, KnownType(typeof(IInput))]
    public class MyX : IInput
    {
        public MyX()
        {

        }
        public string Name { get => "MyX"; set => Name = value; }

        public double Value { get; set; }

        public InputTypes InputType => InputTypes.MyX;
    }

}