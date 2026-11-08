using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evolution
{
    /// <summary>
    /// Configuration settings for the evolutionary simulation.
    /// All parameters can be adjusted at runtime for experimentation.
    /// </summary>
    public class ConfigSettings
    {
        [Description("Number of creatures in each generation (population size)")]
        [Category("General")]
        [DisplayName("Population Size")]
        [DefaultValue(100)]
        public int PopulationSize { get; set; } = 100;

        [Description("How many simulation steps each creature runs before evaluation")]
        [Category("Simulation")]
        [DisplayName("Steps Per Generation")]
        [DefaultValue(100)]
        public int StepsPerGeneration { get; set; } = 100;

        [Description("Fraction of top performers to select for next generation (0-1)")]
        [Category("Evolution")]
        [DisplayName("Selection Rate")]
        [Range(0, 1)]
        [DefaultValue(0.2)]
        public double SelectionRate { get; set; } = 0.2;

        [Description("Probability of mutating a neuron during crossover (0-1)")]
        [Category("Evolution")]
        [DisplayName("Neuron Mutation Rate")]
        [Range(0, 1)]
        [DefaultValue(0.1)]
        public double NeuronMutationRate { get; set; } = 0.1;

        [Description("Probability of mutating input values (0-1)")]
        [Category("Evolution")]
        [DisplayName("Input Mutation Rate")]
        [Range(0, 1)]
        [DefaultValue(0.1)]
        public double InputMutationRate { get; set; } = 0.1;

        [Description("Probability of mutating operators (0-1)")]
        [Category("Evolution")]
        [DisplayName("Operator Mutation Rate")]
        [Range(0, 1)]
        [DefaultValue(0.1)]
        public double OperatorMutationRate { get; set; } = 0.1;

        [Description("Probability of mutating output memory spaces (0-1)")]
        [Category("Evolution")]
        [DisplayName("Memory Space Mutation Rate")]
        [Range(0, 1)]
        [DefaultValue(0.1)]
        public double MemorySpaceMutationRate { get; set; } = 0.1;

        [Description("Number of neurons per creature brain")]
        [Category("Brain Structure")]
        [DisplayName("Neurons Per Brain")]
        [Range(2, 50)]
        [DefaultValue(6)]
        public int NeuronsPerBrain { get; set; } = 6;

        [Description("Number of comparators per creature brain")]
        [Category("Brain Structure")]
        [DisplayName("Comparators Per Brain")]
        [Range(2, 20)]
        [DefaultValue(4)]
        public int ComparatorsPerBrain { get; set; } = 4;

        [Description("Number of decisions per creature brain")]
        [Category("Brain Structure")]
        [DisplayName("Decisions Per Brain")]
        [Range(1, 10)]
        [DefaultValue(2)]
        public int DecisionsPerBrain { get; set; } = 2;

        [Description("Number of memory spaces to track per creature brain")]
        [Category("Brain Structure")]
        [DisplayName("Memory Spaces Per Brain")]
        [Range(1, 10)]
        [DefaultValue(6)]
        public int MemorySpacesPerBrain { get; set; } = 6;

        [Description("Input value range for simulation (0 to this value)")]
        [Category("Simulation Parameters")]
        [DisplayName("Input Value Range")]
        [Range(1, 255)]
        [DefaultValue(248)]
        public int InputValueRange { get; set; } = 248;

        /// <summary>
        /// Creates a JSON-formatted string representation of this configuration.
        /// </summary>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonNamingPolicy.CamelCase
            });
        }

        /// <summary>
        /// Creates a configuration from a JSON string.
        /// </summary>
        public static ConfigSettings FromJson(string json)
        {
            return JsonSerializer.Deserialize<ConfigSettings>(json);
        }
    }
}
