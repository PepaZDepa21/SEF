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
    public partial class MPCPage : ContentPage
    {
        public MPCPage(int index, string type)
        {
            InitializeComponent();
            BindingContext = (MPC)SpaceEvent.AllEvents[index];
            Title = type;
        }
    }
}