using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Interactivity;
using Avalonia.Controls.Platform;

using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using AAgOpenGPS.Pages;

namespace AAgOpenGPS.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        //  var nv = this.FindControl<NavigationView>("nvSample1");
        nv.SelectionChanged += OnNVSample1SelectionChanged;
        nv.SelectedItem = nv.MenuItems.ElementAt(0);

    }
    /*****
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
       *******/
    private void OnNVSample1SelectionChanged(object sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (e.IsSettingsSelected)
        {
            (sender as NavigationView).Content = new PageSettings();
        }
        else if (e.SelectedItem is NavigationViewItem nvi)
        {
            var smpPage = $"AAgOpenGPS.Pages.NV{nvi.Tag}";
            var pg = Activator.CreateInstance(Type.GetType(smpPage));
            (sender as NavigationView).Content = pg;
        }
    }


}


