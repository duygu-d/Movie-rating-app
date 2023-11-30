using CineRankApp.View;
using CineRankApp.ViewModel;

namespace CineRankApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}