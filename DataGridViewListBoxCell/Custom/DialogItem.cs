namespace CustomDataGridViewControls.Custom
{
    public class DialogItem(short no, string data)
    {
        private short _no = no;
        private string _data = data;
        private string _dispData = $"{no} : {data}";

        public short No
        {
            get => _no;
            set => _no = value;
        }

        public string Data
        {
            get => _data;
            set => _data = value;
        }

        public string DispData
        {
            get => _dispData;
            set => _dispData = value;
        }
    }

    public static class DialogItemGenerator
    {
        private static Random _random = new Random();

        public static List<DialogItem> GenerateRandomDialogItems(int count = 20)
        {
            var dialogItems = new List<DialogItem>();
            for (short i = 0; i < count; i++)
            {
                // ランダムな文字列を生成
                string randomData = $"Item {i + 1} - {GenerateRandomString(5)}";
                dialogItems.Add(new DialogItem(i, randomData));
            }
            return dialogItems;
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[_random.Next(chars.Length)];
            }
            return new string(stringChars);
        }
    }
}
