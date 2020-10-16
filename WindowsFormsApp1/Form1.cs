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
using Newtonsoft.Json;
using fuckIGG;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            //assign offset and patches to ints

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "|*.exe"
            };

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string dir = Path.GetDirectoryName(openFileDialog.FileName);

                foreach (string file in Array.FindAll(Directory.GetFiles(dir), x => x.ContainsIgnoreCase("igg") || x.ContainsIgnoreCase("gamestorrent.co")))
                {
                    File.Delete(file);
                }

                string f = File.ReadAllText("fuckyou.json");
                dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(f);

                string i = txtName.Text;
                //prints json entry data to console

                using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        string game_exec = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf("\\") + 1, openFileDialog.FileName.Length - openFileDialog.FileName.LastIndexOf("\\") - 1);

                        if (jsonObj[i]["Exec_name"] == game_exec)
                        {
                            int offset = Convert.ToInt32(jsonObj[i]["Offset"].ToString(), 16);
                            int patches = Convert.ToInt32(jsonObj[i]["Patches"].ToString(), 16);

                            /*set position of stream to offset to apply patches*/
                            bw.BaseStream.Position = offset;

                            //write patch to file
                            bw.Write((byte)patches);
                        }
                    }
                }
            }
        }
    }
}