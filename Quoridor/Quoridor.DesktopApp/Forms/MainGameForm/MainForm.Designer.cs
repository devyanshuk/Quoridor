namespace Quoridor.DesktopApp.Forms.MainGameForm
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnMainMenu = new System.Windows.Forms.Button();
            lbInfoPlayer = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // btnMainMenu
            // 
            btnMainMenu.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            btnMainMenu.Dock = System.Windows.Forms.DockStyle.Top;
            btnMainMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnMainMenu.Location = new System.Drawing.Point(0, 0);
            btnMainMenu.Name = "btnMainMenu";
            btnMainMenu.Size = new System.Drawing.Size(284, 30);
            btnMainMenu.TabIndex = 7;
            btnMainMenu.Text = "Main Menu";
            btnMainMenu.UseVisualStyleBackColor = true;
            btnMainMenu.Click += btnMainMenu_Click;
            // 
            // lbInfoPlayer
            // 
            lbInfoPlayer.Dock = System.Windows.Forms.DockStyle.Top;
            lbInfoPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            lbInfoPlayer.Location = new System.Drawing.Point(0, 30);
            lbInfoPlayer.Name = "lbInfoPlayer";
            lbInfoPlayer.Size = new System.Drawing.Size(284, 30);
            lbInfoPlayer.TabIndex = 8;
            lbInfoPlayer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(284, 261);
            Controls.Add(lbInfoPlayer);
            Controls.Add(btnMainMenu);
            Name = "MainForm";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnMainMenu;
        private System.Windows.Forms.Label lbInfoPlayer;
    }
}
