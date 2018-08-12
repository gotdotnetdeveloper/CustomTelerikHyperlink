using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Documents.Model;

namespace WpfApp
{
    /// <summary>
    /// Расширяет поведение RadRichTextBox.
    /// </summary>
    public static class RadRichTextBoxBehavior
    {
        /// <summary>
        /// Команда на двойное нажатие.
        /// </summary>
        public static readonly DependencyProperty DoubleClickActionProperty =
            DependencyProperty.RegisterAttached("DoubleClickAction", typeof(ICommand), typeof(RadRichTextBoxBehavior),
                new UIPropertyMetadata(null, OnDoubleClickActionChanged));

       
        /// <summary>
        /// Возвращает DefaultAction для грида.
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static ICommand GetDoubleClickAction(RadRichTextBox grid)
        {
            return grid.GetValue(DoubleClickActionProperty) as ICommand;
        }

        /// <summary>
        /// Устанавливает DefaultAction для грида.
        /// </summary>
        public static void SetDoubleClickAction(RadRichTextBox grid, ICommand value)
        {
            grid.SetValue(DoubleClickActionProperty, value);
        }

        private static void OnDoubleClickActionChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            if (!(depObj is RadRichTextBox grid)) return;

            if (!(e.NewValue is ICommand))
            {
                grid. HyperlinkClicked  -= OnHyperlinkClicked;
            }
            else if (!(e.OldValue is ICommand))
            {
                grid.HyperlinkClicked += OnHyperlinkClicked;
            }
        }

        private static void OnHyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
        {
            var s = sender as Telerik.Windows.Documents.Model.Span;
            var parent = ((Telerik.Windows.Documents.Model.DocumentElement) sender).Parent.Parent.Parent as Telerik.Windows.Documents.Model.RadDocument;


            //Telerik.Windows.Documents.Model.Span
            if (sender is RadRichTextBox grid && PerformDoubleClickAction(grid, e.URL))
                e.Handled = true;
        }

        private static bool PerformDoubleClickAction(RadRichTextBox grid, string url)
        {
            var cmd = GetDoubleClickAction(grid);
            if (cmd == null || !cmd.CanExecute(url)) return false;

            cmd.Execute(url);

            return true;
        }


        //private static void OnGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    // Для начала нужно проверить, что это левая кнопка, а потом уже все остальное
        //    if (e.ChangedButton != MouseButton.Left) return;
        //    var clickedElement = (DependencyObject)e.OriginalSource;
        //    while (clickedElement is FrameworkContentElement)
        //        clickedElement = ((FrameworkContentElement)clickedElement).Parent;

        //    // Проверяем, что кликали на строке грида
        //    if (clickedElement.ParentOfType<GridViewRow>() == null) return;
        //    if (sender is RadRichTextBox grid && PerformDoubleClickAction(grid)) e.Handled = true;
        //}


        ///// <summary>
        ///// Преобразование объекта в XML представление.
        ///// </summary>
        ///// <typeparam name="T">Тип объекта.</typeparam>
        ///// <param name="obj">Экземпляр объекта.</param>
        ///// <returns>Сериализованное представление.</returns>
        //private static string GetSerializedObject<T>(T obj)
        //{
        //    XmlSerializer serializer = new XmlSerializer(typeof(T));
        //    using (StringWriter textWriter = new StringWriter())
        //    {
        //        serializer.Serialize(textWriter, obj);
        //        return textWriter.ToString();
        //    }
        //}
    }
}
