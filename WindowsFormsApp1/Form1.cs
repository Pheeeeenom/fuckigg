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

            if (result == DialogResult.OK)
            {
                
                byte[] magic = { 0x49, 0x47, 0x47, 0x2D };
                byte[] temp = new byte[4];
                byte[] zero =  {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

                FileStream fs = new FileStream(openFileDialog.FileName,FileMode.OpenOrCreate);
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
