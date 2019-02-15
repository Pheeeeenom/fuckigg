using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "|*.exe";
            DialogResult result = openFileDialog.ShowDialog();
            byte[] zero =  {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

            if (result == DialogResult.OK)
            {
                

                string unityDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                string[] unityCheck = Directory.GetFiles(unityDirectory, "UnityPlayer.dll");
                string[] garbrage = Directory.GetFiles(unityDirectory);
                foreach (string file in garbrage) {
                    if (file.Contains("IGG")) {                     
                        File.Delete(file);
                    }         
                }

                if (unityCheck[0]!= null)
                {
                    string[] gamesList = new string[] { "PMS_Build", "Tube Tycoon" };
                    string[] unityAssets = Directory.GetFiles(unityDirectory, "sharedassets0.assets", SearchOption.AllDirectories);
                    BinaryWriter ubw = new BinaryWriter(File.Open(unityAssets[0], FileMode.Open));
                    foreach (string game in gamesList)
                    {                       
                        switch (game) {
                            case "PMS_Build":
                                if (!openFileDialog.FileName.Contains(game)) break; 
                                ubw.Seek(0x17BC0, SeekOrigin.Begin);
                                ubw.Write(zero);
                                break;
                            case "Tube Tycoon":
                                if (!openFileDialog.FileName.Contains(game)) break;
                                byte[] b = {0x00, 0x00};                                
                                ubw.Seek(0x21A0+8,SeekOrigin.Begin);
                                ubw.Write(b);
                                break;                            
                        }                       
                    }                    
                    ubw.Dispose();
                }

                byte[] magic = { 0x49, 0x47, 0x47, 0x2D };
                byte[] temp = new byte[4];
                

                FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate);
                BinaryWriter bw = new BinaryWriter(fs);
                bool a;
                while (fs.Read(temp, 0, temp.Length) > 0)
                {
                    a = temp.SequenceEqual(magic);
                    if (a)
                    {
                        fs.Seek(fs.Position - 4, SeekOrigin.Begin);

                        bw.Write(zero);
                        break;
                    }

                }
                bw.Flush();
                fs.Flush();
                bw.Close();
                fs.Close();
            }
            MessageBox.Show("Done");

        }
    }
}
