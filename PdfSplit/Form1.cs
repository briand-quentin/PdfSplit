using System;
using System.IO;
using System.Windows.Forms;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PdfSplit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
            errorProvider1.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var files = openFileDialog1.FileName;
            if (openFileDialog1.CheckFileExists && openFileDialog1.CheckPathExists && !files.Equals(""))
            {
                SplitPdf();
            }
            else
            {
                errorProvider1.SetError(textBox1,"Nom de fichier invalide");
                
            }
        }

        private void SplitPdf()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var filename = openFileDialog1.SafeFileName;

            var newDirectory = Directory.GetCurrentDirectory() + "/Facture" + DateTime.Today.ToString("yyyyMMdd");

            Directory.CreateDirectory(newDirectory);
                

            File.Copy(openFileDialog1.FileName, Path.Combine(newDirectory, filename), true);

            //Path.Combine("../../../../../PDFs/", filename)
            var inputDocument = PdfReader.Open(Path.Combine(newDirectory,filename), PdfDocumentOpenMode.Import);

            var name = Path.GetFileNameWithoutExtension(filename);

            progressBar1.Maximum = inputDocument.PageCount;
            progressBar1.Step = 1;

            for (var idx = 0; idx < inputDocument.PageCount; idx++)
            {
                var outputDocument = new PdfDocument();

                outputDocument.Version = inputDocument.Version;

                outputDocument.Info.Title = string.Format("Page {0} of {1}", idx + 1, inputDocument.Info.Title);

                outputDocument.Info.Creator = inputDocument.Info.Creator;


                // Add the page and save it

                outputDocument.AddPage(inputDocument.Pages[idx]);

                outputDocument.Save(Path.Combine(newDirectory, string.Format("{0} - Page {1}_tempfile.pdf", name, idx + 1)));

                progressBar1.PerformStep();
                progressBar1.Text = string.Format("Page {0}/{1}",idx,inputDocument.PageCount);
            }

            progressBar1.Value = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter =
                "Fichiers pdf (*.pdf)|*.pdf";

            openFileDialog1.FileName = "";
            openFileDialog1.Multiselect = false;
            openFileDialog1.CheckFileExists = true;
        }
    }
}