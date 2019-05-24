namespace TrackerUI
{
    partial class TournamentResultForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TournamentResultForm));
            this.congratsLabel = new System.Windows.Forms.Label();
            this.whoWonText = new System.Windows.Forms.Label();
            this.prizeOneText = new System.Windows.Forms.Label();
            this.prizeTwoText = new System.Windows.Forms.Label();
            this.thanksForPlayingLabel = new System.Windows.Forms.Label();
            this.runnerUpText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // congratsLabel
            // 
            this.congratsLabel.AutoSize = true;
            this.congratsLabel.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.congratsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.congratsLabel.Location = new System.Drawing.Point(45, 271);
            this.congratsLabel.Name = "congratsLabel";
            this.congratsLabel.Size = new System.Drawing.Size(801, 45);
            this.congratsLabel.TabIndex = 5;
            this.congratsLabel.Text = "Congratulations to our winner on a great tournament!";
            this.congratsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // whoWonText
            // 
            this.whoWonText.AutoSize = true;
            this.whoWonText.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.whoWonText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.whoWonText.Location = new System.Drawing.Point(45, 77);
            this.whoWonText.Name = "whoWonText";
            this.whoWonText.Size = new System.Drawing.Size(270, 45);
            this.whoWonText.TabIndex = 6;
            this.whoWonText.Text = "<someone won>";
            // 
            // prizeOneText
            // 
            this.prizeOneText.AutoSize = true;
            this.prizeOneText.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prizeOneText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.prizeOneText.Location = new System.Drawing.Point(45, 368);
            this.prizeOneText.Name = "prizeOneText";
            this.prizeOneText.Size = new System.Drawing.Size(163, 45);
            this.prizeOneText.TabIndex = 7;
            this.prizeOneText.Text = "<prize 1>";
            this.prizeOneText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.prizeOneText.Visible = false;
            // 
            // prizeTwoText
            // 
            this.prizeTwoText.AutoSize = true;
            this.prizeTwoText.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prizeTwoText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.prizeTwoText.Location = new System.Drawing.Point(45, 465);
            this.prizeTwoText.Name = "prizeTwoText";
            this.prizeTwoText.Size = new System.Drawing.Size(163, 45);
            this.prizeTwoText.TabIndex = 8;
            this.prizeTwoText.Text = "<prize 2>";
            this.prizeTwoText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.prizeTwoText.Visible = false;
            // 
            // thanksForPlayingLabel
            // 
            this.thanksForPlayingLabel.AutoSize = true;
            this.thanksForPlayingLabel.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thanksForPlayingLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.thanksForPlayingLabel.Location = new System.Drawing.Point(327, 600);
            this.thanksForPlayingLabel.Name = "thanksForPlayingLabel";
            this.thanksForPlayingLabel.Size = new System.Drawing.Size(295, 45);
            this.thanksForPlayingLabel.TabIndex = 9;
            this.thanksForPlayingLabel.Text = "Thanks for playing!";
            this.thanksForPlayingLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // runnerUpText
            // 
            this.runnerUpText.AutoSize = true;
            this.runnerUpText.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runnerUpText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.runnerUpText.Location = new System.Drawing.Point(45, 174);
            this.runnerUpText.Name = "runnerUpText";
            this.runnerUpText.Size = new System.Drawing.Size(270, 45);
            this.runnerUpText.TabIndex = 10;
            this.runnerUpText.Text = "<someone won>";
            // 
            // TournamentResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1062, 753);
            this.Controls.Add(this.runnerUpText);
            this.Controls.Add(this.thanksForPlayingLabel);
            this.Controls.Add(this.prizeTwoText);
            this.Controls.Add(this.prizeOneText);
            this.Controls.Add(this.whoWonText);
            this.Controls.Add(this.congratsLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TournamentResultForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tournament Results";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label congratsLabel;
        private System.Windows.Forms.Label whoWonText;
        private System.Windows.Forms.Label prizeOneText;
        private System.Windows.Forms.Label prizeTwoText;
        private System.Windows.Forms.Label thanksForPlayingLabel;
        private System.Windows.Forms.Label runnerUpText;
    }
}