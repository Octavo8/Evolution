using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Evolution
{
    public class Controller
    {
        public const int generationTotal = 500;
        public const int generationMiddle = 250;

        #region Properties
        private Random rnd;
        private int MaxInputs = 0;
        private int MaxOperations = 0;
        private IEnumerable<Type> AllInputs;
        private IEnumerable<Type> AllOperators;
        private IEnumerable<Type> AllComparators;
        private int TotalStrategies = 0;
        private List<GenerationStat> generationStats;

        public List<Brain> Generation { get; set; }
        public List<GenerationStat> GenerationStats { get { return generationStats; } }

        #endregion Properties

        #region Constructors
        public Controller()
        {
            rnd = new Random();
            ControllerInit();
        }

        public Controller(int seed)
        {
            rnd = new Random(seed);
            ControllerInit();
        }

        private void ControllerInit()
        {
            Type[] allTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();

            MaxInputs = allTypes.Where(mytype => mytype.GetInterfaces().Contains(typeof(IInput))).Count();

            MaxOperations = allTypes.Where(mytype => mytype.GetInterfaces().Contains(typeof(IOperator))).Count();

            AllInputs = allTypes.Where(mytype => mytype.GetInterfaces().Contains(typeof(IInput)));

            AllOperators = allTypes.Where(mytype => mytype.GetInterfaces().Contains(typeof(IOperator)));

            AllComparators = allTypes.Where(mytype => mytype.GetInterfaces().Contains(typeof(IComparator)));

            TotalStrategies = Enum.GetNames(typeof(Strategies)).Count();

            Generation = new List<Brain>();
            generationStats = new List<GenerationStat>();
        }

        #endregion Constructors

        #region Brain & Generation Creation
        /// <summary>
        /// Create an entire population of creatures
        /// </summary>
        public void CreateGenericGeneration()
        {
            Generation.Clear();
            for (int i = 0; i < generationTotal; i++)
            {
                Generation.Add(CreateGenericBrain());
            }
            StoreGeneration();
        }

        /// <summary>
        /// Create a new creature with a random set of Neurons and Decisions. Some things are added by default to each creature. 
        /// e.g. each creature starts out with a single copy of each input - X & Y coordinates for itself and its goal and
        /// a constant of value -10. You could make these random, but it would just waste cpu time.
        /// Each creature each gets 3 memory spaces which can hold data,
        /// six Neurons which can manipulate the values in the memory spaces,
        /// and three Decisions, which allow the creature to decide what
        /// moves to make.
        /// </summary>
        private Brain CreateGenericBrain()
        {
            var brain = new Brain();
            brain = ResetBrain(brain);
            brain = GenerateSpawnPosition(brain);
            brain = GenerateNewGoal(brain);

            for (int i = 0; i < AllInputs.Count(); i++)
            {
                var newInputType = AllInputs.ElementAt(i);
                IInput instance = (IInput)Activator.CreateInstance(newInputType);

                if (instance.InputType == InputTypes.Constant)
                {
                    instance.Value = -10;
                }
                if (instance.InputType == InputTypes.MemorySpace)
                {
                    instance.Name = "M0";
                }
                brain.MyInputs.Add(instance);
            }
            brain.MyInputs.Add(new MemorySpace() { Name = "M1"  });
            brain.MyInputs.Add(new MemorySpace() { Name = "M2" });

            for (int i = 0; i < AllOperators.Count(); i++)
            {
                var newInputType = AllOperators.ElementAt(i);
                IOperator instance = (IOperator)Activator.CreateInstance(newInputType);
                brain.MyOperations.Add(instance);
            }

            for (int i = 0; i < AllComparators.Count(); i++)
            {
                var newInputType = AllComparators.ElementAt(i);
                IComparator instance = (IComparator)Activator.CreateInstance(newInputType);
                brain.MyComparators.Add(instance);
            }

            for (int i = 0; i < (TotalStrategies * 2); i++)
            {                
               brain.MyNeurons.Add(new Neuron()
               {
                   InputA = rnd.Next(brain.MyInputs.Count()),
                   InputB = rnd.Next(brain.MyInputs.Count()),
                   Operation = rnd.Next(brain.MyOperations.Count()),
                   Output = rnd.Next(brain.MyMemorySpaces.Count()),
               });
            }

            for (int i = 0; i < TotalStrategies; i++)
            {                
                brain.MyDecisions.Add(new Decision()
                {
                    MemorySpaceA = rnd.Next(brain.MyMemorySpaces.Count()),
                    MemorySpaceB = rnd.Next(brain.MyMemorySpaces.Count()),
                    Comparator = rnd.Next(brain.MyComparators.Count())
                });
            }



            brain = UpdateInputsAfterActions(brain);
            brain.FireMyNeurons();
            return brain;
        }

        #endregion Brain & Generation Creation
        /// <summary>
        /// Allow the entire population to run in simulation for a given number of moves or "Steps" to see what they do
        /// </summary>
        public void StepGeneration(int steps)
        {
            for (int j = 0; j < Generation.Count(); j++)
            {
                for (int i = 0; i < steps; i++)
                {
                    Generation[j].DecideActions();
                    Generation[j] = DoCollisionCheck(Generation[j]);
                    Generation[j] = UpdateInputsAfterActions(Generation[j]);
                    Generation[j].FireMyNeurons();
                }
            }
        }

        /// <summary>
        /// Simulate the creature for a given number of steps and 
        /// allow it to decide what to do and where to move.
        /// After checking for collisions with food, update the inputs
        /// and fire neurons in preparation for the next move ("Step")
        /// </summary>
        public Brain StepBrain(Brain brain, int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                brain.DecideActions();
                brain = DoCollisionCheck(brain);
                brain = UpdateInputsAfterActions(brain);
                brain.FireMyNeurons();
            }
            return brain;
        }

        #region Evolution

        /// <summary>
        /// This method updates a class that holds an example of the median and winner brain in each population at
        /// each step.
        /// </summary>
        private void StoreGeneration()
        {
            int[] stats = new int[50];
            var grouping = Generation.GroupBy(item => item.Score).OrderBy(ord => ord.Key).ToArray();
            int maxCount = 0;
            Brain median = null;
            Brain winner = null;
            for (int i = 0; i < grouping.Length; i++)
            {
                stats[grouping[i].Key] = grouping[i].Count();

                if (grouping[i].Count() > maxCount && grouping[i].Key > 0)
                {
                    median = grouping[i].First();
                    maxCount = grouping[i].Count();
                }
                if (i == grouping.Length - 1)
                {
                    winner = grouping[i].First();
                }

            }
            if (median != null)
            {
                median = GenerateSpawnPosition(median);
                median = GenerateNewGoal(median);
                median = UpdateInputsAfterActions(median);
                median.FireMyNeurons();
            }
            winner = GenerateSpawnPosition(winner);
            winner = GenerateNewGoal(winner);
            winner = UpdateInputsAfterActions(winner);
            winner.FireMyNeurons();

            generationStats.Add(new GenerationStat()
            {
                GraphData = stats,
                Median = median,
                Winner = winner,
            });


        }

        /// <summary>
        /// This will tak the population and generate a new one based on the coded weighting for mutations.
        /// Current setup: Only the highest-scoring 1/5 of the population survive and pass on their genes.
        /// The first 1/5 are exact copies of the parents (you don't want to go backwards).
        /// The second 1/5 have a single mutation in a Neuron
        /// The third 1/5 have two mutations in their Neurons
        /// The fourth 1/5 have a mutation in a Decision
        /// The final 1/5 one mutated Neuron and one mutated Decision.
        /// </summary>
        public void Evolve()
        {
            //Store the new generation.
            StoreGeneration();

            int fifth = generationTotal / 5;
            var parents = Generation.OrderByDescending(item => item.FinalScore).Take(fifth).ToList();
            List<Brain> descendants = new List<Brain>();
            for (int j = 0; j < parents.Count(); j++)
            {
                var original = parents[j].Clone();
                original.Score = 0;
                original = GenerateSpawnPosition(original);
                original = GenerateNewGoal(original);
                original = UpdateInputsAfterActions(original);
                original.FireMyNeurons();
                descendants.Add(original);

                var mutated = parents[j].MutateNeurons(rnd);
                mutated = GenerateSpawnPosition(mutated);
                mutated = GenerateNewGoal(mutated);
                mutated = UpdateInputsAfterActions(mutated);
                mutated.FireMyNeurons();
                descendants.Add(mutated);

                mutated = parents[j].MutateNeurons(rnd).MutateNeurons(rnd);
                mutated = GenerateSpawnPosition(mutated);
                mutated = GenerateNewGoal(mutated);
                mutated = UpdateInputsAfterActions(mutated);
                mutated.FireMyNeurons();
                descendants.Add(mutated);

                mutated = parents[j].MutateDecisions(rnd);
                mutated = GenerateSpawnPosition(mutated);
                mutated = GenerateNewGoal(mutated);
                mutated = UpdateInputsAfterActions(mutated);
                mutated.FireMyNeurons();
                descendants.Add(mutated);

                mutated = parents[j].MutateNeurons(rnd).MutateDecisions(rnd);
                mutated = GenerateSpawnPosition(mutated);
                mutated = GenerateNewGoal(mutated);
                mutated = UpdateInputsAfterActions(mutated);
                mutated.FireMyNeurons();
                descendants.Add(mutated);
            }
            Generation.Clear();
            Generation = descendants;

        }

        #endregion Evolution

        #region Private state management 
        /// <summary>
        /// Check whether a creature has reached a goal (food)
        /// if so, increase the score by 1 and spawn a new goal (food)
        /// </summary>
        private Brain DoCollisionCheck(Brain brain)
        {
            if (brain.MyX == brain.GoalX && brain.MyY == brain.GoalY)
            {
                brain.Score++;
                brain = GenerateNewGoal(brain);
            }
            return brain;
        }

        private Brain ResetBrain(Brain brain)
        {
            brain.MyOperations = new List<IOperator>();
            brain.MyInputs = new List<IInput>();
            brain.MyNeurons = new List<Neuron>();
            brain.MyComparators = new List<IComparator>();
            brain.MyDecisions = new List<Decision>();
            brain.Score = 0;
            brain.Ident = Guid.NewGuid();
            return brain;
        }

        /// <summary>
        /// Controls where on the grid creatures spawn when they are created
        /// </summary>
        private Brain GenerateSpawnPosition(Brain brain)
        {
            brain.MyX = rnd.Next(50, 55);
            brain.MyY = rnd.Next(50, 55);
            return brain;
        }

        /// <summary>
        /// Controls where new food spawns on the grid
        /// </summary>
        private Brain GenerateNewGoal(Brain brain)
        {
            brain.GoalX = brain.MyX + GenerateNewPosition();
            brain.GoalY = brain.MyY + GenerateNewPosition();

            return brain;
        }

        /// <summary>
        /// Generates a new random position. Changing values here will change how 
        /// far away new goals (food) is placed.
        /// </summary>
        private int GenerateNewPosition()
        {

            int newPosition = rnd.Next(2);
            if (newPosition == 0)
            {
                return rnd.Next(4);
            }
            else
            {
                return rnd.Next(4) * -1;
            }
        }

        /// <summary>
        /// At the end of a "step", after neurons have fired, after decisons have lead to actions, 
        /// after the creature has moved on the grid, all the inputs need to be updated with the 
        /// latest input values 
        /// </summary>
        private Brain UpdateInputsAfterActions(Brain brain)
        {
            foreach (var item in brain.MyInputs)
            {
                switch (item.InputType)
                {
                    case InputTypes.GoalX:
                        item.Value = brain.GoalX;
                        break;
                    case InputTypes.GoalY:
                        item.Value = brain.GoalY;
                        break;
                    case InputTypes.MyX:
                        item.Value = brain.MyX;
                        break;
                    case InputTypes.MyY:
                        item.Value = brain.MyY;
                        break;
                    default:
                        break;
                }
            }
            return brain;
        }

        #endregion Private state management 

        #region File IO

        public void SaveGeneration(string filepath)
        {
            Stream streamWriter = File.OpenWrite(filepath);
            BinaryFormatter serialiser = new BinaryFormatter();
            serialiser.Serialize(streamWriter, Generation);
            streamWriter.Close();
        }

        public void LoadGeneration(string filepath)
        {
            Stream stream = File.OpenRead(filepath);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            Generation = (List<Brain>)binaryFormatter.Deserialize(stream);
            stream.Close();
        }

        #endregion File IO
    }
}
