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
            List<DialogItem> list = this.GetTestData();
            DataGridViewListBoxColumn column = new()
            {
                DataSource = list,
                DisplayMember = "DispData",
                FormPoint = new Point(200, 200),
                FormSize = new Size(1000, 200)
            };

            this.dataGridView.Columns.Add(column);
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
