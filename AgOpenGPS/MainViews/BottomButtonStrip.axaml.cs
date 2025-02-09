using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AgOpenGPS.MainViews
{
    public partial class BottomButtonStrip : UserControl
    {
        public BottomButtonStrip()
        {
            InitializeComponent();
        }
        //most recent versions don't need this
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
