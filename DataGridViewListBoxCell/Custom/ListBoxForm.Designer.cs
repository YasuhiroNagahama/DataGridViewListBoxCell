namespace CustomDataGridViewControls
{
    partial class ListBoxForm
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
            masterDataListBox.ColumnWidth = 320;
            masterDataListBox.DisplayMember = "DispData";
            masterDataListBox.Dock = DockStyle.Fill;
            masterDataListBox.Font = new Font("Yu Gothic UI", 16F);
            masterDataListBox.FormattingEnabled = true;
            masterDataListBox.ItemHeight = 30;
            masterDataListBox.Location = new Point(0, 0);
            masterDataListBox.MultiColumn = true;
            masterDataListBox.Name = "masterDataListBox";
            masterDataListBox.Size = new Size(572, 366);
            masterDataListBox.TabIndex = 0;
            // 
            // ListBoxForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(572, 366);
            Controls.Add(masterDataListBox);
            Name = "ListBoxForm";
            Text = "ListBoxForm";
            ResumeLayout(false);
        }

        #endregion

        private ListBox masterDataListBox;
    }
}