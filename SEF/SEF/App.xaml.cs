using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SEF
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage())
            {
                HeightRequest = 80,
                BarBackgroundColor = Color.FromHex("#011627"),
                BackgroundColor = Color.FromHex("#171D1C"),
                BarTextColor = Color.FromHex("#B8CDF8"),
            };
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
