using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MqoImporter
{
    /// <summary>
    /// MQOモデルを読み取るクラス
    /// </summary>
    public class MqoImporter
    {
        private static readonly string Error_MagicNumber = "マジックナンバーが正しくありません.MQOファイルで無い可能性があります.";

        /// <summary>
        /// ファイルからMQOモデルのデータを読み込み、それを返す
        /// </summary>
        /// <param name="filepath">ファイルのフルパス</param>
        /// <returns>MQOモデルデータ</returns>
        public static MqoImportResult Import(string filepath)
        {
            // テキストをすべて読み取る
            string text = File.ReadAllText(filepath);

            // スキャナの作成
            Scanner scanner = new Scanner(text);

            // マジックナンバーの読み取り
            if (scanner.NextString() != "Metasequoia") return Error(Error_MagicNumber, scanner);
            if (scanner.NextString() != "Document") return Error(Error_MagicNumber, scanner);
        }

        /// <summary>
        /// MQOモデルの読み取りに失敗したときの結果を返す
        /// </summary>
        /// <param name="err">エラーメッセージ</param>
        /// <param name="scanner">スキャナ</param>
        /// <returns>読み取り失敗の結果</returns>
        private static MqoImportResult Error(string err, Scanner scanner)
        {
            // エラーが発生した場所を特定
            int line = 0;
            int c = 0;


            return new MqoImportResult() { ErrorMessage = err, Succeeded = false, };
        }
    }

    /// <summary>
    /// MQOモデルの読み込み結果
    /// </summary>
    public class MqoImportResult
    {
        public bool Succeeded;
        public string ErrorMessage;
        public int ErrorLine = -1;
        public int ErrorCharactor = -1;
        public string ErrorString;
        public MqoModel Model;
    }
}
