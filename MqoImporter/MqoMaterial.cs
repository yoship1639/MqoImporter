using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MqoImporter
{
    /// <summary>
    /// MQOモデルのマテリアルデータ
    /// </summary>
    public class MqoMaterial
    {
        public string Name;             // 名前
        public int index;               // 材質インデックス

        public int Shader;              // シェーダ [0] Classic [1] Constant [2] Lambert [3] Phong [4] Blinn
        public int VCol;                // 頂点カラー [0] なし [1] あり
        public bool Doubles;            // 両面表示 [0] 片面 [1] 両面
        public Color4 Color;            // 色（ＲＧＢ）、不透明度
        public float Diffuse;           // 拡散光 (0.0f ~ 1.0f)
        public float Ambient;           // 周囲光 (0.0f ~ 1.0f)
        public float Emissive;          // 自己照明 (0.0f ~ 1.0f)
        public float Specular;          // 反射光 (0.0f ~ 1.0f)
        public float SpecularPower;     // 反射光の強さ (0.0f ~ 100.0f)
        public float Reflect;           // 鏡面反射 (Ver4.0以降) (0.0f ~ 1.0f)
        public float Refract;           // 屈折率 (Ver4.0以降) (1.0f ~ 5.0f)

        public string TextureFullpath;  // 模様マップのフルパス
        public string AlphaMapFullpath; // 透明マップのフルパス
        public string BumpMapFullpath;  // バンプマップのフルパス

        public int Mapping;             // マッピング方式 [0] UV [1] 平面 [2] 円筒 [3] 球
        public Vector3 ProjectPos;      // 投影位置
        public Vector3 ProjectScale;    // 投影拡大率
        public Vector3 ProjectAngle;    // 投影角度 (-180 ~ 180)
    }

}
