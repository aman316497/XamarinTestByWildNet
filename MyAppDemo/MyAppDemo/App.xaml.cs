using MyAppDemo.Services;
using MyAppDemo.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyAppDemo
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            if (Application.Current.Properties.ContainsKey("TimerValue"))
            {

                var timeValue = Convert.ToInt32(Application.Current.Properties["TimerValue"]);
                Console.WriteLine(timeValue);
            }

            MainPage = new NavigationPage(new LockScreenPage());
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
