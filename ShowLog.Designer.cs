namespace ParsetCrawler
{
    partial class ShowLog
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
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tmrStatus = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.tmrCurrency = new System.Windows.Forms.Timer(this.components);
            this.bgWorkerCurrency = new System.ComponentModel.BackgroundWorker();
            this.btnCurrency = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // tmrStatus
            // 
            this.tmrStatus.Interval = 1000;
            this.tmrStatus.Tick += new System.EventHandler(this.tmrStatus_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 286);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(491, 33);
            this.button1.TabIndex = 4;
            this.button1.Text = "Crawl Now !";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtLog.Location = new System.Drawing.Point(12, 12);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(733, 268);
            this.txtLog.TabIndex = 3;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(629, 287);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(116, 32);
            this.btnClearLog.TabIndex = 5;
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // tmrCurrency
            // 
            this.tmrCurrency.Interval = 86400000;
            this.tmrCurrency.Tick += new System.EventHandler(this.tmrCurrency_Tick);
            // 
            // bgWorkerCurrency
            // 
            this.bgWorkerCurrency.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerCurrency_DoWork);
            // 
            // btnCurrency
            // 
            this.btnCurrency.Location = new System.Drawing.Point(507, 287);
            this.btnCurrency.Name = "btnCurrency";
            this.btnCurrency.Size = new System.Drawing.Size(116, 32);
            this.btnCurrency.TabIndex = 6;
            this.btnCurrency.Text = "Currency";
            this.btnCurrency.UseVisualStyleBackColor = true;
            this.btnCurrency.Click += new System.EventHandler(this.btnCurrency_Click);
            // 
            // ShowLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 336);
            this.ControlBox = false;
            this.Controls.Add(this.btnCurrency);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtLog);
            this.Name = "ShowLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Khabar Auto Update";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TerminateProgram);
            this.Load += new System.EventHandler(this.ShowLog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrUpdate;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer tmrStatus;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Timer tmrCurrency;
        private System.ComponentModel.BackgroundWorker bgWorkerCurrency;
        private System.Windows.Forms.Button btnCurrency;
    }
}

