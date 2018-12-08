namespace MacroCoach
{
    partial class KeyDebugger
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
            this.btnDebugStart = new System.Windows.Forms.Button();
            this.btnDebugStop = new System.Windows.Forms.Button();
            this.txtKeystrokeDebugger = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDebugStart
            // 
            this.btnDebugStart.Location = new System.Drawing.Point(15, 75);
            this.btnDebugStart.Name = "btnDebugStart";
            this.btnDebugStart.Size = new System.Drawing.Size(98, 31);
            this.btnDebugStart.TabIndex = 0;
            this.btnDebugStart.Text = "Start";
            this.btnDebugStart.UseVisualStyleBackColor = true;
            this.btnDebugStart.Click += new System.EventHandler(this.btnDebugStart_Click);
            // 
            // btnDebugStop
            // 
            this.btnDebugStop.Enabled = false;
            this.btnDebugStop.Location = new System.Drawing.Point(119, 75);
            this.btnDebugStop.Name = "btnDebugStop";
            this.btnDebugStop.Size = new System.Drawing.Size(98, 31);
            this.btnDebugStop.TabIndex = 1;
            this.btnDebugStop.Text = "Stop";
            this.btnDebugStop.UseVisualStyleBackColor = true;
            this.btnDebugStop.Click += new System.EventHandler(this.btnDebugStop_Click);
            // 
            // txtKeystrokeDebugger
            // 
            this.txtKeystrokeDebugger.Enabled = false;
            this.txtKeystrokeDebugger.Location = new System.Drawing.Point(15, 115);
            this.txtKeystrokeDebugger.Multiline = true;
            this.txtKeystrokeDebugger.Name = "txtKeystrokeDebugger";
            this.txtKeystrokeDebugger.Size = new System.Drawing.Size(202, 320);
            this.txtKeystrokeDebugger.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.MaximumSize = new System.Drawing.Size(200, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 52);
            this.label1.TabIndex = 3;
            this.label1.Text = "Dumps the characters read on the keyboard every second to ensure a) things are wo" +
    "rking and b) you have the right character for your button.";
            // 
            // KeyDebugger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 455);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtKeystrokeDebugger);
            this.Controls.Add(this.btnDebugStop);
            this.Controls.Add(this.btnDebugStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "KeyDebugger";
            this.Text = "KeyDebugger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KeyDebugger_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDebugStart;
        private System.Windows.Forms.Button btnDebugStop;
        private System.Windows.Forms.TextBox txtKeystrokeDebugger;
        private System.Windows.Forms.Label label1;
    }
}