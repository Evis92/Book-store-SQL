using System.Configuration;
using System.Data;
using System.Windows;
using DataAccess;

namespace BookStoreUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ApplicationContextManager.Initialize(new BookStoreDbContext());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ApplicationContextManager.Context.Dispose();
        }
    }

}
