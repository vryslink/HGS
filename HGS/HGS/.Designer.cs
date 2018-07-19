namespace HGS
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
            this.components = new System.ComponentModel.Container();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.spawningTimer = new System.Windows.Forms.Timer(this.components);
            this.Lscoretable = new System.Windows.Forms.Label();
            this.LpresentedByText = new System.Windows.Forms.Label();
            this.LpressEnter = new System.Windows.Forms.Label();
            this.finishTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // gameTimer
            // 
            this.gameTimer.Enabled = true;
            this.gameTimer.Interval = 90;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // spawningTimer
            // 
            this.spawningTimer.Interval = 400;
            this.spawningTimer.Tick += new System.EventHandler(this.spawningTimer_Tick);
            // 
            // Lscoretable
            // 
            this.Lscoretable.AutoSize = true;
            this.Lscoretable.BackColor = System.Drawing.Color.Transparent;
            this.Lscoretable.Location = new System.Drawing.Point(1, 2);
            this.Lscoretable.Name = "Lscoretable";
            this.Lscoretable.Size = new System.Drawing.Size(0, 17);
            this.Lscoretable.TabIndex = 0;
            this.Lscoretable.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LpresentedByText
            // 
            this.LpresentedByText.AutoSize = true;
            this.LpresentedByText.Font = new System.Drawing.Font("Script MT Bold", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LpresentedByText.Location = new System.Drawing.Point(75, 446);
            this.LpresentedByText.Name = "LpresentedByText";
            this.LpresentedByText.Size = new System.Drawing.Size(704, 47);
            this.LpresentedByText.TabIndex = 1;
            this.LpresentedByText.Text = "PRESENTED BY VACLAV RYSLINK";
            this.LpresentedByText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // LpressEnter
            // 
            this.LpressEnter.AutoSize = true;
            this.LpressEnter.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LpressEnter.Location = new System.Drawing.Point(153, 337);
            this.LpressEnter.Name = "LpressEnter";
            this.LpressEnter.Size = new System.Drawing.Size(382, 36);
            this.LpressEnter.TabIndex = 2;
            this.LpressEnter.Text = "PRESS ENTER TO START";
            // 
            // finishTimer
            // 
            this.finishTimer.Interval = 40000;
            this.finishTimer.Tick += new System.EventHandler(this.finishTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(198)))), ((int)(((byte)(196)))));
            this.ClientSize = new System.Drawing.Size(756, 533);
            this.Controls.Add(this.LpressEnter);
            this.Controls.Add(this.LpresentedByText);
            this.Controls.Add(this.Lscoretable);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Horace Goes Skying";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Timer spawningTimer;
        private System.Windows.Forms.Label Lscoretable;
        private System.Windows.Forms.Label LpresentedByText;
        private System.Windows.Forms.Label LpressEnter;
        private System.Windows.Forms.Timer finishTimer;
    }
}

