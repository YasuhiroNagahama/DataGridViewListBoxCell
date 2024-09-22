namespace CustomDataGridViewControls
{ 
    internal class DialogItem(short no, string data)
    {
        private short _no = no;
        private string _data = data;
        private string _dispData = $"{no} : {data}";

        public short No 
        {
            get => this._no;
            set => this._no = value;
        }

        public string Data
        {
            get => this._data;
            set => this._data = value;
        }

        public string DispData
        {
            get => this._dispData;
            set => this._dispData = value;
        }
    }
}
