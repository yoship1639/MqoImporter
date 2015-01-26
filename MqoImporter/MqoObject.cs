namespace MqoImporter
{
    /// <summary>
    /// Mqoモデルのオブジェクトデータ
    /// </summary>
    public class MqoObject
    {
        public string Name;         // 名前

        public int UID = -1;        // プラグインで利用されるユニークID。指定されない場合、自動的に連番が割り当てられる。
        public int Depth = -1;      // 階層の深さ　ルート直下を0として深くなるごとに+1
        public int Folding = -1;    // オブジェクトパネル上の階層の折りたたみ [0] 通常表示 [1] 子オブジェクトを折りたたんで非表示に
        public Vector3 Scale;       // ローカル座標の拡大率 XYZ
        public Vector3 Rotation;    // ローカル座標の回転角 HPB
        public Vector3 Translation; // ローカル座標の平行移動量 XYZ
        public int Patch = -1;      // 曲面の形式 [0] 平面(曲面指定をしない) [1] 曲面タイプ１ （スプライン Type1） [2] 曲面タイプ２ （スプライン Type2） [3] Catmull-Clark （Ver2.2以降） [4] OpenSubdiv （Ver4.0以降）
        public int PatchTri = -1;   // Catmull-Clark曲面の三角形面の処理 [0] 四角形に分割 [1] 三角形のまま分割
        public int Segment = -1;    // 曲面の分割数 1～16 (Catmull-Clark/OpenSubdivの場合、再帰分割数を示すため1～4となる）
        public int Visible = -1;    // 表示・非表示 [0] 非表示 [15] 表示
        public int Locking = -1;    // オブジェクトの固定 [0] 編集可能 [1] 編集禁止
        public int Shading = -1;    // シェーディング [0] フラットシェーディング [1] グローシェーディング
        public float Facet = -1;    // スムージング角度 0～180
        public Color3 Color;        // 色（ＲＧＢ）それぞれ0～1
        public int ColorType = -1;  // 辺の色タイプ [0] 環境設定での色を使用 [1] オブジェクト固有の色を使用
        public int Mirror = -1;     // 鏡面のタイプ [0] なし [1] 左右を分離 [2] 左右を接続
        public int MirrorAxis = -1; // 鏡面の適用軸 [1] X軸 [2] Y軸 [4] Z軸
        public float MirrorDis = -1;// 接続距離 0～
        public int Lathe = -1;      // 回転体のタイプ [0] なし [3] 両面
        public int LatheAxis = -1;  // 回転体の軸 [0] X軸 [1] Y軸 [2] Z軸
        public int LatheSeg = -1;   // 回転体の分割数 3～
        public Vector3[] Vertices;  // 頂点データリスト
        public MqoFace[] Faces;     // 面リスト
    }
}
