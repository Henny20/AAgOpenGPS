using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AgOpenGPS.MainViews
{
    public partial class LeftButtonStrip : UserControl
    {
        public LeftButtonStrip()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
