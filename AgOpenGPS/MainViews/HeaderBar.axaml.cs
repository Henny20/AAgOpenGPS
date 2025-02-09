using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AgOpenGPS.MainViews
{
    public partial class HeaderBar : UserControl
    {
        public HeaderBar()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
