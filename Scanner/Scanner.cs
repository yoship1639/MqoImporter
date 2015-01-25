using System.Globalization;

namespace Scanner
{
    /// <summary>
    /// テキストからデータを読み取るクラス
    /// </summary>
    public class Scanner
    {
        /// <summary>
        /// スキャンするテキスト
        /// </summary>
        public string Text { get { return text; } }

        /// <summary>
        /// スキャン位置
        /// </summary>
        public int Seek
        {
            get { return seek; }
            set { seek = value; if (seek < 0) seek = 0; }
        }
        private int seek = 0;

        /// <summary>
        /// 前回のスキャン位置
        /// </summary>
        public int PrevSeek { get { return prevSeek; } }
        private int prevSeek = 0;

        /// <summary>
        /// スキャン位置が既にテキストの終わりか
        /// </summary>
        public bool IsEnd { get { return Seek >= text.Length; } }

        private string text;

        public Scanner(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// 次の文字列を読み取る
        /// </summary>
        /// <returns>文字列</returns>
        public string NextString()
        {
            prevSeek = seek;
            // WhiteSpaceを飛ばす
            while (true)
            {
                if (IsEnd) return null;
                if (char.IsWhiteSpace(text[seek])) seek++;
                else break;
            }

            // テキストの読み取り
            int end = seek;
            while (true)
            {
                if (end == text.Length || char.IsWhiteSpace(text[end]))
                {
                    string str = text.Substring(seek, end - seek);
                    seek = end;
                    return str;
                }
                else end++;
            }
        }

        /// <summary>
        /// 次の整数値を読み取る
        /// </summary>
        /// <returns>次の整数値</returns>
        public int NextInt()
        {
            return int.Parse(NextString());
        }

        /// <summary>
        /// 次の浮動小数値を読み取る
        /// </summary>
        /// <returns>次の浮動少数値</returns>
        public float NextFloat()
        {
            return float.Parse(NextString().Replace(',', '.'), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 次の倍精度小数値を読み取る
        /// </summary>
        /// <returns>次の倍精度小数値</returns>
        public double NextDouble()
        {
            return double.Parse(NextString().Replace(',', '.'), CultureInfo.InvariantCulture);
        }
    }
}
