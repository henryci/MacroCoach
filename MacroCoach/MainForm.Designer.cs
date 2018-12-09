namespace MacroCoach
{
    partial class MainForm
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
            this.listAlerts = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.keyDebuggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblSecondsBetweenAlerts = new System.Windows.Forms.Label();
            this.numSecondsBetweenAlerts = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboKeystroke = new System.Windows.Forms.ComboBox();
            this.btnAddAlert = new System.Windows.Forms.Button();
            this.lbltxtAlertText = new System.Windows.Forms.Label();
            this.lblDelayBeforeAlerting = new System.Windows.Forms.Label();
            this.lblAlertCharacter = new System.Windows.Forms.Label();
            this.numDelayBeforeAlerting = new System.Windows.Forms.NumericUpDown();
            this.txtAlertText = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.chkNotifyOnStartStop = new System.Windows.Forms.CheckBox();
            this.btnDeleteAlert = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSecondsBetweenAlerts)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDelayBeforeAlerting)).BeginInit();
            this.SuspendLayout();
            // 
            // listAlerts
            // 
            this.listAlerts.FormattingEnabled = true;
            this.listAlerts.Location = new System.Drawing.Point(12, 232);
            this.listAlerts.Name = "listAlerts";
            this.listAlerts.Size = new System.Drawing.Size(347, 69);
            this.listAlerts.TabIndex = 12;
            this.listAlerts.SelectedIndexChanged += new System.EventHandler(this.listAlerts_SelectedIndexChanged);
            this.listAlerts.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.listAlerts_Format);
            this.listAlerts.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.listAlerts_PreviewKeyDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keyDebuggerToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(374, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // keyDebuggerToolStripMenuItem
            // 
            this.keyDebuggerToolStripMenuItem.Name = "keyDebuggerToolStripMenuItem";
            this.keyDebuggerToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.keyDebuggerToolStripMenuItem.Text = "Key Debugger";
            this.keyDebuggerToolStripMenuItem.Click += new System.EventHandler(this.keyDebuggerToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.aboutToolStripMenuItem.Text = "Help / About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // lblSecondsBetweenAlerts
            // 
            this.lblSecondsBetweenAlerts.AutoSize = true;
            this.lblSecondsBetweenAlerts.Location = new System.Drawing.Point(9, 40);
            this.lblSecondsBetweenAlerts.Name = "lblSecondsBetweenAlerts";
            this.lblSecondsBetweenAlerts.Size = new System.Drawing.Size(132, 13);
            this.lblSecondsBetweenAlerts.TabIndex = 2;
            this.lblSecondsBetweenAlerts.Text = "Seconds between alerts❓";
            // 
            // numSecondsBetweenAlerts
            // 
            this.numSecondsBetweenAlerts.Location = new System.Drawing.Point(139, 38);
            this.numSecondsBetweenAlerts.Name = "numSecondsBetweenAlerts";
            this.numSecondsBetweenAlerts.Size = new System.Drawing.Size(41, 20);
            this.numSecondsBetweenAlerts.TabIndex = 0;
            this.numSecondsBetweenAlerts.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numSecondsBetweenAlerts.ValueChanged += new System.EventHandler(this.numSecondsBetweenAlerts_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.comboKeystroke);
            this.panel1.Controls.Add(this.btnAddAlert);
            this.panel1.Controls.Add(this.lbltxtAlertText);
            this.panel1.Controls.Add(this.lblDelayBeforeAlerting);
            this.panel1.Controls.Add(this.lblAlertCharacter);
            this.panel1.Controls.Add(this.numDelayBeforeAlerting);
            this.panel1.Controls.Add(this.txtAlertText);
            this.panel1.Location = new System.Drawing.Point(12, 88);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(347, 138);
            this.panel1.TabIndex = 4;
            // 
            // comboKeystroke
            // 
            this.comboKeystroke.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboKeystroke.FormattingEnabled = true;
            this.comboKeystroke.Location = new System.Drawing.Point(6, 29);
            this.comboKeystroke.Name = "comboKeystroke";
            this.comboKeystroke.Size = new System.Drawing.Size(62, 21);
            this.comboKeystroke.TabIndex = 2;
            // 
            // btnAddAlert
            // 
            this.btnAddAlert.Location = new System.Drawing.Point(73, 102);
            this.btnAddAlert.Name = "btnAddAlert";
            this.btnAddAlert.Size = new System.Drawing.Size(203, 25);
            this.btnAddAlert.TabIndex = 5;
            this.btnAddAlert.Text = "Add an alert";
            this.btnAddAlert.UseVisualStyleBackColor = true;
            this.btnAddAlert.Click += new System.EventHandler(this.btnAddAlert_Click);
            // 
            // lbltxtAlertText
            // 
            this.lbltxtAlertText.AutoSize = true;
            this.lbltxtAlertText.Location = new System.Drawing.Point(3, 60);
            this.lbltxtAlertText.Name = "lbltxtAlertText";
            this.lbltxtAlertText.Size = new System.Drawing.Size(84, 13);
            this.lbltxtAlertText.TabIndex = 5;
            this.lbltxtAlertText.Text = "Alert message❓";
            // 
            // lblDelayBeforeAlerting
            // 
            this.lblDelayBeforeAlerting.AutoSize = true;
            this.lblDelayBeforeAlerting.Location = new System.Drawing.Point(87, 13);
            this.lblDelayBeforeAlerting.Name = "lblDelayBeforeAlerting";
            this.lblDelayBeforeAlerting.Size = new System.Drawing.Size(130, 13);
            this.lblDelayBeforeAlerting.TabIndex = 4;
            this.lblDelayBeforeAlerting.Text = "Seconds before alerting❓";
            // 
            // lblAlertCharacter
            // 
            this.lblAlertCharacter.AutoSize = true;
            this.lblAlertCharacter.Location = new System.Drawing.Point(3, 13);
            this.lblAlertCharacter.Name = "lblAlertCharacter";
            this.lblAlertCharacter.Size = new System.Drawing.Size(65, 13);
            this.lblAlertCharacter.TabIndex = 3;
            this.lblAlertCharacter.Text = "Keystroke❓";
            // 
            // numDelayBeforeAlerting
            // 
            this.numDelayBeforeAlerting.Location = new System.Drawing.Point(90, 32);
            this.numDelayBeforeAlerting.Name = "numDelayBeforeAlerting";
            this.numDelayBeforeAlerting.Size = new System.Drawing.Size(52, 20);
            this.numDelayBeforeAlerting.TabIndex = 3;
            this.numDelayBeforeAlerting.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numDelayBeforeAlerting.ValueChanged += new System.EventHandler(this.numDelayBeforeAlerting_ValueChanged);
            // 
            // txtAlertText
            // 
            this.txtAlertText.Location = new System.Drawing.Point(6, 76);
            this.txtAlertText.Name = "txtAlertText";
            this.txtAlertText.Size = new System.Drawing.Size(336, 20);
            this.txtAlertText.TabIndex = 4;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(13, 340);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(141, 33);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(214, 340);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(141, 33);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // chkNotifyOnStartStop
            // 
            this.chkNotifyOnStartStop.AutoSize = true;
            this.chkNotifyOnStartStop.Location = new System.Drawing.Point(12, 65);
            this.chkNotifyOnStartStop.Name = "chkNotifyOnStartStop";
            this.chkNotifyOnStartStop.Size = new System.Drawing.Size(127, 17);
            this.chkNotifyOnStartStop.TabIndex = 1;
            this.chkNotifyOnStartStop.Text = "Notify on start/stop❓";
            this.chkNotifyOnStartStop.UseVisualStyleBackColor = true;
            // 
            // btnDeleteAlert
            // 
            this.btnDeleteAlert.Enabled = false;
            this.btnDeleteAlert.Location = new System.Drawing.Point(281, 304);
            this.btnDeleteAlert.Name = "btnDeleteAlert";
            this.btnDeleteAlert.Size = new System.Drawing.Size(78, 28);
            this.btnDeleteAlert.TabIndex = 13;
            this.btnDeleteAlert.Text = "Delete";
            this.btnDeleteAlert.UseVisualStyleBackColor = true;
            this.btnDeleteAlert.Click += new System.EventHandler(this.btnDeleteAlert_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 383);
            this.Controls.Add(this.btnDeleteAlert);
            this.Controls.Add(this.chkNotifyOnStartStop);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.numSecondsBetweenAlerts);
            this.Controls.Add(this.lblSecondsBetweenAlerts);
            this.Controls.Add(this.listAlerts);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Henry\'s Macro Coach";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSecondsBetweenAlerts)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDelayBeforeAlerting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listAlerts;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem keyDebuggerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label lblSecondsBetweenAlerts;
        private System.Windows.Forms.NumericUpDown numSecondsBetweenAlerts;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblAlertCharacter;
        private System.Windows.Forms.NumericUpDown numDelayBeforeAlerting;
        private System.Windows.Forms.TextBox txtAlertText;
        private System.Windows.Forms.Button btnAddAlert;
        private System.Windows.Forms.Label lbltxtAlertText;
        private System.Windows.Forms.Label lblDelayBeforeAlerting;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox chkNotifyOnStartStop;
        private System.Windows.Forms.ComboBox comboKeystroke;
        private System.Windows.Forms.Button btnDeleteAlert;
    }
}

