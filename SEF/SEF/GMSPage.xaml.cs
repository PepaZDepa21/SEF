using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SEF
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GMSPage : ContentPage
    {
        public GMSPage(int index)
        {
            InitializeComponent();
            BindingContext = (GMS)SpaceEvent.AllEvents[index];
        }
    }
}