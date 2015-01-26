using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MqoImporter;

namespace ImportTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("読み込み開始");
            
            //MqoImportResult res = MqoImporter.MqoImporter.Import(@"C:\Users\yoship\Downloads\モデリング、モーション\モデル\BeamModels\ビームマンモデルセット\ビームマン式初音ミク\BeamMikuVer3.mqo");
            //MqoImportResult res = MqoImporter.MqoImporter.Import(@"C:\Users\yoship\Downloads\モデリング、モーション\モデル\mh20100930_MQO_PracticeKit_keynote\robomiku_keynote.mqo");
            MqoImportResult res = MqoImporter.MqoImporter.Import(@"C:\Users\yoship\Downloads\モデリング、モーション\モデル\EandI\E&I\Ivory.mqo");
            Console.WriteLine("読み込み終了");
            Console.ReadKey();
        }
    }
}
