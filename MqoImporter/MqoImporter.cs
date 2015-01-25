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
            if (scanner.NextString() != "Metasequoia") return Error(Error_MagicNumber, );
            if (scanner.NextString() != "Document") return Error(Error_MagicNumber);
        }

        private static MqoImportResult Error(string err, int seek)
        {
            return new MqoImportResult() { ErrorMessage = err, Succeeded = false; };
        }
    }

    /// <summary>
    /// MQOモデルの読み込み結果
    /// </summary>
    public class MqoImportResult
    {
        public bool Succeeded;
        public string ErrorMessage;
        public MqoModel Model;
    }
}
