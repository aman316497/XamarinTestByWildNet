using MyAppDemo.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MyAppDemo.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}