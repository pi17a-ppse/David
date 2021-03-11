eusing System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace NotepadForum
{
    public partial class Notepad : Form
    {
        public Notepad()
        {
            InitializeComponent();
        }

        string OpenedFileName = string.Empty;

        private void MenuOpen_Click(object sender, EventArgs e)
        {
            statusLeft.Text = "Opening...";
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы|*.*";
            opf.RestoreDirectory = true;
            if (opf.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader input = new StreamReader(opf.FileName, GetActiveEncode()))
                {
                    TextArea.Text = input.ReadToEnd();
                    OpenedFileName = Path.GetFileName(opf.FileName);
                }
            }
            statusLeft.Text = "Ready";
            statusRight.Text = "Saved";
        }

        Encoding GetActiveEncode()
        {
            foreach (ToolStripMenuItem item in MenuEncoding.DropDownItems)
            {
                if (item.Checked)
                {
                    switch (item.Name)
                    {
                        case "MenuSelectANSI":
                            return Encoding.ASCII;
                        case "MenuSelectUnicode":
                            return Encoding.Unicode;
                        case "MenuSelectUTF":
                            return Encoding.UTF8;
                        default:
                            return Encoding.Default;
                    }
                }
            }
            return Encoding.Default;
        }
        void MenuEncodeUncheck()
        {
            MenuSelectDefault.Checked = false;
            MenuSelectANSI.Checked = false;
            MenuSelectUnicode.Checked = false;
            MenuSelectUTF.Checked = false;
        }

        private void MenuSave_Click(object sender, EventArgs e)
        {
            statusLeft.Text = "Saving...";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы|*.*";
            sfd.FileName = OpenedFileName;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string SaveFilePath = sfd.FileName;
                if (Path.GetExtension(SaveFilePath).Length == 0) SaveFilePath += ".txt";
                if (File.Exists(SaveFilePath))
                {
                    if (MessageBox.Show(string.Format("Файл с именем {0} уже существует.\n\n Заменить?", Path.GetFileName(SaveFilePath)), "Замена файла", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                    {
                        return;
                    }
                }
                using(Stream stream = File.Create(SaveFilePath))
                using (StreamWriter output = new StreamWriter(stream, GetActiveEncode()))
                {
                    output.Write(TextArea.Text);
                }
            }
            statusLeft.Text = "Ready";
            statusRight.Text = "Saved";
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MenuSelectDefault_Click(object sender, EventArgs e)
        {
            MenuEncodeUncheck();
            ((ToolStripMenuItem)sender).Checked = true;
        }

        private void MenuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Версия программы: 1.0 Выполнил ст. гр. ПИ-17А Дидур Давид");
        }

        private void TextArea_TextChanged(object sender, EventArgs e)
        {
            if (statusRight.Text != "Modified") statusRight.Text = "Modified";
        }
    }
}
