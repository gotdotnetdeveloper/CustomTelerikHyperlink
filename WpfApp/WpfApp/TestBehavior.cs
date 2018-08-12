using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Model;

namespace WpfApp
{
    
    public class TestBehavior : Behavior<RadRichTextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.HyperlinkClicked += OnHyperlinkClicked;
        }

        private void OnHyperlinkClicked(object sender, HyperlinkClickedEventArgs e)
        {

        }

        protected override void OnDetaching()
        {
            AssociatedObject.HyperlinkClicked -= OnHyperlinkClicked;
        }

       
    }
}
