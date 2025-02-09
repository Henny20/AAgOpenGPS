using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AgOpenGPS.ViewportViews
{
    public partial class SimulatorBar : UserControl
    {
        public SimulatorBar()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
