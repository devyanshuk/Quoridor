namespace Quoridor.DesktopApp.Forms.WelcomeWindowForm
{
    partial class WelcomeForm
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
            components = new System.ComponentModel.Container();
            btnPlay = new System.Windows.Forms.Button();
            lbQuoridor = new System.Windows.Forms.Label();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            lbVS = new System.Windows.Forms.Label();
            btnPlayerOne = new System.Windows.Forms.Button();
            btnPlayerTwo = new System.Windows.Forms.Button();
            toolTipButtonText = new System.Windows.Forms.ToolTip(components);
            btnTwoPlayers = new System.Windows.Forms.Button();
            btnThreePlayers = new System.Windows.Forms.Button();
            btnFourPlayers = new System.Windows.Forms.Button();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btnPlay
            // 
            btnPlay.Dock = System.Windows.Forms.DockStyle.Bottom;
            btnPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F);
            btnPlay.Location = new System.Drawing.Point(0, 443);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new System.Drawing.Size(760, 58);
            btnPlay.TabIndex = 3;
            btnPlay.Text = "Play";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Visible = false;
            btnPlay.Click += btnPlay_Click;
            // 
            // lbQuoridor
            // 
            lbQuoridor.Dock = System.Windows.Forms.DockStyle.Top;
            lbQuoridor.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F);
            lbQuoridor.Location = new System.Drawing.Point(0, 24);
            lbQuoridor.Name = "lbQuoridor";
            lbQuoridor.Size = new System.Drawing.Size(760, 43);
            lbQuoridor.TabIndex = 1;
            lbQuoridor.Text = "Quoridor";
            lbQuoridor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(760, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { loadToolStripMenuItem, helpToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            loadToolStripMenuItem.Text = "Load";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            helpToolStripMenuItem.Text = "Help";
            // 
            // lbVS
            // 
            lbVS.Dock = System.Windows.Forms.DockStyle.Fill;
            lbVS.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            lbVS.Location = new System.Drawing.Point(0, 67);
            lbVS.Name = "lbVS";
            lbVS.Size = new System.Drawing.Size(760, 376);
            lbVS.TabIndex = 3;
            lbVS.Text = "VS";
            lbVS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbVS.Visible = false;
            // 
            // btnPlayerOne
            // 
            btnPlayerOne.Dock = System.Windows.Forms.DockStyle.Left;
            btnPlayerOne.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            btnPlayerOne.Location = new System.Drawing.Point(0, 67);
            btnPlayerOne.Name = "btnPlayerOne";
            btnPlayerOne.Size = new System.Drawing.Size(239, 376);
            btnPlayerOne.TabIndex = 1;
            btnPlayerOne.Text = "button1";
            btnPlayerOne.UseVisualStyleBackColor = true;
            btnPlayerOne.Visible = false;
            btnPlayerOne.Click += btnPlayerOne_Click;
            // 
            // btnPlayerTwo
            // 
            btnPlayerTwo.Dock = System.Windows.Forms.DockStyle.Right;
            btnPlayerTwo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            btnPlayerTwo.Location = new System.Drawing.Point(521, 67);
            btnPlayerTwo.Name = "btnPlayerTwo";
            btnPlayerTwo.Size = new System.Drawing.Size(239, 376);
            btnPlayerTwo.TabIndex = 2;
            btnPlayerTwo.Text = "button2";
            btnPlayerTwo.UseVisualStyleBackColor = true;
            btnPlayerTwo.Visible = false;
            btnPlayerTwo.Click += btnPlayerTwo_Click;
            // 
            // toolTipButtonText
            // 
            toolTipButtonText.ShowAlways = true;
            // 
            // btnTwoPlayers
            // 
            btnTwoPlayers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnTwoPlayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            btnTwoPlayers.Location = new System.Drawing.Point(136, 90);
            btnTwoPlayers.Name = "btnTwoPlayers";
            btnTwoPlayers.Size = new System.Drawing.Size(103, 47);
            btnTwoPlayers.TabIndex = 4;
            btnTwoPlayers.Text = "2 players";
            btnTwoPlayers.UseVisualStyleBackColor = true;
            btnTwoPlayers.Visible = false;
            btnTwoPlayers.Click += btnTwoPlayers_Click;
            // 
            // btnThreePlayers
            // 
            btnThreePlayers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnThreePlayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            btnThreePlayers.Location = new System.Drawing.Point(269, 90);
            btnThreePlayers.Name = "btnThreePlayers";
            btnThreePlayers.Size = new System.Drawing.Size(103, 47);
            btnThreePlayers.TabIndex = 5;
            btnThreePlayers.Text = "3 players";
            btnThreePlayers.UseVisualStyleBackColor = true;
            btnThreePlayers.Visible = false;
            btnThreePlayers.Click += btnThreePlayers_Click;
            // 
            // btnFourPlayers
            // 
            btnFourPlayers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnFourPlayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            btnFourPlayers.Location = new System.Drawing.Point(412, 90);
            btnFourPlayers.Name = "btnFourPlayers";
            btnFourPlayers.Size = new System.Drawing.Size(103, 47);
            btnFourPlayers.TabIndex = 6;
            btnFourPlayers.Text = "4 players";
            btnFourPlayers.UseVisualStyleBackColor = true;
            btnFourPlayers.Visible = false;
            btnFourPlayers.Click += btnFourPlayers_Click;
            // 
            // WelcomeForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(760, 501);
            Controls.Add(btnFourPlayers);
            Controls.Add(btnThreePlayers);
            Controls.Add(btnTwoPlayers);
            Controls.Add(btnPlayerTwo);
            Controls.Add(btnPlayerOne);
            Controls.Add(lbVS);
            Controls.Add(lbQuoridor);
            Controls.Add(btnPlay);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "WelcomeForm";
            Text = "Welcome";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Label lbQuoridor;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Label lbVS;
        private System.Windows.Forms.Button btnPlayerOne;
        private System.Windows.Forms.Button btnPlayerTwo;
        private System.Windows.Forms.ToolTip toolTipButtonText;
        private System.Windows.Forms.Button btnTwoPlayers;
        private System.Windows.Forms.Button btnThreePlayers;
        private System.Windows.Forms.Button btnFourPlayers;
    }
}