using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Command;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Model;

namespace WpfApp
{

    /// <summary>
    /// Расширение функциональности RadRichTextBox. Кастомная обработка клика по гиперссылки в документе.
    /// </summary>
    public class RadRichTextBoxBehavior : Behavior<RadRichTextBox>
    {
        /// <summary>
        /// DependencyProperty клика по гиперлинку.
        /// </summary>
        public static readonly DependencyProperty HyperlinkActionProperty =
            DependencyProperty.RegisterAttached("HyperlinkAction", typeof(ICommand), typeof(RadRichTextBoxBehavior),
                new UIPropertyMetadata(null, null));


        /// <summary>
        /// Команда, которая обрабатывает клик по гиперссылки
        /// </summary>
        public ICommand HyperlinkAction
        {
            get => (ICommand)GetValue(HyperlinkActionProperty);
            set => SetValue(HyperlinkActionProperty, value);
        }

        /// <summary>
        /// Подключение расширенной функциональности для RadRichTextBox.
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.HyperlinkClicked += OnHyperlinkClicked;
        }

        /// <summary>
        /// Отключение расширенной функциональности для RadRichTextBox.
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.HyperlinkClicked -= OnHyperlinkClicked;
        }

        /// <summary>
        /// Обработчик клика по гиперссылки. Запускает команду, перехватывает событие.
        /// </summary>
        private void OnHyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
        {
            if (GetValue(HyperlinkActionProperty) is ICommand comm)
            {
                if (comm.CanExecute(e.URL))
                {
                    comm.Execute(e.URL);
                    e.Handled = true;
                }
            }
        }
    }
}
