using System.Windows;
using System.Windows.Controls;

namespace WorkReport
{
    public partial class BuildingQuality : UserControl
    {
        public BuildingQuality()
        {
            InitializeComponent();
        }
        public static DependencyProperty QuilityProperty = DependencyProperty.Register(
              "Quility", typeof(string), typeof(BuildingQuality), new PropertyMetadata("", new PropertyChangedCallback(QuilityHandler)));

        public static void QuilityHandler(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            BuildingQuality quility = s as BuildingQuality;
            quility.buildingQC.Text = e.NewValue.ToString();
        }

        public string Quility
        {
            get
            {
                return GetValue(QuilityProperty).ToString();
            }
            set
            {
                SetValue(QuilityProperty, value);
            }
        }
    }
}
