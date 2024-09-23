using System.Diagnostics;

namespace CustomDataGridViewControls.Custom
{
    public partial class CellListBoxForm : Form
    {
        private bool isHandled;

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
             List<DialogItem> dataSource)
        {
            InitializeComponent();

            this.isHandled = false;

            this.Location = point;
            this.Size = size;

            this.masterDataListBox.DisplayMember = displayMember;
            this.masterDataListBox.DataSource = dataSource;
        }

        public bool IsHandled => this.isHandled;
        public ListBox ListBox => this.masterDataListBox;

        public void CustomHide()
        {
            if(this.Visible)
            {
                base.Hide();

                // Hide イベントを発生させる
                this.FormHiding?.Invoke(this, EventArgs.Empty);

                // Hidingイベントを初期化する。
                // ここで初期化することにより、イベントハンドラ登録前に削除する必要がなくなる。（重複を意識しなくてもよくなる）;
                this.Initialize();
            }
        }

        private void ListBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.isHandled = true;
                this.CustomHide();
            }
        }

        private void ListBox_DoubleClick(object sender, EventArgs e)
        {
            this.isHandled = true;
            this.CustomHide();
        }

        private void ListBoxForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            this.CustomHide();
        }

        private void Initialize()
        {
            this.isHandled = false;
            this.FormHiding = null;
        }

        public string? Search(string input)
        {
            List<DialogItem> items = this.masterDataListBox.DataSource as List<DialogItem>;

            // 数値の場合、Noで検索
            if (int.TryParse(input, out int no))
            {
                var itemByNo = items.FirstOrDefault(i => i.No == no);
                if (itemByNo != null)
                {
                    return itemByNo.Data;
                }
            }
            else
            {
                // 文字列一致で検索
                var itemByData = items.FirstOrDefault(i => i.Data == input);
                if (itemByData != null)
                {
                    return itemByData.Data;
                }
            }

            return null;
        }
    }

    public class DataGridViewListBoxColumn(Point formPoint, Size formSize, string displayMember, List<DialogItem> DataSource) : DataGridViewColumn(new DataGridViewListBoxCell())
    {
        public readonly CellListBoxForm CellListBoxForm = new(formPoint, formSize, displayMember, DataSource);
    }

    public class DataGridViewListBoxCell : DataGridViewTextBoxCell
    {
        public override Type EditType => typeof(DataGridViewTextBoxEditingControl);

        /// <summary>
        /// 行追加時のセルの初期値として空文字を返す
        /// </summary>
        public override object DefaultNewRowValue => "";

        protected override void OnEnter(int rowIndex, bool throughMouseClick)
        {
            base.OnEnter(rowIndex, throughMouseClick);

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
                if(Value is not null) {
                    if(Value.ToString() is not null)
                    {
                        // やっぱりコントロールのTextboxでEnterを押したときも同じ処理をしなければならない

                        // セルの編集状態を終了(ここで終了しておかないと編集状態の時にセルに反映されない)
                        DataGridView?.EndEdit();

                        string input = Value.ToString();
                        string? searchValue = column.CellListBoxForm.Search(input);

                        if (searchValue is not null)
                        {
                            Value = searchValue;
                        }
                        else
                        {
                            Value = "";
                        }

                        // Listから探す処理

                        DataGridView.NotifyCurrentCellDirty(true);
                    }
                }

                column.CellListBoxForm.CustomHide();
            }
        }

        private void FormHidingHandler(object? sender, EventArgs e)
        {
            if (OwningColumn is DataGridViewListBoxColumn column)
            {
                if (column.CellListBoxForm.ListBox.SelectedItem is DialogItem dialogItem
                    && column.CellListBoxForm.IsHandled)
                {
                    // セルの編集状態を終了(ここで終了しておかないと編集状態の時にセルに反映されない)
                    DataGridView?.EndEdit();

                    Value = dialogItem.Data; // 選択された値をセルに設定

                    DataGridView.NotifyCurrentCellDirty(true);
                }
            }
        }
    }
}
