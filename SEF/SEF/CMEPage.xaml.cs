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
    public partial class CMEPage : ContentPage
    {
        public CMEPage(int index)
        {
            InitializeComponent();
            BindingContext = (CME)SpaceEvent.AllEvents[index];
        }
    }
}