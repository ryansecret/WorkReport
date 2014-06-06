using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WorkReport
{
    public partial class OrderList : UserControl
    {
        public OrderList()
        {
            InitializeComponent();
        }

        private void gotoReport(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "ShowReport", true);
        }

        private void showList(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "ShowList", true);
        }
    }
}
