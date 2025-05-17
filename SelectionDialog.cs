using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PhoneFileTransfer
{
    class SelectionDialog : Form
    {
        private ListBox listBox;
        private Button btnConfirm;
        private Button btnCancel;

        public string SelectedItem { get; private set; }

        public SelectionDialog(string message, List<string> items)
        {
            // Initialize components
            this.Text = message;
            this.Width = 300;
            this.Height = 250;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create table layout for better control positioning
            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                AutoSize = true
            };
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 70)); // 70% for listBox
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 10)); // 10% space between listBox and buttons
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20)); // 20% for buttons

            listBox = new ListBox
            {
                Dock = DockStyle.Fill
            };
            listBox.Items.AddRange(items.ToArray());

            // Button container for better alignment
            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 10, 0, 0),
                AutoSize = true
            };

            btnConfirm = new Button
            {
                Text = "Conferma",
                Width = 100,
                DialogResult = DialogResult.OK
            };

            btnCancel = new Button
            {
                Text = "Annulla",
                Width = 100,
                DialogResult = DialogResult.Cancel
            };

            buttonPanel.Controls.Add(btnConfirm);
            buttonPanel.Controls.Add(btnCancel);

            // Add controls to the panel
            panel.Controls.Add(listBox, 0, 0);
            panel.Controls.Add(new Label { Height = 10 }, 0, 1); // spacer
            panel.Controls.Add(buttonPanel, 0, 2);

            // Add the panel to the form
            Controls.Add(panel);

            // Event handlers
            btnConfirm.Click += BtnConfirm_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                SelectedItem = listBox.SelectedItem.ToString();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Seleziona un elemento prima di confermare.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
