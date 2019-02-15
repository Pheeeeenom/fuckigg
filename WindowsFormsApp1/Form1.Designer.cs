namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btn = new System.Windows.Forms.Button();
            this.btnTEXToPNG = new System.Windows.Forms.Button();
            this.btnPNGtoTEX = new System.Windows.Forms.Button();
            this.btnRepac39gk = new System.Windows.Forms.Button();
            this.btnUnpack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(284, 259);
            this.btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(115, 50);
            this.btn.TabIndex = 0;
            this.btn.Text = "STERILIZE";
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnTEXToPNG
            // 
            this.btnTEXToPNG.Location = new System.Drawing.Point(13, 257);
            this.btnTEXToPNG.Margin = new System.Windows.Forms.Padding(4);
            this.btnTEXToPNG.Name = "btnTEXToPNG";
            this.btnTEXToPNG.Size = new System.Drawing.Size(119, 50);
            this.btnTEXToPNG.TabIndex = 1;
            this.btnTEXToPNG.Text = ".TEX to .PNG";
            this.btnTEXToPNG.UseVisualStyleBackColor = true;
            this.btnTEXToPNG.Click += new System.EventHandler(this.btnTEXToPNG_Click);
            // 
            // btnPNGtoTEX
            // 
            this.btnPNGtoTEX.Location = new System.Drawing.Point(13, 199);
            this.btnPNGtoTEX.Margin = new System.Windows.Forms.Padding(4);
            this.btnPNGtoTEX.Name = "btnPNGtoTEX";
            this.btnPNGtoTEX.Size = new System.Drawing.Size(119, 50);
            this.btnPNGtoTEX.TabIndex = 2;
            this.btnPNGtoTEX.Text = ".PNG to .Tex";
            this.btnPNGtoTEX.UseVisualStyleBackColor = true;
            this.btnPNGtoTEX.Click += new System.EventHandler(this.btnPNGtoTEX_Click);
            // 
            // btnRepac39gk
            // 
            this.btnRepac39gk.Location = new System.Drawing.Point(13, 138);
            this.btnRepac39gk.Name = "btnRepac39gk";
            this.btnRepac39gk.Size = new System.Drawing.Size(119, 54);
            this.btnRepac39gk.TabIndex = 3;
            this.btnRepac39gk.Text = "Repack";
            this.btnRepac39gk.UseVisualStyleBackColor = true;
            // 
            // btnUnpack
            // 
            this.btnUnpack.Location = new System.Drawing.Point(12, 78);
            this.btnUnpack.Name = "btnUnpack";
            this.btnUnpack.Size = new System.Drawing.Size(119, 54);
            this.btnUnpack.TabIndex = 4;
            this.btnUnpack.Text = "Unpack";
            this.btnUnpack.UseVisualStyleBackColor = true;
            this.btnUnpack.Click += new System.EventHandler(this.btnUnpack_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(411, 320);
            this.Controls.Add(this.btnUnpack);
            this.Controls.Add(this.btnRepac39gk);
            this.Controls.Add(this.btnPNGtoTEX);
            this.Controls.Add(this.btnTEXToPNG);
            this.Controls.Add(this.btn);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "STERILIZE ME";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.Button btnTEXToPNG;
        private System.Windows.Forms.Button btnPNGtoTEX;
        private System.Windows.Forms.Button btnRepac39gk;
        private System.Windows.Forms.Button btnUnpack;
    }
}

