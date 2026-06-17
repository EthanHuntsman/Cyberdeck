using Cyberdeck.Desktop.ViewModels;
using System.Windows.Controls;

namespace Cyberdeck.Desktop.Views.Pages;

public partial class BrowseCardsPage : Page
{
    public BrowseCardsPage()
    {
        InitializeComponent();

        DataContext = new BrowseCardsViewModel();
    }
}