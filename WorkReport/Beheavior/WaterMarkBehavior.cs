using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WorkReport.Beheavior
{
    public class WaterMarkBehavior : Behavior<TextBox>
    {
        private string _tagTxt;

        protected override void OnAttached()
        {
            base.OnAttached();
            this._tagTxt = this.AssociatedObject.Tag.ToString();
            this.AssociatedObject.SelectionChanged += this.AssociatedObject_SelectionChanged;
            this.AssociatedObject.GotFocus += this.AssociatedObject_GotFocus;
            this.AssociatedObject.LostFocus += this.AssociatedObject_LostFocus;
        }

        private void AssociatedObject_SelectionChanged(object sender, RoutedEventArgs e)
        {
            this.AssociatedObject.Tag = "";
        }

        private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.AssociatedObject.Text))
            {
                this.AssociatedObject.Text = this._tagTxt;
            }
        }

        private void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.AssociatedObject.Text == this._tagTxt)
            {
                this.AssociatedObject.Text = "";
            }
        }
    }
}
