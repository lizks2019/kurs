using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageEditor
{
    public partial class MDIParent : Form
    {
        public MDIParent()
        {
            InitializeComponent();
        }
        
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)  //Открытие изображения
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                FormImageEditor childForm = new FormImageEditor();
                childForm.MdiParent = this;
                childForm.OpenFileClick(openFileDialog.FileName);

                childForm.Text = openFileDialog.FileName;

                childForm.Show();
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)    //Сохранить
        {
            //openFileDialog.Dispose();
            //saveFileDialog.Dispose();
            //MessageBox.Show("Очистка 2", "Вывод", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (MdiChildren.Length == 0)
            {
                MessageBox.Show("Не создано ни одного изображения!", "Ошибка");
                return;
            }

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                FormImageEditor childForm = (FormImageEditor)this.ActiveMdiChild;
                if (childForm.SaveImage(saveFileDialog.FileName))
                {
                    childForm.Text = saveFileDialog.FileName;
                }
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)   //Выход из программы
        {
            this.Close();
        }
        
        private void fileMenu_Click(object sender, EventArgs e)
        {

        }

        private void MDIParent_Load(object sender, EventArgs e)
        {

        }

        private void AboutOfProgramToolStripMenuItem_Click(object sender, EventArgs e)   //О программе
        {
            MessageBox.Show("Автор: Соболева Е.П.");
        }

        // Расположения дочерних окон:
        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void VerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void HorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void WindowListToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // Создание нового изображения из выделенного фрагмента
        private void NewFromFragmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormImageEditor childForm = (FormImageEditor)this.ActiveMdiChild;
            if (childForm != null)
            {
                if (childForm.GetFragm())
                {
                    FormImageEditor childForm2 = new FormImageEditor();
                    childForm2.MdiParent = this;
                    childForm2.Text = "Новое изображение";

                    childForm2.Show();

                    childForm2.Fragment = childForm.Fragment;
                    childForm2.PasteFragment();
                }
            } else {
                MessageBox.Show("Не открыто ни одного изображения!", "Ошибка");
            }
        }
    }
}