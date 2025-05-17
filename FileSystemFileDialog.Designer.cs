namespace PhoneFileTransfer
{
    partial class FileSystemFileDialog
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
            this.lstFolders = new ListBox();
            this.lstFiles = new ListBox();
            this.btnCopyFiles = new Button();
            this.buttonUpFolder = new Button();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.labelCurrentDirectory = new Label();
            this.btnCopyDir = new Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
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
            this.lstFolders.SelectionMode = SelectionMode.MultiExtended;
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
            // 
            // btnCopyFiles
            // 
            this.btnCopyFiles.Dock = DockStyle.Fill;
            this.btnCopyFiles.Location = new Point(606, 189);
            this.btnCopyFiles.Name = "btnCopyFiles";
            this.btnCopyFiles.Size = new Size(197, 24);
            this.btnCopyFiles.TabIndex = 3;
            this.btnCopyFiles.Text = "Copia files selezionati";
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
            this.tableLayoutPanel1.Controls.Add(this.btnCopyDir, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonUpFolder, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lstFolders, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lstFiles, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnCopyFiles, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelCurrentDirectory, 0, 0);
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
            // labelCurrentDirectory
            // 
            this.labelCurrentDirectory.AutoSize = true;
            this.labelCurrentDirectory.Location = new Point(3, 0);
            this.labelCurrentDirectory.Name = "labelCurrentDirectory";
            this.labelCurrentDirectory.Size = new Size(120, 15);
            this.labelCurrentDirectory.TabIndex = 6;
            this.labelCurrentDirectory.Text = "labelCurrentDirectory";
            // 
            // btnCopyDir
            // 
            this.btnCopyDir.Dock = DockStyle.Fill;
            this.btnCopyDir.Location = new Point(204, 189);
            this.btnCopyDir.Name = "btnCopyDir";
            this.btnCopyDir.Size = new Size(195, 24);
            this.btnCopyDir.TabIndex = 7;
            this.btnCopyDir.Text = "Copia directory selezionate";
            this.btnCopyDir.UseVisualStyleBackColor = true;
            this.btnCopyDir.Click += this.btnCopyDir_Click;
            // 
            // FileSystemFileDialog
            // 
            this.ClientSize = new Size(806, 216);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FileSystemFileDialog";
            this.Text = "Selezione File e Folders";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ListBox lstFolders;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Button btnCopyFiles;
        private Button buttonUpFolder;
        private TableLayoutPanel tableLayoutPanel1;
        private Label labelCurrentDirectory;
        private Button btnCopyDir;
    }
}