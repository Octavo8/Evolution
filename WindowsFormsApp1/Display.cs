using Evolution;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Display : Form
    {
        Graphics graphics;
        Controller controller;
        public Brain median;
        public Brain winner;
        public Display()
        {
            InitializeComponent();
        }

        private void Display_Load(object sender, EventArgs e)
        {
            graphics = this.CreateGraphics();
            controller = new Controller();
            graphics.FillRectangle(Brushes.White, 0, 0, 900, 700);

            Draw(median, 0, 0);
            DrawGrid(median, 100, 220);

            Draw(winner, 500, 0);
            DrawGrid(winner, 600, 220);
            controller.StepBrain(median, 1);
            controller.StepBrain(winner, 1);
        }

        private void Display_FormClosing(object sender, FormClosingEventArgs e)
        {
            graphics.Dispose();
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            graphics.FillRectangle(Brushes.White, 0, 0, 900, 700);

            Draw(median, 0, 0);
            DrawGrid(median, 100, 220);

            Draw(winner, 500, 0);
            DrawGrid(winner, 600, 220);
            controller.StepBrain(median, 1);
            controller.StepBrain(winner, 1);

        }
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
                //graphics.DrawRectangle(pen, xOffset + 90, yOffset + (counter * 15), 40, 15);
                //graphics.DrawString(memSpace.Value.ToString("00.00"), this.Font, black, xOffset + 90 + 2, yOffset + 2 + (counter * 15));
                counter++;
            }

            graphics.DrawString(brain.FinalScore.ToString(), this.Font, black, xOffset, yOffset + 80);

            counter = 0;
            foreach (var decision in brain.DisplayDecisions())
            {
                graphics.DrawString(decision, this.Font, black, xOffset, yOffset + 100 + (counter * 15));
                //graphics.DrawRectangle(pen, xOffset + 90, yOffset + (counter * 15), 40, 15);
                //graphics.DrawString(memSpace.Value.ToString("00.00"), this.Font, black, xOffset + 90 + 2, yOffset + 2 + (counter * 15));
                counter++;
            }
        }

        public void DrawGrid(Brain brain, int xOffset, int yOffset)
        {
            Brush black = Brushes.Black;
            Pen pen = new Pen(black);
            int rectifier = 8;
            int size = 15;
            int x = brain.GoalX - brain.MyX;
            int y = brain.GoalY - brain.MyY;
            for (int i = -8; i < 9; i++)
            {
                int offsetR = rectifier + i;
                pen.Color = Color.Green;
                graphics.DrawLine(pen, xOffset + (offsetR * size), yOffset, xOffset + (offsetR * size), yOffset + (16 * size));
                pen.Color = Color.Green;
                graphics.DrawLine(pen, xOffset, yOffset + (offsetR * size), xOffset + (16 * size), yOffset + (offsetR * size));
                if (i == 0)
                {
                    graphics.FillEllipse(new SolidBrush(Color.DarkOrange), xOffset + (offsetR * size) + 2, yOffset + (offsetR * size) + 2, size - 4, size - 4);
                }
                if (i == x)
                {
                    int offsetYR = rectifier + y;
                    graphics.FillEllipse(new SolidBrush(Color.CornflowerBlue), xOffset + (offsetR * size) + 2, yOffset + (offsetYR * size) + 2, size - 4, size - 4);
                }
                graphics.DrawString((brain.GoalX - i).ToString(), this.Font, black, xOffset + (offsetR * size) + 2, yOffset + (16 * size));
                graphics.DrawString((brain.GoalY - i).ToString(), this.Font, black, xOffset + (16 * size), yOffset + (offsetR * size) + 2);
            }

        }

    }
}
