using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Interactivity;
using Avalonia.Controls.Platform;

namespace AAgOpenGPS.Pages;

public partial class NVSamplePage1 : UserControl
{
    public NVSamplePage1()
    {
        InitializeComponent();
    }
}

public class EmbedGL : NativeControlHost
{
    public static INativeDemoControl? Implementation { get; set; }

    static EmbedGL()
    {

    }

    //public bool IsSecond { get; set; }

    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        return Implementation?.CreateControl(/**IsSecond,**/ parent, () => base.CreateNativeControlCore(parent))
            ?? base.CreateNativeControlCore(parent);
    }

    protected override void DestroyNativeControlCore(IPlatformHandle control)
    {
        base.DestroyNativeControlCore(control);
    }
}

public interface INativeDemoControl
{
    /// <param name="isSecond">Used to specify which control should be displayed as a demo</param>
    /// <param name="parent"></param>
    /// <param name="createDefault"></param>
    IPlatformHandle CreateControl(/**bool isSecond,**/ IPlatformHandle parent, Func<IPlatformHandle> createDefault);
}
