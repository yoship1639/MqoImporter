using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MqoImporter
{
    /// <summary>
    /// MQOモデルデータ
    /// </summary>
    public class MqoModel
    {

        public MqoMaterial[] Materials;     // マテリアルデータリスト
        public MqoObject[] Objects;         // オブジェクトリスト
    }
}
