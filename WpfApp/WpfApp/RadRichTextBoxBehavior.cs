using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Documents.Model;

namespace WpfApp
{
 
    /// <summary>
    /// Расширяет поведение грида.
    /// Добавляет DefaultAction - реакция на Double Click. 
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
        /// <param name="grid"></param>
        /// <param name="value"></param>
        public static void SetDoubleClickAction(RadRichTextBox grid, ICommand value)
        {
            grid.SetValue(DoubleClickActionProperty, value);
        }

        private static void OnDoubleClickActionChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            if (!(depObj is RadRichTextBox grid)) return;

            if (!(e.NewValue is ICommand))
            {
                grid.HyperlinkClicked  -= OnHyperlinkClicked;
            }
            else if (!(e.OldValue is ICommand))
            {
                grid.HyperlinkClicked += OnHyperlinkClicked;
            }
        }

        private static void OnHyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
        {
           // e.URL
        }

        private static bool PerformDoubleClickAction(RadRichTextBox grid, string url)
        {
            var cmd = GetDoubleClickAction(grid);
            if (cmd == null || !cmd.CanExecute(null)) return false;

           // cmd.Execute(grid?.SelectedItem);

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
    }
}
