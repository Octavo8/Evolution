namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnNew = new System.Windows.Forms.Button();
            this.btnStep = new System.Windows.Forms.Button();
            this.btnEvolve = new System.Windows.Forms.Button();
            this.btnEvolveNS = new System.Windows.Forms.Button();
            this.genUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.genUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(923, 12);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(175, 23);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "Create a new Population";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnStep
            // 
            this.btnStep.Location = new System.Drawing.Point(923, 133);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(175, 23);
            this.btnStep.TabIndex = 0;
            this.btnStep.Text = "Simulate Individuals";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // btnEvolve
            // 
            this.btnEvolve.Location = new System.Drawing.Point(923, 41);
            this.btnEvolve.Name = "btnEvolve";
            this.btnEvolve.Size = new System.Drawing.Size(175, 27);
            this.btnEvolve.TabIndex = 1;
            this.btnEvolve.Text = "Evolve by 1 Generation";
            this.btnEvolve.UseVisualStyleBackColor = true;
            this.btnEvolve.Click += new System.EventHandler(this.btnEvolve_Click);
            // 
            // btnEvolveNS
            // 
            this.btnEvolveNS.Location = new System.Drawing.Point(923, 74);
            this.btnEvolveNS.Name = "btnEvolveNS";
            this.btnEvolveNS.Size = new System.Drawing.Size(175, 27);
            this.btnEvolveNS.TabIndex = 2;
            this.btnEvolveNS.Text = "Evolve by 25 Generations";
            this.btnEvolveNS.UseVisualStyleBackColor = true;
            this.btnEvolveNS.Click += new System.EventHandler(this.btnEvolveNS_Click);
            // 
            // genUpDown
            // 
            this.genUpDown.Location = new System.Drawing.Point(1034, 107);
            this.genUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.genUpDown.Name = "genUpDown";
            this.genUpDown.Size = new System.Drawing.Size(64, 20);
            this.genUpDown.TabIndex = 3;
            this.genUpDown.ValueChanged += new System.EventHandler(this.genUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(920, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select a Generation:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1145, 508);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.genUpDown);
            this.Controls.Add(this.btnEvolveNS);
            this.Controls.Add(this.btnEvolve);
            this.Controls.Add(this.btnStep);
            this.Controls.Add(this.btnNew);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.genUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Button btnEvolve;
        private System.Windows.Forms.Button btnEvolveNS;
        private System.Windows.Forms.NumericUpDown genUpDown;
        private System.Windows.Forms.Label label1;
    }
}

