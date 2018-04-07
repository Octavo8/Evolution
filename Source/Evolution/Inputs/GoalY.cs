using System;
using System.Runtime.Serialization;

namespace Evolution
{
    [Serializable, DataContract, KnownType(typeof(IInput))]
    public class GoalY : IInput
    {
        public GoalY()
        {

        }
        public string Name { get => "GoalY"; set => Name = value; }

        public double Value { get; set; }

        public InputTypes InputType => InputTypes.GoalY;
    }

}