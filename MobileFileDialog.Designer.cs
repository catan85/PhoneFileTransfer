namespace PhoneFileTransfer
{
    partial class MobileFileDialog
    {/// <summary>
     /// Variabile necessaria per il supporto del designer.
     /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulisce tutte le risorse in uso.
        /// </summary>
        /// <param name="disposing">True se le risorse gestite devono essere eliminate; False altrimenti.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato dal designer

        private void InitializeComponent()
        {
            this.btnSelectFiles = new Button();
            this.lstFolders = new ListBox();
            this.lstFiles = new ListBox();
            this.btnCopyFiles = new Button();
            this.buttonUpFolder = new Button();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.progressBar1 = new ProgressBar();
            this.buttonCopyFolders = new Button();
            this.labelStatus = new Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelectFiles
            // 
            this.btnSelectFiles.Dock = DockStyle.Fill;
            this.btnSelectFiles.Location = new Point(3, 3);
            this.btnSelectFiles.Name = "btnSelectFiles";
            this.btnSelectFiles.Size = new Size(195, 24);
            this.btnSelectFiles.TabIndex = 0;
            this.btnSelectFiles.Text = "Seleziona Dispositivo";
            this.btnSelectFiles.UseVisualStyleBackColor = true;
            this.btnSelectFiles.Click += this.btnSelectDevice_Click;
            // 
            // lstFolders
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.lstFolders, 2);
            this.lstFolders.Dock = DockStyle.Fill;
            this.lstFolders.FormattingEnabled = true;
            this.lstFolders.ItemHeight = 15;
            this.lstFolders.Location = new Point(3, 33);
            this.lstFolders.Name = "lstFolders";
            this.lstFolders.Size = new Size(396, 150);
            this.lstFolders.TabIndex = 1;
            this.lstFolders.DoubleClick += this.lstFolders_DoubleClick;
            // 
            // lstFiles
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.lstFiles, 2);
            this.lstFiles.Dock = DockStyle.Fill;
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.ItemHeight = 15;
            this.lstFiles.Location = new Point(405, 33);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.SelectionMode = SelectionMode.MultiExtended;
            this.lstFiles.Size = new Size(398, 150);
            this.lstFiles.TabIndex = 2;
            this.lstFiles.SelectedIndexChanged += this.lstFiles_SelectedIndexChanged;
            // 
            // btnCopyFiles
            // 
            this.btnCopyFiles.Dock = DockStyle.Fill;
            this.btnCopyFiles.Location = new Point(606, 189);
            this.btnCopyFiles.Name = "btnCopyFiles";
            this.btnCopyFiles.Size = new Size(197, 24);
            this.btnCopyFiles.TabIndex = 3;
            this.btnCopyFiles.Text = "Copia file selezionati";
            this.btnCopyFiles.UseVisualStyleBackColor = true;
            this.btnCopyFiles.Click += this.btnCopyFiles_Click;
            // 
            // buttonUpFolder
            // 
            this.buttonUpFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.buttonUpFolder.Location = new Point(352, 3);
            this.buttonUpFolder.Name = "buttonUpFolder";
            this.buttonUpFolder.Size = new Size(47, 23);
            this.buttonUpFolder.TabIndex = 4;
            this.buttonUpFolder.Text = "<<";
            this.buttonUpFolder.UseVisualStyleBackColor = true;
            this.buttonUpFolder.Click += this.buttonUpFolder_Click;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.buttonUpFolder, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lstFolders, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectFiles, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lstFiles, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnCopyFiles, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonCopyFolders, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelStatus, 2, 2);
            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new Size(806, 216);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = DockStyle.Fill;
            this.progressBar1.Location = new Point(3, 189);
            this.progressBar1.MarqueeAnimationSpeed = 30;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new Size(195, 24);
            this.progressBar1.TabIndex = 5;
            // 
            // buttonCopyFolders
            // 
            this.buttonCopyFolders.Location = new Point(204, 189);
            this.buttonCopyFolders.Name = "buttonCopyFolders";
            this.buttonCopyFolders.Size = new Size(151, 23);
            this.buttonCopyFolders.TabIndex = 6;
            this.buttonCopyFolders.Text = "Copia folder selezionate";
            this.buttonCopyFolders.UseMnemonic = false;
            this.buttonCopyFolders.UseVisualStyleBackColor = true;
            this.buttonCopyFolders.Click += this.button1_Click;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new Point(405, 186);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new Size(45, 15);
            this.labelStatus.TabIndex = 7;
            this.labelStatus.Text = "Status: ";
            // 
            // MobileFileDialog
            // 
            this.ClientSize = new Size(806, 216);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MobileFileDialog";
            this.Text = "Selezione File MTP";
            this.Load += this.MobileFileDialog_Load;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button btnSelectFiles;
        private System.Windows.Forms.ListBox lstFolders;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Button btnCopyFiles;
        private Button buttonUpFolder;
        private TableLayoutPanel tableLayoutPanel1;
        private ProgressBar progressBar1;
        private Button buttonCopyFolders;
        private Label labelStatus;
    }
}
