using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ImageEditor
{
    public partial class FormImageEditor : Form
    {
        Bitmap bmp;

        public Point mousePos1;
        public Point mousePos2;
        public Bitmap Fragment;
        int x1 = 0;
        int x2 = 0;
        int y1 = 0;
        int y2 = 0;

        public FormImageEditor()
        {
            InitializeComponent();
        }
        
        public bool SaveImage(string FileName)   //Сохранение изображения в файл
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Save(FileName, System.Drawing.Imaging.ImageFormat.Bmp);

                return true;

            } else {
                MessageBox.Show("Для начала создайте изображение!", "Ошибка!");
                return false;
            }
        }

        public void OpenFileClick(string FileName)   // Открытие изображения
        {
            Bitmap tbmp = new Bitmap(FileName);
            bmp = new Bitmap(tbmp.Width, tbmp.Height);
            
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(tbmp, new Point(0, 0));
            g.Dispose();
            tbmp.Dispose();
            
            pictureBox1.Image = bmp;
            
            this.Text = Path.GetFileNameWithoutExtension(FileName);

            SaveImage(FileName);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePos2 = e.Location;
                x2 = e.Location.X;
                y2 = e.Location.Y;
                pictureBox1.Invalidate();
            }
            else
            {
                mousePos1 = mousePos2 = e.Location;
                //x1 = x2 = e.Location.X;
                //y1 = y2 = e.Location.Y;
            }
        }

        // Сбросить выделение
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            x1 = 0;
            x2 = 0;
            y1 = 0;
            y2 = 0;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawFocusRectangle(e.Graphics, new Rectangle(x1, y1, x2 - x1, y2 - y1)); //рисуем рамку
            
            if (mousePos1 != mousePos2)
            {
                ControlPaint.DrawFocusRectangle(e.Graphics, GetRect(mousePos1, mousePos2)); //рисуем рамку

                x1 = Math.Min(mousePos1.X, mousePos2.X);
                x2 = Math.Max(mousePos1.X, mousePos2.X);
                y1 = Math.Min(mousePos1.Y, mousePos2.Y);
                y2 = Math.Max(mousePos1.Y, mousePos2.Y);
            }
            
        }

        //получение Rectangle из двух точек
        Rectangle GetRect(Point p1, Point p2)
        {
            var x1 = Math.Min(p1.X, p2.X);
            var x2 = Math.Max(p1.X, p2.X);
            var y1 = Math.Min(p1.Y, p2.Y);
            var y2 = Math.Max(p1.Y, p2.Y);
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        public bool GetFragm()
        {
            if (pictureBox1.Image != null)
            {
                Color pixel;
                bmp = new Bitmap(pictureBox1.Image);
                Fragment = new Bitmap(pictureBox1.Image);

                if ((x1 < bmp.Width) & (x2 < bmp.Width) & (y1 < bmp.Height) & (y2 < bmp.Height))
                {
                    if ((x1 != x2) & (y1 != y2))
                    {
                        int xxx = -1;
                        int yyy = -1;
                        for (int x = x1; x < x2; x++)
                        {
                            xxx = xxx + 1;
                            for (int y = y1; y < y2; y++)
                            {
                                yyy = yyy + 1;
                                pixel = bmp.GetPixel(x, y);
                                Fragment.SetPixel(xxx, yyy, pixel);
                            }
                            yyy = -1;
                        }

                        Fragment = Fragment.Clone(new Rectangle(0, 0, x2 - x1, y2 - y1), bmp.PixelFormat);

                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Фрагмент не выделен!", "Ошибка!");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Фрагмент вышел за границы изображения!", "Ошибка!");
                    return false;
                }
            } else {
                return false;
            }
        }

        public void PasteFragment()
        {
            pictureBox1.Image = Fragment;
        }

        private void FormTableEditor_Load(object sender, EventArgs e)
        {

        }
    }
}
