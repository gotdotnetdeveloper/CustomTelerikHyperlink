using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using GalaSoft.MvvmLight.Command;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Docx;
using Telerik.Windows.Documents.Layout;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Model.Styles;
using ViewModelBase = GalaSoft.MvvmLight.ViewModelBase;

namespace WpfApp
{
    class FirstViewModel : ViewModelBase
    {
        #region private proprty
        private RadDocument _documentData = new RadDocument();
        private RelayCommand _ExportCommand;
        private RelayCommand _importCommand;
        private RelayCommand _insertLinkCommand;
        private RelayCommand<string> _OpenLinkCommand;
        #endregion

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
            var newContext = new OpenLinkContext();
            newContext.Id = Guid.NewGuid();
            newContext.Type = 2;
            newContext.Text = "Тестовый ''❸''  '№;%:?*(@#$%^&*0.;'текст";

          

            InsertLink(newContext);
        }



        #endregion

       #region OpenLinkCommand
        public RelayCommand<string> OpenLinkCommand => _OpenLinkCommand ?? (_OpenLinkCommand = new RelayCommand<string>(OpenLinkCommandExecute));

        private void OpenLinkCommandExecute(string obj)
        {
            if (obj != null)
            {
                var o = GetDeserializedObject<OpenLinkContext>(obj);
                MessageBox.Show(o.Text);
            }
                
        }

        #endregion


        #region private method

        private void InsertLink(OpenLinkContext newContext)
        {
            var txt = GetSerializedObject(newContext);
            var range = DocumentData.Selection.Ranges.First;
            if (range != null)
            {
                HyperlinkRangeStart hyperlinkStart = new HyperlinkRangeStart();
                HyperlinkRangeEnd hyperlinkEnd = new HyperlinkRangeEnd();
                hyperlinkEnd.PairWithStart(hyperlinkStart);
                HyperlinkInfo hyperlinkInfo = new HyperlinkInfo()
                {
                    NavigateUri = txt,
                    Target = HyperlinkTargets.Self,
                    IsAnchor = true
                };
                hyperlinkStart.HyperlinkInfo = hyperlinkInfo;
                RadDocumentEditor documentEditor = new RadDocumentEditor(DocumentData);
                documentEditor.InsertHyperlink(hyperlinkInfo);
            }
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
                Filter = "Document (*.docx)|*.docx" +
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
        /// Преобразование объекта в XML представление.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="obj">Экземпляр объекта.</param>
        /// <returns>Сериализованное представление.</returns>
        private string GetSerializedObject<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, obj);
                return textWriter.ToString();
            }
        }
        /// <summary>
        /// Преобразование строки XML в объект.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlText"></param>
        /// <returns></returns>
        private T GetDeserializedObject<T>(string xmlText)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            using (StringReader textReader = new StringReader(xmlText))
            {
                var result = deserializer.Deserialize(textReader);
                return (T)result;
            }
        }
        #endregion
    }
}
