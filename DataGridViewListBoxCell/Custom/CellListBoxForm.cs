using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;

namespace CustomDataGridViewControls.Custom
{
    public partial class CellListBoxForm : Form
    {
        public Point FormPoint { get; private set; }
        public Size FormSize { get; private set; }
        // ユーザーの入力があったかどうかをbool値で保持
        // ListBox内のItemをDoubleClick、もしくは、Enterで選択した場合のみTrueになる
        public bool UserInputReceived { get; private set; }

        // Hideしたら発火するイベントハンドラ
        public event EventHandler? FormHiding;

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

            this.UserInputReceived = false;

            this.FormPoint = point;
            this.FormSize = size;

            this.masterDataListBox.DisplayMember = displayMember;
            this.masterDataListBox.DataSource = dataSource;
        }

        private void Initialize()
        {
            this.UserInputReceived = false;
            this.FormHiding = null;
        }

        private void ListBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.UserInputReceived = true;
                this.CustomHide();
            }
        }

        private void ListBox_DoubleClick(object sender, EventArgs e)
        {
            this.UserInputReceived = true;
            this.CustomHide();
        }

        private void ListBoxForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            this.CustomHide();
        }

        public DialogItem? GetSelectedItem() => this.masterDataListBox.SelectedItem as DialogItem;

        public void CustomHide()
        {
            if (this.Visible)
            {
                this.Hide();

                this.FormPoint = this.Location;
                this.FormSize = this.Size;

                this.FormHiding?.Invoke(this, EventArgs.Empty);
                this.Initialize();
            }
        }

        public DialogItem? Search(string input)
        {
            List<DialogItem>? items = this.masterDataListBox.DataSource as List<DialogItem>;

            if (int.TryParse(input, out int no))
            {
                // 数値の場合、Noで検索
                var itemByNo = items?.FirstOrDefault(i => i.No == no);
                if (itemByNo != null)
                {
                    return itemByNo;
                }
            }
            else
            {
                // 数値変換負荷の場合、文字列一致で検索
                var itemByData = items?.FirstOrDefault(i => i.Data == input);
                if (itemByData != null)
                {
                    return itemByData;
                }
            }

            return null;
        }

        private void CellListBoxForm_Load(object sender, EventArgs e)
        {
            this.Location = this.FormPoint;
            this.Size = this.FormSize;
        }
    }

    public class DataGridViewListBoxColumn(Point formPoint, Size formSize, string displayMember, List<DialogItem> dataSource, string dataElsePropertyName) : DataGridViewColumn(new DataGridViewListBoxCell())
    {
        public readonly CellListBoxForm CellListBoxForm = new(formPoint, formSize, displayMember, dataSource);
        public readonly string DataElsePropertyName = dataElsePropertyName;
    }

    public class DataGridViewListBoxCell : DataGridViewTextBoxCell
    {
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            // イベントハンドラの重複回避
            DataGridView.CellEndEdit -= this.DataGridView_CellEndEdit;
            DataGridView.CellEndEdit += this.DataGridView_CellEndEdit;
        }
        public override Type EditType => typeof(DataGridViewTextBoxEditingControl);
        public override object DefaultNewRowValue => "";

        /*----- オーバーライドして処理を追加 -----*/
        protected override void OnEnter(int rowIndex, bool throughMouseClick)
        {
            base.OnEnter(rowIndex, throughMouseClick);

            if (OwningColumn is DataGridViewListBoxColumn column)
            {
                // イベントハンドラの重複回避
                column.CellListBoxForm.FormHiding -= this.FormHidingHandler;
                column.CellListBoxForm.FormHiding += this.FormHidingHandler;

                column.CellListBoxForm.Show();
            }
        }
        protected override void OnLeave(int rowIndex, bool throughMouseClick)
        {
            base.OnLeave(rowIndex, throughMouseClick);

            if (OwningColumn is DataGridViewListBoxColumn column)
            {
                column.CellListBoxForm.CustomHide();
            }
        }
        protected override void OnClick(DataGridViewCellEventArgs e)
        {
            base.OnClick(e);

            if (OwningColumn is DataGridViewListBoxColumn column)
            {
                if(!column.CellListBoxForm.Visible)
                {
                    column.CellListBoxForm.FormHiding -= this.FormHidingHandler;
                    column.CellListBoxForm.FormHiding += this.FormHidingHandler;

                    column.CellListBoxForm.Show();
                }
            }
        }
        /*----------*/

        /*----- イベントハンドラの追加 -----*/
        private void DataGridView_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            if (OwningColumn is DataGridViewListBoxColumn column)
            {
                if (Value is object v && v.ToString() is string input)
                {
                    // やっぱりコントロールのTextboxでEnterを押したときも同じ処理をしなければならない

                    // セルの編集状態を終了(ここで終了しておかないと編集状態の時にセルに反映されない)

                    DialogItem? searchedItem = column.CellListBoxForm.Search(input);

                    if (searchedItem is not null)
                    {
                        Value = searchedItem.DispData;

                        this.SetValueBindProperty(column.DataElsePropertyName, searchedItem);
                    }
                    else
                    {
                        Value = "";
                    }

                    DataGridView.NotifyCurrentCellDirty(true);
                }

                column.CellListBoxForm.CustomHide();
            }
        }
        private void FormHidingHandler(object? sender, EventArgs e)
        {
            if (OwningColumn is DataGridViewListBoxColumn column)
            {
                if (column.CellListBoxForm.UserInputReceived
                    && column.CellListBoxForm.GetSelectedItem() is DialogItem dialogItem)
                {
                    // セルの編集状態を終了(ここで終了しておかないと編集状態の時にセルに反映されない)
                    // DataGridView?.EndEdit();

                    Value = dialogItem.Data; // 選択された値をセルに設定

                    this.SetValueBindProperty(column.DataElsePropertyName, dialogItem);

                    DataGridView.NotifyCurrentCellDirty(true);
                }
            }
        }

        private void SetValueBindProperty(string propertyName, DialogItem value)
        {
            if (DataGridView?.CurrentRow.DataBoundItem is object boundItem)
            {
                var propertyInfo = boundItem.GetType().GetProperty(propertyName);

                if (propertyInfo is PropertyInfo info)
                {
                    info.SetValue(boundItem, value);
                }
            }
        }
        /*----------*/
    }
}
