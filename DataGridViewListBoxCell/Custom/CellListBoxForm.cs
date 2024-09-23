using System.Diagnostics;

namespace CustomDataGridViewControls.Custom
{
    public partial class CellListBoxForm : Form
    {
        public event EventHandler FormHiding;

        /// <summary>
        /// コンストラクタ。フォームの位置、サイズ、表示メンバー、データソースを設定
        /// </summary>
        /// <param name="point">フォームの位置を示す座標</param>
        /// <param name="size">フォームのサイズ</param>
        /// <param name="displayMember">ListBoxに表示するメンバー</param>
        /// <param name="dataSource">ListBoxにバインドするデータソース</param>
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

        /// <summary>
        /// ListBoxコントロールを取得
        /// </summary>
        public ListBox ListBox => this.masterDataListBox;

        public void CustomHide()
        {
            base.Hide();

            // Hide イベントを発生させる
            this.FormHiding?.Invoke(this, EventArgs.Empty);

            // Hidingイベントを初期化する。
            // ここで初期化することにより、イベントハンドラ登録前に削除する必要がなくなる。（重複を意識しなくてもよくなる）;
            this.ResetHidingEvents();
        }

        private void ListBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.CustomHide();
            }
        }

        private void ListBox_DoubleClick(object sender, EventArgs e)
        {
            this.CustomHide();
        }

        private void ListBoxForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.CustomHide();
        }

        private void ResetHidingEvents()
        {
            this.FormHiding = null;
        }
    }

    public class DataGridViewListBoxColumn : DataGridViewColumn
    {
        public readonly CellListBoxForm CellListBoxForm;

        // フォームのListBoxの表示メンバー
        public string DisplayMember;
        // フォームのListBoxのデータソース
        public object DataSource;
        // 表示するフォームの位置
        public Point FormPoint { get; set; }
        // 表示するフォームの大きさ
        public Size FormSize { get; set; }

        /// <summary>
        /// コンストラクタ。カスタムセル（DataGridViewListBoxCell）を列のセルテンプレートとして設定
        /// </summary>
        public DataGridViewListBoxColumn(string displayMember, object DataSource, Point formPoint, Size formSize)
            : base(new DataGridViewListBoxCell())
        {
            this.DisplayMember = displayMember;
            this.DataSource = DataSource;
            this.FormPoint = formPoint;
            this.FormSize = formSize;

            this.CellListBoxForm = new(this.FormPoint, this.FormSize, this.DisplayMember, this.DataSource);
        }

        /// <summary>
        /// 列をクローンし、データソースを新しい列にもコピーします。
        /// </summary>
        /// <returns>クローンされたDataGridViewListBoxColumnオブジェクト</returns>
        public override object Clone()
        {
            var clone = (DataGridViewListBoxColumn)base.Clone();
            clone.DataSource = DataSource;
            return clone;
        }
    }

    public class DataGridViewListBoxCell : DataGridViewTextBoxCell
    {
        /// <summary>
        /// 編集モードを開始する際にコントロールを初期化
        /// </summary>
        /// <remarks>
        /// セルが編集モードに入ったときに呼び出されます。
        /// </remarks>
        /// <param name="rowIndex">編集する行のインデックス</param>
        /// <param name="initialFormattedValue">初期の書式設定済みの値</param>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            // 編集モードを開始する際にコントロールを初期化
            //if (DataGridView.EditingControl is DataGridViewListBoxEditingControl control)
            //{
            //    var column = OwningColumn as DataGridViewListBoxColumn;
            //    control.DataSource = column?.DataSource;
            //}
        }

        /// <summary>
        /// セルの編集コントロールとしてNullを返す
        /// </summary>
        /// <remarks>
        /// セルの編集コントロールの型をNullに設定することにより、通常の編集機能を無効化する。
        /// </remarks>
        public override Type EditType => typeof(DataGridViewTextBoxEditingControl);

        /// <summary>
        /// 行追加時のセルの初期値として空文字を返す
        /// </summary>
        public override object DefaultNewRowValue => "";

        protected override void OnClick(DataGridViewCellEventArgs e)
        {
            base.OnClick(e);

            if (OwningColumn is DataGridViewListBoxColumn column)
            {
                column.CellListBoxForm.FormHiding += this.FormHidingHandler;

                column.CellListBoxForm.Show();
            }
        }

        protected override void OnLeave(int rowIndex, bool throughMouseClick)
        {
            base.OnLeave(rowIndex, throughMouseClick);

            if(OwningColumn is DataGridViewListBoxColumn column)
            {
                column.CellListBoxForm.CustomHide();
            }
        }

        private void FormHidingHandler(object? sender, EventArgs e)
        {
            if (OwningColumn is DataGridViewListBoxColumn column)
            {
                if (column.CellListBoxForm.ListBox.SelectedItem != null)
                {

                    if (column.CellListBoxForm.ListBox.SelectedItem is DialogItem dialogItem)
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
