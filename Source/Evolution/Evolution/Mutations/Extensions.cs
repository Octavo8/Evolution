using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Evolution
{
    public static class Extensions
    {
        /// <summary>
        /// This extension method will return a mutated brain.
        /// It takes the input brain, selects a random Neuron to mutate
        /// and then selects a random mutation. The mutation might change either 
        /// Inputs, the operator or the output memory space that the neuron 
        /// manipulates. 
        /// </summary>
        public static Brain MutateNeurons(this Brain brain, Random rnd)
        {
            var tempBrain = brain.Clone();
            int whichCode = rnd.Next(tempBrain.MyNeurons.Count());
            var neuron = tempBrain.MyNeurons[whichCode];

            int whichMutation = rnd.Next(4);
            int maxValue = 0;
            switch (whichMutation)
            {
                case 0: //Mutate InputA
                    maxValue = tempBrain.MyInputs.Count();
                    neuron.InputA = rnd.Next(maxValue);
                    break;
                case 1://Mutate InputB
                    maxValue = tempBrain.MyInputs.Count();
                    neuron.InputB = rnd.Next(maxValue);
                    break;
                case 2://Mutate the Operator
                    maxValue = tempBrain.MyOperations.Count();
                    neuron.Operation = rnd.Next(maxValue);
                    break;
                case 3: //Mutate the output Memory space
                    maxValue = tempBrain.MyMemorySpaces.Count();
                    neuron.Output = rnd.Next(maxValue);
                    break;
                default:
                    throw new Exception("Impossible");
            }
            Brain child = new Brain();
            child.MyInputs = tempBrain.MyInputs;
            child.MyOperations = tempBrain.MyOperations;
            child.MyNeurons = tempBrain.MyNeurons;
            child.MyNeurons[whichCode] = neuron;
            child.MyComparators = tempBrain.MyComparators;
            child.MyDecisions = tempBrain.MyDecisions;
            child.Score = 0;

            return child;
        }

        /// <summary>
        /// This extension method will return a brain with a mutated Decision
        /// A random decision is selected for mutation and then a random mutation
        /// is applied to that decision. The mutation can be on either of the input
        /// memory spaces or on the comparator .
        /// </summary>
        public static Brain MutateDecisions(this Brain brain, Random rnd)
        {
            var tempBrain = brain.Clone();
            int whichDecision = rnd.Next(tempBrain.MyDecisions.Count());
            var decision = tempBrain.MyDecisions[whichDecision];

            int whichMutation = rnd.Next(3);
            int maxValue = 0;
            switch (whichMutation)
            {
                case 0: //Mutate Memory Space A
                    maxValue = tempBrain.MyMemorySpaces.Count();
                    decision.MemorySpaceA = rnd.Next(maxValue);
                    break;
                case 1://Mutate Memory Space B
                    maxValue = tempBrain.MyMemorySpaces.Count();
                    decision.MemorySpaceB = rnd.Next(maxValue);
                    break;
                case 2://Mutate the Comparator
                    maxValue = tempBrain.MyComparators.Count();
                    decision.Comparator = rnd.Next(maxValue);
                    break;
                default:
                    throw new Exception("Impossible");
            }
            Brain child = new Brain();
            child.MyInputs = tempBrain.MyInputs;
            child.MyOperations = tempBrain.MyOperations;
            child.MyNeurons = tempBrain.MyNeurons;
            child.MyComparators = tempBrain.MyComparators;
            child.MyDecisions = tempBrain.MyDecisions;
            child.MyDecisions[whichDecision] = decision;
            child.Score = 0;

            return child;
        }

        /// <summary>
        /// This hack serialises a given brain to binary and then deserialises it, cloning the brain.
        /// </summary>
        public static Brain Clone<Brain>(this Brain source)
        {
            if (!typeof(Brain).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(Brain);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (Brain)formatter.Deserialize(stream);
            }
        }
    }
}
