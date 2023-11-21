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
    public partial class RBEPage : ContentPage
    {
        public RBEPage(int index)
        {
            InitializeComponent();
            BindingContext = (RBE)SpaceEvent.AllEvents[index];
        }
    }
}