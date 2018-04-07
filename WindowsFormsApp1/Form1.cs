using Evolution;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string saveFilePath = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }
        Graphics graphics;

        private void Form1_Load(object sender, EventArgs e)
        {
            saveFilePath = Directory.GetCurrentDirectory() + @"\population.bin";
            graphics = this.CreateGraphics();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            graphics.Dispose();


        }
        Controller controller;
        Brain winBrain;
        Brain medianBrain;
        Brain loseBrain;
        Brain winner;
        Brain median;
        Brain loser;

        const int genSteps = 100; //This constant determines how many "Steps" in simulation, each creature gets to try and find food. 

        /// <summary>
        /// When you generate a new seed, a new population of creatures with random brains is created. These are saved to disk and then
        /// allowed to run in simulation for a given number of steps (see genSteps above).
        /// </summary>

        #region Event Handlers
        private void btnNew_Click(object sender, EventArgs e)
        {
            controller = new Controller();

            controller.CreateGenericGeneration();     //Create a population
            controller.SaveGeneration(saveFilePath);
            controller.StepGeneration(genSteps);      //Simulate them to assess their fitness

            genUpDown.Minimum = 1;
            genUpDown.Maximum = 1;
            genUpDown.Value = 1;

            graphics.FillRectangle(new SolidBrush(Color.White), 0, 00, 4000, 700);

            DrawStats();                            //Draw the stats and winners and losers on the form.

            FindAndDrawExampleCreatures();

            controller.LoadGeneration(saveFilePath);

            winBrain = controller.Generation.Single(item => item.Ident == winner.Ident);
            medianBrain = controller.Generation.Single(item => item.Ident == median.Ident);
            loseBrain = controller.Generation.Single(item => item.Ident == loser.Ident);

            DrawBrainSimulation(winBrain, 250, 30);
            DrawBrainSimulation(medianBrain, 250, 230);
            DrawBrainSimulation(loseBrain, 250, 430);
        }

        /// <summary>
        /// This will cause the population to be evolved by taking the top 1/5 scoring creatures and evoling them
        /// </summary>
        private void btnEvolve_Click(object sender, EventArgs e)
        {
            controller.LoadGeneration(saveFilePath);    
            controller.StepGeneration(genSteps);    
            controller.Evolve();
            controller.SaveGeneration(saveFilePath);
            controller.StepGeneration(genSteps);

            graphics.FillRectangle(new SolidBrush(Color.White), 0, 00, 4000, 700);
            DrawStats();

            FindAndDrawExampleCreatures();
            
            controller.LoadGeneration(saveFilePath);
            winBrain = controller.Generation.Single(item => item.Ident == winner.Ident);
            medianBrain = controller.Generation.Single(item => item.Ident == median.Ident);
            loseBrain = controller.Generation.Single(item => item.Ident == loser.Ident);

            DrawBrainSimulation(winBrain, 250, 200);
            DrawBrainSimulation(medianBrain, 250, 400);
            DrawBrainSimulation(loseBrain, 250, 600);
            genUpDown.Maximum = controller.GenerationStats.Count();
            genUpDown.Value = controller.GenerationStats.Count();


        }

        /// <summary>
        /// Evolve non-stop for 25 generations
        /// </summary>
        private void btnEvolveNS_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            controller.LoadGeneration(saveFilePath);
            for (int i = 0; i < 25; i++)
            {
                controller.StepGeneration(genSteps);
                controller.Evolve();
                
            }
            genUpDown.Maximum = controller.GenerationStats.Count();
            genUpDown.Value = controller.GenerationStats.Count();
            controller.SaveGeneration(saveFilePath);
            btnEvolve_Click(sender, e);
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// If you want to simulate the winning and median brains from a generation.
        /// </summary>
        private void btnStep_Click(object sender, EventArgs e)
        {
            Display display = new Display();
            if (controller.GenerationStats.Count() == 0) return;
            display.median = controller.GenerationStats[Decimal.ToInt32(genUpDown.Value)-1].Median;
            display.winner = controller.GenerationStats[Decimal.ToInt32(genUpDown.Value)-1].Winner;
            display.Show();
        }

        /// <summary>
        /// Redraw the results graph if the UpDown ticker is changed
        /// </summary>
        private void genUpDown_ValueChanged(object sender, EventArgs e)
        {
            graphics.FillRectangle(new SolidBrush(Color.White), 0, 00, 4000, 700);
            DrawNewStats(880, 500);
        }

        #endregion Event Handlers

        private void FindAndDrawExampleCreatures()
        {
            winner = controller.Generation.OrderByDescending(item => item.FinalScore).First();
            median = controller.Generation.OrderByDescending(item => item.FinalScore).Skip(Controller.generationMiddle).First();
            loser = controller.Generation.OrderByDescending(item => item.FinalScore).Last();
            Draw(winner, 6, 30);
            Draw(median, 6, 230);
            Draw(loser, 6, 430);
        }

        /// <summary>
        /// Draw a grid, centered on the given creature (marked by a dark blue elipse), and 
        /// draw the goal (food) as a Crimson elipse.
        /// </summary>
        private void DrawBrainSimulation(Brain brain, int xOffset, int yOffset)
        {
            int lineLength = 820;

            SolidBrush solidBrush = new SolidBrush(Color.Black);
            Pen myPen = new Pen(solidBrush);

            graphics.DrawLine(myPen, xOffset + (12 * 20), yOffset, lineLength, yOffset);

            for (int i = -8; i < 9; i++)
            {
                if (i == 0)
                {
                    graphics.FillEllipse(new SolidBrush(Color.DarkBlue), xOffset + ((i + 20) * 20), yOffset - 5, 10, 10);
                }

                int x = brain.GoalX - brain.MyX;

                if (i == (brain.GoalX - brain.MyX))
                {
                    graphics.FillEllipse(new SolidBrush(Color.Crimson), xOffset + ((i + 20) * 20), yOffset - 5, 10, 10);
                }
                int scale = (i + brain.MyX);
                graphics.DrawString(scale.ToString(), this.Font, Brushes.Black, xOffset + ((i + 20) * 20), yOffset + 3);
            }

            myPen.Dispose();
        }

        /// <summary>
        /// This draws a brain on the form at a given offset.
        /// It draws Memory spaces, Neurons, Decisions and score.
        /// </summary>
        private void Draw(Brain brain, int xOffset, int yOffset)
        {
            Brush black = Brushes.Black;
            Pen pen = new Pen(black);
            int counter = 0;

            foreach (var input in brain.MyInputs.Where(inp => inp.InputType != InputTypes.MemorySpace))
            {
                graphics.DrawString(input.Name, this.Font, black, xOffset, yOffset + 2 + (counter * 15));
                graphics.DrawRectangle(pen, xOffset + 35, yOffset + (counter * 15), 40, 15);
                graphics.DrawString(input.Value.ToString("00.00"), this.Font, black, xOffset + 35 + 2, yOffset + 2 + (counter * 15));
                counter++;
            }

            counter = 0;
            foreach (var memSpace in brain.MyMemorySpaces)
            {
                graphics.DrawString("M" + counter.ToString(), this.Font, black, xOffset + 80, yOffset + 2 + (counter * 15));
                graphics.DrawRectangle(pen, xOffset + 100, yOffset + (counter * 15), 40, 15);
                graphics.DrawString(memSpace.Value.ToString("00.00"), this.Font, black, xOffset + 100 + 2, yOffset + 2 + (counter * 15));
                counter++;
            }

            counter = 0;
            foreach (var dna in brain.DisplayNeurons())
            {
                graphics.DrawString(dna, this.Font, black, xOffset + 145, yOffset + 2 + (counter * 15));
                counter++;
            }

            graphics.DrawString(brain.FinalScore.ToString(), this.Font, black, xOffset, yOffset + 80);

            counter = 0;
            foreach (var decision in brain.DisplayDecisions())
            {
                graphics.DrawString(decision, this.Font, black, xOffset, yOffset + 100 + (counter * 15));
                counter++;
            }

        }

        /// <summary>
        /// Draws the graph that shows what the score distribution of the current generation is
        /// </summary>
        private void DrawStats()
        {
            var orderedGroup = controller.Generation.GroupBy(item => item.Score).OrderBy(ord => ord.Key);

            for (int i = 0; i < 26; i++)
            {
                var set = orderedGroup.SingleOrDefault(item => item.Key == i);
                int setSize = 0;
                if (set != null)
                {
                    setSize = set.Count();
                }

                graphics.FillRectangle(Brushes.ForestGreen, 880 + (i * 15), 500 - setSize, 15, setSize);

                graphics.DrawString(i.ToString(), this.Font, Brushes.DarkGreen, 880 + (i * 15), 500);
                Font myFont = new Font(FontFamily.GenericSansSerif, 6);
                if (setSize > 0)
                {
                    graphics.DrawString(setSize.ToString(), myFont, Brushes.DarkGreen, 880 + (i * 15), 512);
                }
            }
        }

        /// <summary>
        /// A copy of the above method, used for drawing the generation selected by the up/down selector
        /// </summary>
        private void DrawNewStats(int xOffset, int yOffset)
        {
            if (controller.GenerationStats.Count == 0) return;

            int val = Decimal.ToInt32(genUpDown.Value)-1;
            var set = controller.GenerationStats[val].GraphData;
            for (int i = 0; i < set.Length; i++)
            {
                graphics.DrawString(i.ToString(), this.Font, Brushes.DarkGreen, xOffset + (i * 15), yOffset);
                Font myFont = new Font(FontFamily.GenericSansSerif, 6);
                if (set[i] > 0)
                {
                    graphics.FillRectangle(Brushes.ForestGreen, xOffset + (i * 15), yOffset - set[i], 15, set[i]);
                    graphics.DrawString(set[i].ToString(), myFont, Brushes.DarkGreen, xOffset + (i * 15), yOffset + 12);
                }
            }
        }
    }
}
