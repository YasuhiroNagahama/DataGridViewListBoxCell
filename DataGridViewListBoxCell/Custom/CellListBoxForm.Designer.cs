namespace CustomDataGridViewControls.Custom
{
    partial class CellListBoxForm
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
            masterDataListBox = new ListBox();
            SuspendLayout();
            // 
            // masterDataListBox
            // 
            masterDataListBox.Dock = DockStyle.Fill;
            masterDataListBox.Font = new Font("Yu Gothic UI", 16F);
            masterDataListBox.FormattingEnabled = true;
            masterDataListBox.ItemHeight = 30;
            masterDataListBox.Location = new Point(0, 0);
            masterDataListBox.MultiColumn = true;
            masterDataListBox.Name = "masterDataListBox";
            masterDataListBox.Size = new Size(495, 325);
            masterDataListBox.TabIndex = 0;
            // 
            // CellListBoxForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(495, 325);
            Controls.Add(masterDataListBox);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CellListBoxForm";
            Text = "CellListBoxForm";
            ResumeLayout(false);
        }

        #endregion

        private ListBox masterDataListBox;
    }
}