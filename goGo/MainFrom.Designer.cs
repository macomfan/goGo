namespace goGo
{
    partial class MainFrom
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
            this.splitPanelMain = new System.Windows.Forms.SplitContainer();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.mainView1 = new goGo.GoView();
            this.button1 = new System.Windows.Forms.Button();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanelMain)).BeginInit();
            this.splitPanelMain.Panel1.SuspendLayout();
            this.splitPanelMain.Panel2.SuspendLayout();
            this.splitPanelMain.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitPanelMain
            // 
            this.splitPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPanelMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitPanelMain.IsSplitterFixed = true;
            this.splitPanelMain.Location = new System.Drawing.Point(0, 26);
            this.splitPanelMain.Name = "splitPanelMain";
            // 
            // splitPanelMain.Panel1
            // 
            this.splitPanelMain.Panel1.AutoScroll = true;
            this.splitPanelMain.Panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.splitPanelMain.Panel1.Controls.Add(this.mainPanel);
            this.splitPanelMain.Panel1MinSize = 0;
            // 
            // splitPanelMain.Panel2
            // 
            this.splitPanelMain.Panel2.Controls.Add(this.button1);
            this.splitPanelMain.Panel2MinSize = 100;
            this.splitPanelMain.Size = new System.Drawing.Size(735, 415);
            this.splitPanelMain.SplitterDistance = 550;
            this.splitPanelMain.SplitterWidth = 1;
            this.splitPanelMain.TabIndex = 1;
            // 
            // mainPanel
            // 
            this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mainPanel.Controls.Add(this.mainView1);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(550, 415);
            this.mainPanel.TabIndex = 0;
            // 
            // mainView1
            // 
            this.mainView1.BackColor = System.Drawing.SystemColors.Window;
            this.mainView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainView1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.mainView1.Location = new System.Drawing.Point(0, 0);
            this.mainView1.Margin = new System.Windows.Forms.Padding(0);
            this.mainView1.Name = "mainView1";
            this.mainView1.Size = new System.Drawing.Size(546, 411);
            this.mainView1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(83, 60);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(735, 26);
            this.menuMain.TabIndex = 2;
            this.menuMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(40, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // MainFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 441);
            this.Controls.Add(this.splitPanelMain);
            this.Controls.Add(this.menuMain);
            this.MainMenuStrip = this.menuMain;
            this.Name = "MainFrom";
            this.Text = "Form1";
            this.splitPanelMain.Panel1.ResumeLayout(false);
            this.splitPanelMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPanelMain)).EndInit();
            this.splitPanelMain.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitPanelMain;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private GoView mainView1;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Button button1;

    }
}

