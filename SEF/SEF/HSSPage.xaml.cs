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
    public partial class HSSPage : ContentPage
    {
        public HSSPage(int index)
        {
            InitializeComponent();
            BindingContext = (HSS)SpaceEvent.AllEvents[index];
        }
    }
}