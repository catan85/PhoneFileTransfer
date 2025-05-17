namespace PhoneFileTransfer
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
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.buttonRemove = new Button();
            this.buttonCopy = new Button();
            this.buttonRemoveJobs = new Button();
            this.dataGridView1 = new DataGridView();
            this.buttonAddJobs = new Button();
            this.labelStatusTitle = new Label();
            this.labelStatus = new Label();
            this.checkBoxMediaDevice = new CheckBox();
            this.progressBar1 = new ProgressBar();
            this.buttonCopyAndRemove = new Button();
            this.checkBoxAdbDriver = new CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.buttonRemove, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonCopy, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonRemoveJobs, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonAddJobs, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelStatusTitle, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelStatus, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxMediaDevice, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonCopyAndRemove, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxAdbDriver, 4, 3);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new Size(833, 496);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Dock = DockStyle.Fill;
            this.buttonRemove.Location = new Point(501, 379);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new Size(160, 44);
            this.buttonRemove.TabIndex = 4;
            this.buttonRemove.Text = "Remove source";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += this.buttonRemove_Click;
            // 
            // buttonCopy
            // 
            this.buttonCopy.Dock = DockStyle.Fill;
            this.buttonCopy.Location = new Point(335, 379);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new Size(160, 44);
            this.buttonCopy.TabIndex = 3;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += this.buttonCopy_Click;
            // 
            // buttonRemoveJobs
            // 
            this.buttonRemoveJobs.Dock = DockStyle.Fill;
            this.buttonRemoveJobs.Location = new Point(169, 379);
            this.buttonRemoveJobs.Name = "buttonRemoveJobs";
            this.buttonRemoveJobs.Size = new Size(160, 44);
            this.buttonRemoveJobs.TabIndex = 2;
            this.buttonRemoveJobs.Text = "Remove Jobs";
            this.buttonRemoveJobs.UseVisualStyleBackColor = true;
            this.buttonRemoveJobs.Click += this.buttonRemoveJobs_Click;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridView1, 5);
            this.dataGridView1.Dock = DockStyle.Fill;
            this.dataGridView1.Location = new Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new Size(827, 340);
            this.dataGridView1.TabIndex = 0;
            // 
            // buttonAddJobs
            // 
            this.buttonAddJobs.Dock = DockStyle.Fill;
            this.buttonAddJobs.Location = new Point(3, 379);
            this.buttonAddJobs.Name = "buttonAddJobs";
            this.buttonAddJobs.Size = new Size(160, 44);
            this.buttonAddJobs.TabIndex = 1;
            this.buttonAddJobs.Text = "Add Jobs";
            this.buttonAddJobs.UseVisualStyleBackColor = true;
            this.buttonAddJobs.Click += this.buttonAddJobs_Click;
            // 
            // labelStatusTitle
            // 
            this.labelStatusTitle.AutoSize = true;
            this.labelStatusTitle.Location = new Point(3, 346);
            this.labelStatusTitle.Name = "labelStatusTitle";
            this.labelStatusTitle.Size = new Size(39, 15);
            this.labelStatusTitle.TabIndex = 5;
            this.labelStatusTitle.Text = "Status";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelStatus, 4);
            this.labelStatus.Dock = DockStyle.Top;
            this.labelStatus.Location = new Point(169, 346);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new Size(661, 15);
            this.labelStatus.TabIndex = 6;
            this.labelStatus.Text = "Idle";
            // 
            // checkBoxMediaDevice
            // 
            this.checkBoxMediaDevice.AutoSize = true;
            this.checkBoxMediaDevice.Location = new Point(667, 379);
            this.checkBoxMediaDevice.Name = "checkBoxMediaDevice";
            this.checkBoxMediaDevice.Size = new Size(97, 19);
            this.checkBoxMediaDevice.TabIndex = 7;
            this.checkBoxMediaDevice.Text = "Mobile mode";
            this.checkBoxMediaDevice.UseVisualStyleBackColor = true;
            this.checkBoxMediaDevice.CheckedChanged += this.checkBoxMediaDevice_CheckedChanged;
            // 
            // progressBar1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.progressBar1, 5);
            this.progressBar1.Dock = DockStyle.Fill;
            this.progressBar1.Location = new Point(3, 479);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new Size(827, 14);
            this.progressBar1.Style = ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 8;
            // 
            // buttonCopyAndRemove
            // 
            this.buttonCopyAndRemove.Location = new Point(335, 429);
            this.buttonCopyAndRemove.Name = "buttonCopyAndRemove";
            this.buttonCopyAndRemove.Size = new Size(160, 44);
            this.buttonCopyAndRemove.TabIndex = 9;
            this.buttonCopyAndRemove.Text = "Copy and Remove";
            this.buttonCopyAndRemove.UseVisualStyleBackColor = true;
            this.buttonCopyAndRemove.Click += this.buttonCopyAndRemove_Click;
            // 
            // checkBoxAdbDriver
            // 
            this.checkBoxAdbDriver.AutoSize = true;
            this.checkBoxAdbDriver.Location = new Point(667, 429);
            this.checkBoxAdbDriver.Name = "checkBoxAdbDriver";
            this.checkBoxAdbDriver.Size = new Size(81, 19);
            this.checkBoxAdbDriver.TabIndex = 12;
            this.checkBoxAdbDriver.Text = "Adb driver";
            this.checkBoxAdbDriver.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(833, 496);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "Phone File Transfer";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dataGridView1;
        private Button buttonRemoveJobs;
        private Button buttonAddJobs;
        private Button buttonCopy;
        private Button buttonRemove;
        private Label labelStatus;
        private Label labelStatusTitle;
        private CheckBox checkBoxMediaDevice;
        private ProgressBar progressBar1;
        private Button buttonCopyAndRemove;
        private CheckBox checkBoxAdbDriver;
    }
}
