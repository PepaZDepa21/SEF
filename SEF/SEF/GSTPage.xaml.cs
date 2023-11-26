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
    public partial class GSTPage : ContentPage
    {
        public GSTPage(int index, string type)
        {
            InitializeComponent();
            GST gst = (GST)SpaceEvent.AllEvents[index];
            BindingContext = gst;
            LwKpIndex.ItemsSource = gst.KpIndexes;
            Title = type;
        }
    }
}