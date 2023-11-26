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
    public partial class IPSPage : ContentPage
    {
        public IPSPage(int index, string type)
        {
            InitializeComponent();
            BindingContext = (IPS)SpaceEvent.AllEvents[index];
            Title = type;
        }
    }
}