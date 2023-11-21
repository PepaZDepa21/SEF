using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SEF
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FLRPage : ContentPage
    {
        public FLRPage(int index)
        {
            InitializeComponent();
            BindingContext = (FLR)SpaceEvent.AllEvents[index];
        }
    }
}