using System;

namespace Evolution
{
    /// <summary>
    /// Types of input the creature can use
    /// </summary>
    [Serializable]
    public enum InputTypes
    {
        GoalX = 0,
        GoalY = 1,
        MyX = 2,
        MyY = 3,
        Constant = 4,
        MemorySpace = 5,
    }

}