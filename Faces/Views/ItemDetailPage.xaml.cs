using System.ComponentModel;
using Xamarin.Forms;
using Faces.ViewModels;

namespace Faces.Views
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