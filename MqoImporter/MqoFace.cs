namespace MqoImporter
{
    /// <summary>
    /// Mqoオブジェクトの面データ
    /// </summary>
    public class MqoFace
    {
        public int VertexNum = -1;      // 頂点数 2以上の値 フォーマットバージョンが1.0のときは常に4以下
        public int[] Indices;           // 頂点インデックス 0～頂点数-1 （頂点数分の数が存在）
        public int MaterialIndex = -1;  // 材質インデックス -1（未着色面）または0～材質数-1
        public Vector2[] UVs;           // UV値 (頂点数分の数が存在)
        public Color4[] VertexColors;   // 頂点カラー （頂点数分の数が存在） 通常はnull
        public float[] CRS;             // Catmull-Clark/OpenSubdiv曲面用のエッジの折れ目（頂点数分の数が存在）Catmull-Clarkの場合 [0] OFF [1] ON OpenSubdivの場合0以上の値 通常はnull
    }
}
