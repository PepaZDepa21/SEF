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
    public partial class SEPPage : ContentPage
    {
        public SEPPage(int index, string type)
        {
            InitializeComponent();
            BindingContext = (SEP)SpaceEvent.AllEvents[index];
            Title = type;
        }
    }
}