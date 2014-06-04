using System.Windows;
using System.Windows.Controls;

namespace WorkReport
{
    public partial class LogGrid : Grid
    {
        public LogGrid()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header",
            typeof(object), typeof(LogGrid), new PropertyMetadata(null, new PropertyChangedCallback(HeaderChangeHandler)));

        public static void HeaderChangeHandler(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            LogGrid grid = s as LogGrid;
            grid.headerContent.Content = e.NewValue;
        }


        public static readonly DependencyProperty BodyContentProperty = DependencyProperty.Register("BodyContent",
            typeof(UIElement), typeof(LogGrid), new PropertyMetadata(null, new PropertyChangedCallback(BodyContentChangeHandler)));

        public static void BodyContentChangeHandler(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            LogGrid grid = s as LogGrid;
            grid.bodyContent.Content = e.NewValue as UIElement;
        }

        public object Header
        {
            get
            {
                return GetValue(HeaderProperty);
            }
            set
            {
                SetValue(HeaderProperty, value);
            }
        }

        public UIElement Content
        {
            get
            {
                return GetValue(BodyContentProperty) as UIElement;
            }
            set
            {
                SetValue(BodyContentProperty, value);
            }
        }
    }
}
