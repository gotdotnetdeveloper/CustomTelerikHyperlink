using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Model;

namespace WpfApp
{
    /// <summary>
    /// Свойство зависимости привязки документа.
    /// </summary>
    public static class RadDocumentBindProperty
    {
        /// <summary>
        /// Документ.
        /// </summary>
        public static readonly DependencyProperty BindDocumentProperty = DependencyProperty.RegisterAttached(
            "BindDocument", typeof(RadDocument), typeof(RadDocumentBindProperty), new PropertyMetadata(null, (o, args) =>
            {
                var ctrl = o as RadRichTextBox;
                if (ctrl != null && args.NewValue != null)
                {
                    ctrl.Document = (RadDocument)args.NewValue;
                }
            }));
        /// <summary>
        /// Установка свойства.
        /// </summary>
        /// <param name="element">Элемент управления.</param>
        /// <param name="value">Значение.</param>
        public static void SetBindDocument(DependencyObject element, RadDocument value)
        {
            element.SetValue(BindDocumentProperty, value);
        }
        /// <summary>
        /// Получение свойства.
        /// </summary>
        /// <param name="element">Элемент управления.</param>
        /// <returns>Значение свойства.</returns>
        public static RadDocument GetBindDocument(DependencyObject element)
        {
            return (RadDocument)element.GetValue(BindDocumentProperty);
        }
    }
}
