namespace Quoridor.DesktopApp.Forms.StrategySetupWindowForm
{
    partial class StrategySetupForm
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
            lbConfigureStrategy = new System.Windows.Forms.Label();
            cbStrategy = new System.Windows.Forms.ComboBox();
            lbParameters = new System.Windows.Forms.Label();
            cbMinimaxDepth = new System.Windows.Forms.ComboBox();
            lbDepth = new System.Windows.Forms.Label();
            lbC = new System.Windows.Forms.Label();
            lbSimulations = new System.Windows.Forms.Label();
            lbAgent = new System.Windows.Forms.Label();
            cbC = new System.Windows.Forms.ComboBox();
            cbSimulations = new System.Windows.Forms.ComboBox();
            cbAgent = new System.Windows.Forms.ComboBox();
            lbSeed = new System.Windows.Forms.Label();
            numSeed = new System.Windows.Forms.NumericUpDown();
            btnOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)numSeed).BeginInit();
            SuspendLayout();
            // 
            // lbConfigureStrategy
            // 
            lbConfigureStrategy.Dock = System.Windows.Forms.DockStyle.Top;
            lbConfigureStrategy.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F);
            lbConfigureStrategy.Location = new System.Drawing.Point(0, 0);
            lbConfigureStrategy.Name = "lbConfigureStrategy";
            lbConfigureStrategy.Size = new System.Drawing.Size(734, 119);
            lbConfigureStrategy.TabIndex = 0;
            lbConfigureStrategy.Text = "Configure Strategy ";
            lbConfigureStrategy.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cbStrategy
            // 
            cbStrategy.Dock = System.Windows.Forms.DockStyle.Fill;
            cbStrategy.FormattingEnabled = true;
            cbStrategy.Location = new System.Drawing.Point(0, 119);
            cbStrategy.Name = "cbStrategy";
            cbStrategy.Size = new System.Drawing.Size(734, 23);
            cbStrategy.TabIndex = 0;
            cbStrategy.SelectedIndexChanged += cbStrategy_SelectedIndexChanged;
            // 
            // lbParameters
            // 
            lbParameters.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            lbParameters.Location = new System.Drawing.Point(-316, 154);
            lbParameters.Name = "lbParameters";
            lbParameters.Size = new System.Drawing.Size(800, 49);
            lbParameters.TabIndex = 2;
            lbParameters.Text = "Parameters";
            lbParameters.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbParameters.Visible = false;
            // 
            // cbMinimaxDepth
            // 
            cbMinimaxDepth.FormattingEnabled = true;
            cbMinimaxDepth.Items.AddRange(new object[] { "1", "2", "3", "4" });
            cbMinimaxDepth.Location = new System.Drawing.Point(70, 217);
            cbMinimaxDepth.Name = "cbMinimaxDepth";
            cbMinimaxDepth.Size = new System.Drawing.Size(121, 23);
            cbMinimaxDepth.TabIndex = 3;
            cbMinimaxDepth.Visible = false;
            // 
            // lbDepth
            // 
            lbDepth.AutoSize = true;
            lbDepth.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            lbDepth.Location = new System.Drawing.Point(10, 216);
            lbDepth.Name = "lbDepth";
            lbDepth.Size = new System.Drawing.Size(63, 22);
            lbDepth.TabIndex = 4;
            lbDepth.Text = "Depth:";
            lbDepth.Visible = false;
            // 
            // lbC
            // 
            lbC.AutoSize = true;
            lbC.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            lbC.Location = new System.Drawing.Point(12, 240);
            lbC.Name = "lbC";
            lbC.Size = new System.Drawing.Size(28, 22);
            lbC.TabIndex = 5;
            lbC.Text = "C:";
            lbC.Visible = false;
            // 
            // lbSimulations
            // 
            lbSimulations.AutoSize = true;
            lbSimulations.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            lbSimulations.Location = new System.Drawing.Point(164, 243);
            lbSimulations.Name = "lbSimulations";
            lbSimulations.Size = new System.Drawing.Size(107, 22);
            lbSimulations.TabIndex = 6;
            lbSimulations.Text = "Simulations:";
            lbSimulations.Visible = false;
            // 
            // lbAgent
            // 
            lbAgent.AutoSize = true;
            lbAgent.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            lbAgent.Location = new System.Drawing.Point(11, 280);
            lbAgent.Name = "lbAgent";
            lbAgent.Size = new System.Drawing.Size(62, 22);
            lbAgent.TabIndex = 7;
            lbAgent.Text = "Agent:";
            lbAgent.Visible = false;
            // 
            // cbC
            // 
            cbC.FormattingEnabled = true;
            cbC.Items.AddRange(new object[] { "0.81", "1.01", "1.21", "1.41", "1.61", "1.81", "2.01" });
            cbC.Location = new System.Drawing.Point(37, 241);
            cbC.Name = "cbC";
            cbC.Size = new System.Drawing.Size(121, 23);
            cbC.TabIndex = 8;
            cbC.Visible = false;
            // 
            // cbSimulations
            // 
            cbSimulations.FormattingEnabled = true;
            cbSimulations.Items.AddRange(new object[] { "400", "800", "100", "1200" });
            cbSimulations.Location = new System.Drawing.Point(267, 244);
            cbSimulations.Name = "cbSimulations";
            cbSimulations.Size = new System.Drawing.Size(121, 23);
            cbSimulations.TabIndex = 9;
            cbSimulations.Visible = false;
            // 
            // cbAgent
            // 
            cbAgent.FormattingEnabled = true;
            cbAgent.Location = new System.Drawing.Point(70, 279);
            cbAgent.Name = "cbAgent";
            cbAgent.Size = new System.Drawing.Size(121, 23);
            cbAgent.TabIndex = 10;
            cbAgent.Visible = false;
            // 
            // lbSeed
            // 
            lbSeed.AutoSize = true;
            lbSeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            lbSeed.Location = new System.Drawing.Point(12, 216);
            lbSeed.Name = "lbSeed";
            lbSeed.Size = new System.Drawing.Size(57, 22);
            lbSeed.TabIndex = 11;
            lbSeed.Text = "Seed:";
            lbSeed.Visible = false;
            // 
            // numSeed
            // 
            numSeed.Location = new System.Drawing.Point(71, 218);
            numSeed.Name = "numSeed";
            numSeed.Size = new System.Drawing.Size(120, 23);
            numSeed.TabIndex = 12;
            numSeed.Visible = false;
            // 
            // btnOk
            // 
            btnOk.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnOk.Location = new System.Drawing.Point(0, 453);
            btnOk.Name = "btnOk";
            btnOk.Size = new System.Drawing.Size(734, 39);
            btnOk.TabIndex = 13;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // StrategySetupForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(734, 492);
            Controls.Add(btnOk);
            Controls.Add(numSeed);
            Controls.Add(lbSeed);
            Controls.Add(cbAgent);
            Controls.Add(cbSimulations);
            Controls.Add(cbC);
            Controls.Add(lbAgent);
            Controls.Add(lbSimulations);
            Controls.Add(lbC);
            Controls.Add(lbDepth);
            Controls.Add(cbMinimaxDepth);
            Controls.Add(lbParameters);
            Controls.Add(cbStrategy);
            Controls.Add(lbConfigureStrategy);
            Name = "StrategySetupForm";
            Text = "StrategySetupForm";
            ((System.ComponentModel.ISupportInitialize)numSeed).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lbConfigureStrategy;
        private System.Windows.Forms.ComboBox cbStrategy;
        private System.Windows.Forms.Label lbParameters;
        private System.Windows.Forms.ComboBox cbMinimaxDepth;
        private System.Windows.Forms.Label lbDepth;
        private System.Windows.Forms.Label lbC;
        private System.Windows.Forms.Label lbSimulations;
        private System.Windows.Forms.Label lbAgent;
        private System.Windows.Forms.ComboBox cbC;
        private System.Windows.Forms.ComboBox cbSimulations;
        private System.Windows.Forms.ComboBox cbAgent;
        private System.Windows.Forms.Label lbSeed;
        private System.Windows.Forms.NumericUpDown numSeed;
        private System.Windows.Forms.Button btnOk;
    }
}