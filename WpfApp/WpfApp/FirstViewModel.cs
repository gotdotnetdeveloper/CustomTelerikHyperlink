using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Docx;
using Telerik.Windows.Documents.Layout;
using Telerik.Windows.Documents.Model;
using ViewModelBase = GalaSoft.MvvmLight.ViewModelBase;

namespace WpfApp
{
    class FirstViewModel : ViewModelBase
    {
        private RadDocument _documentData;
        private RelayCommand _ExportCommand;
        private RelayCommand _importCommand;
        private RelayCommand _insertLinkCommand;

        #region Save
        public RelayCommand SaveCommand => _ExportCommand ?? (_ExportCommand = new RelayCommand(ExportDoumentsCommandExecute));
        private void ExportDoumentsCommandExecute()
        {
            SaveDocument();
        }
        #endregion

        #region Load
        public RelayCommand LoadCommand => _importCommand ?? (_importCommand = new RelayCommand(ImportCommandExecute));
        private void ImportCommandExecute()
        {
            LoaderDocument();
        }
        #endregion

        #region InsertLinkCommand
        public RelayCommand InsertLinkCommand => _insertLinkCommand ?? (_insertLinkCommand = new RelayCommand(InsertLinkCommandExecute));
        private void InsertLinkCommandExecute()
        {
            InsertLink();
        }



        #endregion

   

        private void InsertLink()
        {
            var range = DocumentData.Selection.Ranges.First;
            if (range != null)
            {
                HyperlinkInfo info = new HyperlinkInfo()
                {
                    NavigateUri = "https://demos.telerik.com/silverlight/#RichTextBox/TelerikEditor",
                    Target = HyperlinkTargets.Blank,
                    IsAnchor = false,
                };
          
                //[Obsolete("Use RadDocumentEditor.InsertHyperlink method instead.")]
                //DocumentPosition startPosition, DocumentPosition endPosition, HyperlinkInfo hyperlinkInfo
            var newLink =    DocumentData.InsertHyperlink(range.StartPosition, range.EndPosition ,info);

            }
        }


        private void f()
        {
            HyperlinkRangeStart hyperlinkStart = new HyperlinkRangeStart();
            HyperlinkRangeEnd hyperlinkEnd = new HyperlinkRangeEnd();
            hyperlinkEnd.PairWithStart(hyperlinkStart);
            HyperlinkInfo hyperlinkInfo = new HyperlinkInfo() { NavigateUri = "http://telerik.com", Target = HyperlinkTargets.Blank };
            hyperlinkStart.HyperlinkInfo = hyperlinkInfo;
            RadDocument document = new RadDocument();
            Section section = new Section();
            Paragraph paragraph = new Paragraph();
            Span spanBefore = new Span("Text before the image ");
            ImageInline image = new ImageInline(new Uri("/Telerik.Windows.Controls.RichTextBoxUI;component/Images/MSOffice/32/Picture.png", UriKind.Relative));
            image.Size = new Size(32, 32);
            Span spanAfter = new Span(" and some text after the image");
            paragraph.Inlines.Add(hyperlinkStart);
            paragraph.Inlines.Add(spanBefore);
            paragraph.Inlines.Add(image);
            paragraph.Inlines.Add(spanAfter);
            section.Blocks.Add(paragraph);
            Paragraph anotherParagraph = new Paragraph();
            anotherParagraph.Inlines.Add(new Span("Another paragraph here and still in hyperlink"));
            anotherParagraph.Inlines.Add(hyperlinkEnd);
            section.Blocks.Add(anotherParagraph);
            document.Sections.Add(section);
          //  this.richTextBox.Document = document;

        }


        private void LoaderDocument()
        {
            DocxFormatProvider provider = new DocxFormatProvider();
            string fileName = "";
            RadOpenFileDialog openFileDialog = new RadOpenFileDialog();
            openFileDialog.ShowDialog();
            if (openFileDialog.DialogResult == true)
            {
                fileName = openFileDialog.FileName;

                using (FileStream inputStream = File.OpenRead(fileName))
                {
                    DocumentData = provider.Import(inputStream);
                }
            }
        }

        private void SaveDocument()
        {
            DocxFormatProvider provider = new DocxFormatProvider();
            string fileName = "";
            RadSaveFileDialog saveFileDialog = new RadSaveFileDialog
            {
                Filter = "Рубрикатор (*.docx)|*.docx" +
                         "|Все файлы (*.*)|*.*",
                FilterIndex = 1,
            };

            saveFileDialog.ShowDialog();
            if (saveFileDialog.DialogResult == true)
            {
                fileName = saveFileDialog.FileName;

                var data = provider.Export(DocumentData);
                File.WriteAllBytes(fileName, data);
            }
        }


        /// <summary>
        /// Содержимое документа.
        /// </summary>
        public RadDocument DocumentData
        {
            get => _documentData;
            set
            {
                _documentData = value;
                RaisePropertyChanged();
            }
        }
    }
}
