using CustomDataGridViewControls.Custom;

namespace CustomDataGridViewControls
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            List<DialogItem> list1 = DialogItemGenerator.GenerateRandomDialogItems();
            List<DialogItem> list2 = DialogItemGenerator.GenerateRandomDialogItems();
            Point p = new(200,200);
            Size s = new(200, 200);

            DataGridViewListBoxColumn column1 = new(p, s, "DispData", list1);
            DataGridViewListBoxColumn column2 = new(p, s, "DispData", list2);

            // ここでOwnerを指定しないとセル編集状態になった時にフォームが隠れてしまう。
            column1.CellListBoxForm.Owner = this;
            column2.CellListBoxForm.Owner = this;

            this.dataGridView.Columns.Add(column1);
            this.dataGridView.Columns.Add(column2);
        }

        private List<DialogItem> GetTestData()
        {
            return
            [
                new(1, "Apple"),
                new(2, "Banana"),
                new(3, "Orange"),
                new(4, "Grape"),
                new(5, "Mango"),
                new(6, "Pineapple"),
                new(7, "Strawberry"),
                new(8, "Blueberry"),
                new(9, "Peach"),
                new(10, "Watermelon")
            ];
        }
    }
}
