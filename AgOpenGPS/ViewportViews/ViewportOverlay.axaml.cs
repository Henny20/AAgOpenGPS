using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AgOpenGPS.ViewportViews
{
    public partial class ViewportOverlay : UserControl
    {
        public ViewportOverlay()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
