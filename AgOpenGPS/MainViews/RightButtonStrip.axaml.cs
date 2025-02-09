using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AgOpenGPS.MainViews
{
    public partial class RightButtonStrip : UserControl
    {
        public RightButtonStrip()
        {
            InitializeComponent();
        }
        
         private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
