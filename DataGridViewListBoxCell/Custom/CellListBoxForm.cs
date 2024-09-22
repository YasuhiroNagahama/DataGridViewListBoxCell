namespace CustomDataGridViewControls.Custom
{
    public partial class CellListBoxForm : Form
    {
        public CellListBoxForm
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

    public class DataGridViewListBoxColumn : DataGridViewColumn
    {
        /// <summary>
        /// 派生元クラス（DataGirdViewColumn）にカスタムセル（DataGridViweListBoxCell）
        /// を渡し列内のセルテンプレートを設定します。
        /// セルクリック時にDataGridViweListBoxCellの動作になります。
        /// </summary>
        public DataGridViewListBoxColumn()
            : base(new DataGridViewListBoxCell())
        {
        }

        // フォームのListBoxに表示するデータソース
        public object DataSource { get; set; }
        public string DisplayMember { get; set; }
        // 表示するフォームの位置
        public Point FormPoint { get; set; }
        // 表示するフォームの大きさ
        public Size FormSize { get; set; }

        /// <summary>
        /// クローンメソッドがない場合の問題
        /// もし、Clone()メソッドをオーバーライドせず、デフォルトのままにしておくと、DataGridViewが列を複製したときにDataSourceの値が新しい列にコピーされません。その結果、複製された列ではDataSourceが設定されておらず、ListBoxのデータ表示ができなくなる可能性があります。
        /// クローンメソッドをオーバーライドする理由:
        /// Clone()メソッドをオーバーライドして、複製された列でもDataSourceが引き継がれるようにするためです。以下のコードでは、オーバーライドされたClone()メソッド内でDataSourceを手動でコピーしています。
        /// </summary>
        public override object Clone()
        {
            var clone = (DataGridViewListBoxColumn)base.Clone();
            clone.DataSource = DataSource;
            return clone;
        }
    }

    public class DataGridViewListBoxCell : DataGridViewTextBoxCell
    {
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            // 編集モードを開始する際にコントロールを初期化
            var control = DataGridView.EditingControl as DataGridViewListBoxEditingControl;
            if (control != null)
            {
                var column = OwningColumn as DataGridViewListBoxColumn;
                control.DataSource = column?.DataSource;
            }
        }

        public override Type EditType => null;

        public override object DefaultNewRowValue => string.Empty;

        protected override void OnClick(DataGridViewCellEventArgs e)
        {
            base.OnClick(e);

            var column = OwningColumn as DataGridViewListBoxColumn;

            if (column?.DataSource != null)
            {
                CellListBoxForm listBoxForm = new(column.FormPoint, column.FormSize, column.DisplayMember, column.DataSource);

                listBoxForm.ListBox.KeyPress += (s, e) =>
                {
                    if (e.KeyChar == (char)Keys.Enter)
                    {
                        listBoxForm.Close();
                    }
                };

                listBoxForm.ListBox.DoubleClick += (s, e) =>
                {
                    listBoxForm.Close();
                };

                listBoxForm.FormClosing += (s, e) =>
                {
                    column.FormPoint = listBoxForm.Location;
                    column.FormSize = listBoxForm.Size;
                };


                listBoxForm.ShowDialog();

                if (listBoxForm.ListBox.SelectedItem != null)
                {
                    if (listBoxForm.ListBox.SelectedItem is DialogItem dialogItem)
                    {
                        Value = dialogItem.Data; // 選択された値をセルに設定
                    }

                    DataGridView.NotifyCurrentCellDirty(true);
                }
            }
        }
    }

    public class DataGridViewListBoxEditingControl : ListBox, IDataGridViewEditingControl
    {
        public DataGridView EditingControlDataGridView { get; set; }
        public object EditingControlFormattedValue
        {
            get => SelectedItem is DialogItem dialogItem ? dialogItem.Data : string.Empty;
            set => SelectedItem = value;
        }
        public int EditingControlRowIndex { get; set; }
        public bool EditingControlValueChanged { get; set; }
        public Cursor EditingPanelCursor => Cursors.Default;

        public bool RepositionEditingControlOnValueChange => false;

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            // スタイル適用
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            return keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.Up || keyData == Keys.Down;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // 編集の準備
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            EditingControlValueChanged = true;
            EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }
    }
}
