using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyAppDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LockScreenPage : ContentPage
    {
        LockScreenPageViewModel viewModel;
        public LockScreenPage()
        {
            InitializeComponent();
           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing(); 
            viewModel = new LockScreenPageViewModel(this.Navigation);
            BindingContext = viewModel;

        }

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    viewModel.StartStopTimer(false);

        //}
    }
    internal class LockScreenPageViewModel : INotifyPropertyChanged
    {
        private INavigation _navigation;
        public int Timer { get; set; } //seconds
        private Thread th;
        public LockScreenPageViewModel(INavigation navigation)
        {
            Timer = 10;
            th = new Thread(onStart);
            th.Start();
            _navigation = navigation;
        }

        [Obsolete]
        async public Task StartStopTimer(bool flag)
        {
            try
            {

                DateTime dt = DateTime.Now;
                if (!flag)
                {
                    if(Application.Current.Properties.ContainsKey("DateTimeRecord"))
                    Application.Current.Properties.Add("DateTimeRecord", dt);
                    else
                        Application.Current.Properties["DateTimeRecord"] = dt;

                    if (Application.Current.Properties.ContainsKey("TimerValue"))
                        Application.Current.Properties.Add("TimerValue", Timer);
                    else
                        Application.Current.Properties["TimerValue"] = Timer;

                    await Application.Current.SavePropertiesAsync();
                    th.Abort();
                }
                else
                {

                    //if (Application.Current.Properties.ContainsKey("DateTimeRecord") &&
                    //Application.Current.Properties.ContainsKey("TimerValue"))
                    //{
                    //    var previousDt = Convert.ToDateTime(Application.Current.Properties["DateTimeRecord"]);
                    //    var timerValue = Convert.ToInt32(Application.Current.Properties["TimerValue"]);
                    //    var timeDiff = (dt - previousDt).TotalSeconds;
                    //    if (timeDiff < timerValue)
                    //    {
                    //        Timer = timerValue - (int)timeDiff;
                    //        th.Start();
                    //    }
                    //    else
                    //        Timer = 0;
                    //}
                    //else
                        Timer = 10;
                    th.Start();
                    OnPropertyChanged(nameof(Timer));
                }
            }catch(Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        async void onStart()
        {
            try
            {
                while(true)
                {
                    
                    if (Timer == 0)
                    {
                        
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await _navigation.PushAsync(new Dashboard(),true);
                        });
                        th.Abort();
                        break;
                    }
                    else
                    {
                        Timer = Timer - 1;
                        OnPropertyChanged(nameof(Timer));
                    }

                    await Task.Delay(1000);
                }

            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void OnPropertyChanged(string invokeName = null)
        {
            try
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(invokeName));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}