using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace SaisieLivre
{
    public partial class ImageForm : Form
    {        
        public ImageForm()
        {
            InitializeComponent();
        }
        
        private void SetPictureBoxes(object sender, EventArgs e)
        {
            int MidWidth = (this.Width / 2);
            pictureBox1.Width = MidWidth - 1;
            pictureBox1.Height = this.Height - 30;
            pictureBox1.Location = new Point(0, 0);

            pictureBox2.Width = MidWidth - 1;
            pictureBox2.Height = this.Height - 30;
            pictureBox2.Location = new Point(MidWidth, 0);
        }


        private void Enregistre_Images_Click(object sender, EventArgs e)
        {
            string ImgType = (sender as Control).Tag.ToString();
            string EAN = TB_Picture_EAN.Text;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "JPEG Image|*.jpg";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sfd.Title = "Enregistrer le visuel";
            sfd.FileName = EAN + "_" + ImgType + "_75.jpg";
            
            if(sfd.ShowDialog(this) == DialogResult.OK)            
            {
                Bitmap bmp = new Bitmap((sender as PictureBox).Image);
                bmp.Save(sfd.FileName, ImageFormat.Jpeg);
                bmp.Dispose();
            }
        }

    }
}
