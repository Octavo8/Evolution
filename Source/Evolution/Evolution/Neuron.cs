using System;
using System.Runtime.Serialization;

namespace Evolution
{
    /// <summary>
    /// The Neuron holds references to two inputs, an operation and an output memory space
    /// </summary>
    [DataContract, Serializable]
    public class Neuron
    {
        public int InputA { get; set; }
        public int InputB { get; set; }
        public int Operation { get; set; }

        public int Output { get; set; }
    }
}