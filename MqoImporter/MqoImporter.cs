﻿using System;
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
        private static readonly string Error_NotFoundMaterial = "マテリアルチャンクが見つかりません.ファイルが破損している可能性があります.";
        private static readonly string Error_NotFoundObject = "オブジェクトチャンクが見つかりません.ファイルが破損している可能性があります.";

        /// <summary>
        /// ファイルからMQOモデルのデータを読み込み、それを返す
        /// </summary>
        /// <param name="filepath">ファイルのフルパス</param>
        /// <returns>MQOモデルデータ</returns>
        public static MqoImportResult Import(string filepath)
        {
            // テキストをすべて読み取る
            string text = File.ReadAllText(filepath, Encoding.GetEncoding("Shift-JIS"));

            // スキャナの作成
            Scanner scanner = new Scanner(text);

            // マジックナンバーの読み取り
            if (scanner.NextString() != "Metasequoia") return Error(Error_MagicNumber, scanner);
            if (scanner.NextString() != "Document") return Error(Error_MagicNumber, scanner);
            if (scanner.NextString() != "Format") return Error(Error_MagicNumber, scanner);
            if (scanner.NextString() != "Text") return Error(Error_MagicNumber, scanner);
            if (scanner.NextString() != "Ver") return Error(Error_MagicNumber, scanner);
            
            // バージョンの読み取り
            float version = scanner.NextFloat();


            /// <summary>
            /// マテリアルの読み取り
            /// </summary>
            while (scanner.NextString() != "Material" && !scanner.IsEnd) ;
            if (scanner.IsEnd) return Error(Error_NotFoundMaterial, scanner);

            // マテリアル数
            int materialNum = scanner.NextInt();
            MqoMaterial[] materials = new MqoMaterial[materialNum];
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = new MqoMaterial() { Index = i };
            }

            // チャンクの開始
            if (scanner.NextString() != "{") return Error(Error_NotFoundMaterial, scanner);

            foreach (var m in materials)
            {
                // 1行取得
                string line = scanner.NextString("\r\n");

                Scanner s = new Scanner(line);
                // 名前の取得
                string name = s.NextString();
                //m.Name = encode(name.Substring(1, name.Length - 2), Encoding.GetEncoding("Shift-JIS"), Encoding.UTF8);
                m.Name = name.Substring(1, name.Length - 2);

                while (!s.IsEnd)
                {
                    string str = s.NextString(")"); // 項目の取得
                    if (str == null) continue;

                    if (str.IndexOf("shader") != -1) // シェーダ
                    {
                        m.Shader = int.Parse(str.Substring(7, 1));
                    }
                    else if (str.IndexOf("vcol") != -1) // 頂点カラー
                    {
                        m.VCol = int.Parse(str.Substring(5, 1));
                    }
                    else if (str.IndexOf("dbls") != -1) // 両面表示
                    {
                        m.Doubles = int.Parse(str.Substring(5, 1));
                    }
                    else if (str.IndexOf("col") != -1) // 色（ＲＧＢ）、不透明度
                    {
                        Scanner cols = new Scanner(str.Substring(4, str.Length - 5));
                        m.Color.R = cols.NextFloat();
                        m.Color.G = cols.NextFloat();
                        m.Color.B = cols.NextFloat();
                        m.Color.A = cols.NextFloat();
                    }
                    else if (str.IndexOf("dif") != -1) // 拡散光
                    {
                        Scanner difs = new Scanner(str.Substring(4, str.Length - 5));
                        m.Diffuse = difs.NextFloat();
                    }
                    else if (str.IndexOf("amb") != -1) // 周囲光
                    {
                        Scanner ambs = new Scanner(str.Substring(4, str.Length - 5));
                        m.Ambient = ambs.NextFloat();
                    }
                    else if (str.IndexOf("emi") != -1) // 自己照明
                    {
                        Scanner emis = new Scanner(str.Substring(4, str.Length - 5));
                        m.Emissive = emis.NextFloat();
                    }
                    else if (str.IndexOf("spc") != -1) // 反射光
                    {
                        Scanner spes = new Scanner(str.Substring(4, str.Length - 5));
                        m.Specular = spes.NextFloat();
                    }
                    else if (str.IndexOf("power") != -1) // 反射光の強さ
                    {
                        Scanner ps = new Scanner(str.Substring(6, str.Length - 7));
                        m.SpecularPower = ps.NextFloat();
                    }
                    else if (str.IndexOf("reflect") != -1) // 鏡面反射 （Ver4.0以降)
                    {
                        Scanner rs = new Scanner(str.Substring(8, str.Length - 9));
                        m.Reflect = rs.NextFloat();
                    }
                    else if (str.IndexOf("refract") != -1) // 屈折率 （Ver4.0以降)
                    {
                        Scanner rs = new Scanner(str.Substring(8, str.Length - 9));
                        m.Refract = rs.NextFloat();
                    }
                    else if (str.IndexOf("tex") != -1) // 模様マッピング名
                    {
                        string path = str.Substring(5, str.Length - 7);
                        if (File.Exists(path)) m.TextureFullpath = path;
                        else
                        {
                            string dir = Directory.GetParent(filepath).FullName;
                            string fp = dir + "\\" + path;
                            if (File.Exists(fp)) m.TextureFullpath = fp;
                        }
                    }
                    else if (str.IndexOf("aplane") != -1) // 透明マッピング名
                    {
                        string path = str.Substring(8, str.Length - 10);
                        if (File.Exists(path)) m.AlphaMapFullpath = path;
                        else
                        {
                            string dir = Directory.GetParent(filepath).FullName;
                            string fp = dir + "\\" + path;
                            if (File.Exists(fp)) m.AlphaMapFullpath = fp;
                        }
                    }
                    else if (str.IndexOf("bump") != -1) // 凹凸マッピング名
                    {
                        string path = str.Substring(6, str.Length - 8);
                        if (File.Exists(path)) m.BumpMapFullpath = path;
                        else
                        {
                            string dir = Directory.GetParent(filepath).FullName;
                            string fp = dir + "\\" + path;
                            if (File.Exists(fp)) m.BumpMapFullpath = fp;
                        }
                    }
                    else if (str.IndexOf("proj_type") != -1) // マッピング方式
                    {
                        Scanner ps = new Scanner(str.Substring(10, str.Length - 11));
                        m.ProjectType = ps.NextInt();
                    }
                    else if (str.IndexOf("proj_pos") != -1) // 投影位置（ＸＹＺ）
                    {
                        Scanner ps = new Scanner(str.Substring(9, str.Length - 10));
                        m.ProjectPos.X = ps.NextFloat();
                        m.ProjectPos.Y = ps.NextFloat();
                        m.ProjectPos.Z = ps.NextFloat();
                    }
                    else if (str.IndexOf("proj_scale") != -1) // 投影拡大率（ＸＹＺ）
                    {
                        Scanner ps = new Scanner(str.Substring(11, str.Length - 12));
                        m.ProjectScale.X = ps.NextFloat();
                        m.ProjectScale.Y = ps.NextFloat();
                        m.ProjectScale.Z = ps.NextFloat();
                    }
                    else if (str.IndexOf("proj_angle") != -1) // 投影角度（ＨＰＢ）
                    {
                        Scanner ps = new Scanner(str.Substring(11, str.Length - 12));
                        m.ProjectAngle.X = ps.NextFloat();
                        m.ProjectAngle.Y = ps.NextFloat();
                        m.ProjectAngle.Z = ps.NextFloat();
                    }
                }
            }

            /// <summary>
            /// マテリアルの読み取り
            /// </summary>
            List<MqoObject> objectList = new List<MqoObject>();
            bool eof = false;
            while (true)
            {
                while (true)
                {
                    string str = scanner.NextString();
                    if (str == "Object") // オブジェクトチャンクを見つけたら
                    {
                        break;
                    }
                    else if (str == "Eof") // ファイルの最後なら
                    {
                        eof = true;
                        break;
                    }
                }
                if (eof) break;

                MqoObject obj = new MqoObject();

                // オブジェクトの名前
                obj.Name = scanner.NextString();
                obj.Name = obj.Name.Substring(1, obj.Name.Length - 2);

                // チャンクの開始
                if (scanner.NextString() != "{") return Error(Error_NotFoundObject, scanner);

                while (true)
                {
                    string str = scanner.NextString(); // 項目の取得
                    bool breakFlag = false;

                    switch (str)
                    {
                        case "uid":
                            obj.UID = scanner.NextInt();
                            break;

                        case "depth":
                            obj.Depth = scanner.NextInt();
                            break;

                        case "folding":
                            obj.Folding = scanner.NextInt();
                            break;

                        case "scale":
                            obj.Scale.X = scanner.NextFloat();
                            obj.Scale.Y = scanner.NextFloat();
                            obj.Scale.Z = scanner.NextFloat();
                            break;

                        case "rotation":
                            obj.Rotation.X = scanner.NextFloat();
                            obj.Rotation.Y = scanner.NextFloat();
                            obj.Rotation.Z = scanner.NextFloat();
                            break;

                        case "translation":
                            obj.Translation.X = scanner.NextFloat();
                            obj.Translation.Y = scanner.NextFloat();
                            obj.Translation.Z = scanner.NextFloat();
                            break;

                        case "patch":
                            obj.Patch = scanner.NextInt();
                            break;

                        case "patchtri":
                            obj.PatchTri = scanner.NextInt();
                            break;

                        case "segment":
                            obj.Segment = scanner.NextInt();
                            break;

                        case "visible":
                            obj.Visible = scanner.NextInt();
                            break;

                        case "locking":
                            obj.Locking = scanner.NextInt();
                            break;

                        case "shading":
                            obj.Shading = scanner.NextInt();
                            break;

                        case "facet":
                            obj.Facet = scanner.NextFloat();
                            break;

                        case "color":
                            obj.Color.R = scanner.NextFloat();
                            obj.Color.G = scanner.NextFloat();
                            obj.Color.B = scanner.NextFloat();
                            break;

                        case "color_type":
                            obj.ColorType = scanner.NextInt();
                            break;

                        case "mirror":
                            obj.Mirror = scanner.NextInt();
                            break;

                        case "mirror_axis":
                            obj.MirrorAxis = scanner.NextInt();
                            break;

                        case "mirror_dis":
                            obj.MirrorDis = scanner.NextFloat();
                            break;

                        case "lathe":
                            obj.Lathe = scanner.NextInt();
                            break;

                        case "lathe_axis":
                            obj.LatheAxis = scanner.NextInt();
                            break;

                        case "lathe_seg":
                            obj.LatheSeg = scanner.NextInt();
                            break;

                        case "vertex":
                            int vertexNum = scanner.NextInt(); // 頂点数
                            obj.Vertices = new Vector3[vertexNum];
                            scanner.NextString();
                            for (int i = 0; i < vertexNum; i++)
                            {
                                obj.Vertices[i].X = scanner.NextFloat();
                                obj.Vertices[i].Y = scanner.NextFloat();
                                obj.Vertices[i].Z = scanner.NextFloat();
                            }
                            scanner.NextString();
                            break;

                        case "BVertex":
                            break;

                        case "face":
                            int faceNum = scanner.NextInt(); // 面数
                            obj.Faces = new MqoFace[faceNum];
                            scanner.NextString();
                            for (int i = 0; i < faceNum; i++)
                            {
                                obj.Faces[i] = new MqoFace();
                                MqoFace face = obj.Faces[i];

                                string line = scanner.NextString("\r\n"); // 1行取り出し
                                Scanner s = new Scanner(line);

                                face.VertexNum = s.NextInt(); // 面の頂点数

                                while (!s.IsEnd)
                                {
                                    string fs = s.NextString(")");
                                    if (str == null) continue;

                                    if (fs.IndexOf("V") != -1)
                                    {
                                        string sub = fs.Substring(2, fs.Length - 3);
                                        Scanner vs = new Scanner(sub);
                                        face.Indices = new int[face.VertexNum];
                                        for (int j = 0; j < face.VertexNum; j++) face.Indices[j] = vs.NextInt();
                                    }
                                    else if (fs.IndexOf("M") != -1)
                                    {
                                        Scanner ms = new Scanner(fs.Substring(2, fs.Length - 3));
                                        face.MaterialIndex = ms.NextInt();
                                    }
                                    else if (fs.IndexOf("UV") != -1)
                                    {
                                        Scanner uvs = new Scanner(fs.Substring(3, fs.Length - 4));
                                        face.UVs =new Vector2[face.VertexNum];
                                        for (int j = 0; j < face.VertexNum; j++)
                                        {
                                            face.UVs[j].X = uvs.NextFloat();
                                            face.UVs[j].Y = uvs.NextFloat();
                                        } 
                                        
                                    }
                                    else if (fs.IndexOf("COL") != -1)
                                    {
                                        
                                    }
                                    else if (fs.IndexOf("CRS") != -1)
                                    { 
                                        
                                    }
                                }
                            }

                            breakFlag = true;
                            break;

                        default:
                            breakFlag = true;
                            break;
                    }

                    if (breakFlag) break;
                }

                objectList.Add(obj);
            }

            return new MqoImportResult()
            {
                Succeeded = true,
                Model = new MqoModel()
                {
                    Filepath = filepath.ToString(),
                    Version = version,
                    Materials = materials,
                },
            };
            
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
            int seek = scanner.PrevSeek;
            int line = 0;
            int c = 0;
            using (StringReader reader = new StringReader(scanner.Text))
            {
                while (true)
                {

                    string str = reader.ReadLine();
                    if (seek < str.Length)
                    {
                        c = seek;
                        break;
                    }
                    else
                    {
                        seek -= str.Length + 2;
                        line++;
                    }
                }
            }

            // 読み取り失敗の結果を返す
            return new MqoImportResult()
            {
                ErrorMessage = err,
                Succeeded = false,
                ErrorString = scanner.ScanString,
                ErrorLine = line,
                ErrorCharactor = c,
            };
        }

        private static string encode(string str, Encoding src, Encoding dst)
        {
            byte[] resultBytes = src.GetBytes(str.ToCharArray());
            return dst.GetString(resultBytes);

        }
    }

    /// <summary>
    /// MQOモデルの読み込み結果
    /// </summary>
    public class MqoImportResult
    {
        public bool Succeeded;              // 成功したらtrue
        public string ErrorMessage;         // エラーメッセージ
        public int ErrorLine = -1;          // 読み取りに失敗した行
        public int ErrorCharactor = -1;     // 読み取りに失敗した文字数
        public string ErrorString;          // 読み取りに失敗した文字列
        public MqoModel Model;              // 読み取ったMQOモデルデータ
    }
}
