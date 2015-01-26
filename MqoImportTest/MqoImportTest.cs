using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MqoImporter;
using System.IO;

namespace MqoImportTest
{
    public partial class MqoImportTest : Form
    {
        public MqoImportTest()
        {
            InitializeComponent();
            textBox2.AppendText("ファイルパスを指定してください\n");
            textBox2.AppendText("\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MQOファイル(*.mqo;)|*.mqo|すべてのファイル|*.*";
            ofd.Title = "MQOファイルを選択してください";

            if (ofd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox1.Text))
            {
                textBox2.AppendText("指定のファイルは存在しません!\n");
                return;
            }

            // MQO読み込み
            textBox2.AppendText(textBox1.Text + "を読み込んでいます...\n");
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            MqoImportResult result = null;
            sw.Start();
            try
            {
                result = MqoModelImporter.Import(textBox1.Text);
            }
            catch (Exception ex)
            {
                textBox2.AppendText(ex.Message);
                return;
            }
            sw.Stop();

            if (!result.Succeeded)
            {
                textBox2.AppendText(result.ErrorMessage + "\n");
                textBox2.AppendText("エラーが発生した行:" + result.ErrorLine + "\n");
                textBox2.AppendText("エラーが発生した文字数:" + result.ErrorCharactor + "\n");
                textBox2.AppendText("エラーが発生した文字:" + result.ErrorString + "\n");
            }
            else
            {
                textBox2.AppendText("読み込み成功!\n");
                textBox2.AppendText("読み込みにかかった時間:" + sw.ElapsedMilliseconds + "ms\n");

                textBox2.AppendText("バージョン:" + result.Model.Version + "\n");
                textBox2.AppendText("マテリアル数:" + result.Model.Materials.Length + "\n");
                textBox2.AppendText("オブジェクト数:" + result.Model.Objects.Length + "\n");
            }
            textBox2.AppendText("\r\n");
        }
    }
}
