using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Evolution
{
    /// <summary>
    /// This is the brain of a creature. It contains the X & Y coordinates of the creature
    /// as well as the nearest goal (food). Score is earned by reaching the goal.
    /// 
    /// The brain contains several neurons which manipulate the values in 
    /// memory spaces by performing operations on various inputs.
    /// 
    /// Decisons on how to move are taken by comparing memory spaces to each other in various ways
    /// </summary>
    [Serializable, DataContract]
    public class Brain
    {

        public Brain()
        {
            Ident = Guid.NewGuid();
        }

        public Guid Ident { get; set; }

        public int GoalY { get; set; }
        public int GoalX { get; set; }
        public int MyX { get;  set; }
        public int MyY { get;  set; }
        public int Score { get; internal set; }
        public double FinalScore
        {
            //fitness  check
            get
            {
                int myScoreX = GoalX - MyX;
                int myScoreY = GoalY - MyY;

                if (myScoreX < 0) myScoreX = myScoreX * -1;
                if (myScoreY < 0) myScoreY = myScoreY * -1;

                return Score + ConvertFinalScore(myScoreX) + ConvertFinalScore(myScoreY);
            }
        }        
        private double ConvertFinalScore(int score)
        {
            double moveScore = 0;
            switch (score)
            {
                case 1: //fractions of a score should not be able to make a full point
                    moveScore = 0.49;
                    break;
                case 2:
                    moveScore = 0.4;
                    break;
                case 3:
                    moveScore = 0.3;
                    break;
                case 4:
                    moveScore = 0.2;
                    break;
                case 5:
                    moveScore = 0.1;
                    break;
                default:
                    break;
            }
            return moveScore;
        }

        /// <summary>
        /// Purely for display, this outputs the brain's neurons in a human readable format
        /// </summary>
        /// <returns></returns>
        public List<string> DisplayNeurons()
        {
            var codes = new List<string>();
            foreach (var item in MyNeurons)
            {
                
                codes.Add(MyMemorySpaces[item.Output].Name + " = " +
                    MyInputs[item.InputA].Name + " " + 
                    MyOperations[item.Operation].Name + " " +
                    MyInputs[item.InputB].Name);
            }
            return codes;
        }

        /// <summary>
        /// Purely for display, this outputs the brains DNA in a human readable format
        /// </summary>
        /// <returns></returns>
        public List<string> DisplayDecisions()
        {
            var decisions = new List<string>();
            int count = 0;
            foreach (var item in MyDecisions)
            {
                bool val = MyComparators[item.Comparator].Invoke(MyMemorySpaces[item.MemorySpaceA].Value, MyMemorySpaces[item.MemorySpaceB].Value);
                string output = MyMemorySpaces[item.MemorySpaceA].Name.ToString() + " "
                    + MyComparators[item.Comparator].Name + " "
                    + MyMemorySpaces[item.MemorySpaceB].Name.ToString() + " is " +
                    val.ToString();

                if (count == 0)
                {
                    if (val) output = output + " X";
                    else output = output + " Y";
                }
                else
                {
                    if (val) output = output + " +";
                    else output = output + " -";
                }
                decisions.Add(output);
                count++;
            }
            return decisions;
        }

        /// <summary>
        /// The brain's list of operations
        /// </summary>
        [DataMember]
        public List<IOperator> MyOperations { get; set; }

        /// <summary>
        /// The brain's list of inputs
        /// </summary>
        [DataMember]
        public List<IInput> MyInputs { get; set; }

        /// <summary>
        /// The brain's list of available memory spaces. Memory spaces are a subset of inputs
        /// </summary>
        public List<IInput> MyMemorySpaces
        {
            get
            {
                return MyInputs.Where(item => item.InputType == InputTypes.MemorySpace).ToList();
            }
        }

        /// <summary>
        /// Neurons take two inputs, perform a mathematical operation on them and store the output in a memory space
        /// </summary>
        [DataMember]
        public List<Neuron> MyNeurons { get; set; }

        /// <summary>
        /// A list of comparisons the brain can perform
        /// </summary>
        [DataMember]
        public List<IComparator> MyComparators { get; set; }

        /// <summary>
        /// A list of decisions a brain can take - see Strategies.cs
        /// </summary>
        [DataMember]
        public List<Decision> MyDecisions { get; set; }


        /// <summary>
        /// Manipulates values inside memory spaces - take two inputs, perform a mathematical operation on them and store the output in a memory space
        /// </summary>
        public void FireMyNeurons()
        {
            foreach (var item in MyNeurons)
            {
                var memSpace = MyMemorySpaces.ElementAt(item.Output);
                memSpace.Value = MyOperations[item.Operation].Invoke(MyInputs[item.InputA].Value, MyInputs[item.InputB].Value);
            }
        }

        /// <summary>
        /// This decides the creatures actions. The first decision is whether to move in the X or Y directions.
        /// The second decision is only used for moving in the X direction and determines whether to move left or right
        /// The third decision is only used for moving in the Y direction and determines whether to move up or down
        /// </summary>
        public void DecideActions()
        {
            var decision = MyDecisions[0];
            bool act = MyComparators[decision.Comparator].Invoke(MyMemorySpaces[decision.MemorySpaceA].Value, MyMemorySpaces[decision.MemorySpaceB].Value);

            if (act) //Move in the X direction
            {
                decision = MyDecisions[1];
                bool direction = MyComparators[decision.Comparator].Invoke(MyMemorySpaces[decision.MemorySpaceA].Value, MyMemorySpaces[decision.MemorySpaceB].Value);
                MoveX(direction);
            }
            else //Move in the Y direction
            {
                decision = MyDecisions[2];
                bool direction = MyComparators[decision.Comparator].Invoke(MyMemorySpaces[decision.MemorySpaceA].Value, MyMemorySpaces[decision.MemorySpaceB].Value);
                MoveY(direction);
            }
        }

        /// <summary>
        /// Move the creature
        /// </summary>
        private void MoveX(bool direction)
        {
            if (direction)
            {
                MyX++;
            }
            else
            {
                MyX--;
            }
        }

        /// <summary>
        /// Move the creature
        /// </summary>
        private void MoveY(bool direction)
        {
            if (direction)
            {
                MyY++;
            }
            else
            {
                MyY--;
            }
        }

    }

}

