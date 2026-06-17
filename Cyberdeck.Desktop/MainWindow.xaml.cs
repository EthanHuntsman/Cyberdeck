using Cyberdeck.Desktop.ViewModels;
using Cyberdeck.Desktop.Views.Pages;
using System.Windows;

namespace Cyberdeck.Desktop;

public partial class MainWindow : Window
{
    public MainWindow(DeckBuilderPage deckBuilderPage)
    {
        InitializeComponent();

        MainFrame.Navigate(deckBuilderPage);
    }
}