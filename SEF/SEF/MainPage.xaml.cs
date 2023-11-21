using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SEF
{
    public partial class MainPage : ContentPage
    {
        public SpaceEvent spaceEvent;
        public MainPage()
        {
            InitializeComponent();
            spaceEvent = new SpaceEvent();
            Title = "Space Event Finder";
            eventsOptions.ItemsSource = SpaceEvent.EventOptions;
            BindingContext = spaceEvent;
        }

        private void Search_Clicked(object sender, EventArgs e)
        {
               
        }
    }

    public class SpaceEvent: INotifyPropertyChanged
    {
        public static List<string> EventOptions = new List<string>() 
            {
                "Coronal Mass Ejection",
                "Geomagnetic storm",
                "Interplanetary shock",
                "Solar Flare",
                "Solar Energetic Particle",
                "Magnetopause Crossing",
                "Radiation Belt Enhancement",
                "Hight Speed Stream",
            };
        private DateTime startDate;
        public DateTime StartDate {
            get => startDate;
            set 
            { 
                startDate = value; 
                OnPropertyChanged(nameof(StartDate));
            }
        }
        private DateTime endDate;
        public DateTime EndDate
        {
            get => endDate;
            set 
            { 
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
        private string eventOption;

        public event PropertyChangedEventHandler PropertyChanged;

        public SpaceEvent()
        {

        }

        public SpaceEvent(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
            APIKey = string.Empty;
        }

        public string EventOption 
        { 
            get => eventOption;
            set 
            { 
                eventOption = value;
                OnPropertyChanged(nameof(EventOption));
            } 
        }
        private string apiKey;
        public string APIKey
        {
            get => apiKey;
            set
            {
                apiKey = value;
                OnPropertyChanged(nameof(APIKey));
            }
        }
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        
    }
    public class CME //Coronal Mass Ejection
    {
        public static List<CME> AllEvents = new List<CME>();
    }
    public class GMS //Geomagnetic storm
    {
        public static List<GMS> AllEvents = new List<GMS>();
    }
    public class IPS //Interplanetary shock
    {
        public static List<IPS> AllEvents = new List<IPS>();
    }
    public class FLR //Solar Flare
    {
        public static List<FLR> AllEvents = new List<FLR>();
    }
    public class SEP //Solar Energetic Particle
    {
        public static List<SEP> AllEvents = new List<SEP>();
    }
    public class MPC //Magnetopause Crossing
    {
        public static List<MPC> AllEvents = new List<MPC>();
    }
    public class RBE //Radiation Belt Enhancement
    {
        public static List<RBE> AllEvents = new List<RBE>();
    }
    public class HSS //Hight Speed Stream
    {
        public static List<HSS> AllEvents = new List<HSS>();
    }
}
