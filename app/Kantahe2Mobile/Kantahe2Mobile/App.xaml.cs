using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Kantahe2Mobile.Services;
using Kantahe2Mobile.Views;

namespace Kantahe2Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
