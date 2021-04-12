using System;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace SubtitleLanguageSwapper
{
    public partial class Form1 : Form
    {
        private OpenFileDialog openFileDialog1 = new OpenFileDialog();
        private string text;
        string file;

        public Form1()
        {
            InitializeComponent();
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                file = openFileDialog1.FileName;
                try
                {
                    text = File.ReadAllText(file);
                    richTextBox1.AppendText(text);
                }
                catch (IOException)
                {
                }
            }
        }

        private void convertBtn_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();

            StringReader input = new StringReader(text);
            string result = "";

            // matches a timestamp line with a "->" and no alpha characters
            Regex timestampRegex = new Regex(@"[^A-Za-z]*-\s*>[^A-Za-z]*");

            string line;
            while ((line = input.ReadLine()) != null)
            {
                // if a timestamp line is found then it is modified
                if (timestampRegex.IsMatch(line))
                {
                    line = Regex.Replace(line, @"\s", ""); // remove all whitespace
                    line = line.Replace("-", ""); // remove excessive hyphens
                    line = line.Replace(">", " --> "); // update arrow style
                    line = line.Replace(".", ","); // update floating point format
                }

                result += (line + "\n");
            }

            richTextBox1.AppendText(result);
            StreamWriter streamWriter = File.CreateText(file);
            streamWriter.Write(result);
        }

        private void quitBtn_Click(object sender, EventArgs e)
        {
            if (Application.MessageLoop)
            {
                // WinForms app
                Application.Exit();
            }
            else
            {
                // Console app
                Environment.Exit(1);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
