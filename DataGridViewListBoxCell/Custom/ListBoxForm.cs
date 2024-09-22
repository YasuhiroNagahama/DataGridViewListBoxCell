namespace CustomDataGridViewControls
{
    public partial class ListBoxForm : Form
    {
        public ListBoxForm
            (Point point,
             Size size,
             string displayMember,
             object dataSource)
        {
            InitializeComponent();

            this.Location = point;
            this.Size = size;

            this.masterDataListBox.DisplayMember = displayMember;
            this.masterDataListBox.DataSource = dataSource;
        }

        public ListBox ListBox => this.masterDataListBox;
    }
}
