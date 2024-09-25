using CustomDataGridViewControls.Custom;
using CustomDataGridViewControls.Test2;
using System.Diagnostics;

namespace CustomDataGridViewControls
{
    public partial class BindingTestForm : Form
    {
        private readonly ViewModel _viewModel;


        public BindingTestForm()
        {
            InitializeComponent();

            this._viewModel = new();

            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn tC1 = new();
            tC1.DataPropertyName = "UserName";
            tC1.HeaderText = "ユーザー名";
            DataGridViewTextBoxColumn tC2 = new();
            tC2.DataPropertyName = "Age";
            tC2.HeaderText = "年齢";
            DataGridViewTextBoxColumn tC3 = new();
            tC3.DataPropertyName = "Description";
            tC3.HeaderText = "説明";

            Point p = new(200, 200);
            Size s = new(200, 200);
            List<DialogItem> dialogItems = DialogItemGenerator.GenerateRandomDialogItems();

            DataGridViewListBoxColumn tC4 = new(p, s, Constants.DIALOG_DISP_DATA, dialogItems);
            tC4.DataPropertyName = "DispData";
            tC4.CellListBoxForm.Owner = this;

            this.dataGridView.Columns.Add(tC1);
            this.dataGridView.Columns.Add(tC2);
            this.dataGridView.Columns.Add(tC3);
            this.dataGridView.Columns.Add(tC4);

            this.dataGridView.DataSource = this._viewModel.rowDataList;
        }

        private void BindingTestForm_Load(object sender, EventArgs e)
        {

        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            this._viewModel.rowDataList.Add(new());
        }

        private void AddUserNameButton_Click(object sender, EventArgs e)
        {
            this._viewModel.rowDataList[0].UserName = "竹村";
        }
    }
}
