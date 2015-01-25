namespace MqoImporter
{
    /// <summary>
    /// MQOモデルデータ
    /// </summary>
    public class MqoModel
    {
        public string Filepath;             // ファイルのフルパス
        public float Version;               // バージョン [1.0] 多角形をサポートせず4角以下 [1.1] 5角以上の多角形をサポート (Ver4.0以降）
        public MqoMaterial[] Materials;     // マテリアルデータリスト
        public MqoObject[] Objects;         // オブジェクトリスト
    }
}
