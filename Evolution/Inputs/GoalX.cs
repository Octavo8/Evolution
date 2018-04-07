using System;
using System.Runtime.Serialization;

namespace Evolution
{
    [Serializable, DataContract, KnownType(typeof(IInput))]
    public class GoalX : IInput
    {
        public GoalX()
        {

        }
        public string Name { get => "GoalX"; set => Name = value; } 

        public double Value { get; set; }

        public InputTypes InputType => InputTypes.GoalX;
    }

}