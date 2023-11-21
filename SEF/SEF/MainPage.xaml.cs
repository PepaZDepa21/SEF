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
        public Filter fil;
        public string filteredEvent;
        public MainPage()
        {
            InitializeComponent();
            fil = new Filter(start: DateTime.Now, end: DateTime.Now);
            Title = "Space Event Finder";
            eventsOptions.ItemsSource = Filter.EventOptions;
            BindingContext = fil;
        }

        private async void Search_Clicked(object sender, EventArgs e)
        {
            if (!fil.IsOK())
            {
                await DisplayAlert("Cannot start search", "Some values are inappropriate", "Ok");
                return;
            }
            filteredEvent = eventsOptions.SelectedItem.ToString();
            if (filteredEvent == "Coronal Mass Ejection")
            {
                SpaceEvent.AllEvents.Add(new CME(-20.0, 120.4, 31.0, 674.0, "C", "F", "CME 1", "16-11-2023", "L"));
                UpdateLW();
            }

        }
        private void Details_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                Navigation.PushAsync(new CMEPage(SpaceEvent.AllEvents.IndexOf((SpaceEvent)button.BindingContext)));
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void UpdateLW()
        {
            EventsLW.ItemsSource = null;
            EventsLW.ItemsSource = SpaceEvent.AllEvents;
        }
        public void ClearEvents()
        {
            SpaceEvent.AllEvents.Clear();
            UpdateLW();
        }
    }

    public class Filter: INotifyPropertyChanged
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

        public Filter() { }

        public Filter(DateTime start, DateTime end)
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
        public bool IsOK() => APIKey != string.Empty && EventOption != string.Empty && DateTime.Compare(StartDate, EndDate) <= 0;
    }
    
    public class SpaceEvent
    {
        public Guid ID { get; set; }
        private string name;
        public string Name
        {
            get => name;
            set => name = value;
        }

        public SpaceEvent() { }
        public SpaceEvent(string eventName, string eventDate, string eventLink)
        {
            ID = Guid.NewGuid();
            Name = eventName;
            Date = eventDate;
            link = eventLink;
        }
        public static List<SpaceEvent> AllEvents = new List<SpaceEvent>();
        private string date;
        public string Date { get => date; set => date = value; }
        private string link;
        public string Link { get => link; set => link = value; }
    }
    
    public class CME : SpaceEvent //Coronal Mass Ejection
    {
        private double latitude;
        public double Latitude { get => latitude; set => latitude = value; }
        private double longitude;
        public double Longitude { get => longitude; set => longitude = value; }
        private double halfAngle;
        public double HalfAngle { get => halfAngle; set => halfAngle = value; }
        private double speed;
        public double Speed { get => speed; set => speed = value; }
        private string type;
        public string Type { get => type; set => type = value; }
        private string note;

        public CME(double CMELatitude, double CMELongitude, double CMEHalfAngle, double CMESpeed, string CMEType, string CMENote, string CMEName, string CMEDate, string CMELink): base(CMEName, CMEDate, CMELink)
        {
            Latitude = CMELatitude;
            Longitude = CMELongitude;
            HalfAngle = CMEHalfAngle;
            Speed = CMESpeed;
            Type = CMEType;
            Note = CMENote;
        }

        public string Note { get => note; set => note = value; }
    }
    public class GMS : SpaceEvent //Geomagnetic storm
    {
        private double kpIndex;
        public double KpIndex { get => kpIndex; set => kpIndex = value; }
        private string source;
        public string Source { get => source; set => source = value; }
        public GMS(double GMSKpIndex, string GMSSource, string GMSName, string GMSDate, string GMSLink): base(GMSName, GMSDate, GMSLink)
        {
            KpIndex = GMSKpIndex;
            Source = GMSSource;
        }

    }
    public class IPS : SpaceEvent //Interplanetary shock
    {
        private string location;
        public string Location { get => location; set => location = value; }
        public IPS(string IPSLocation, string IPSName, string IPSDate, string IPSLink): base(IPSName, IPSDate, IPSLink)
        {
            Location = IPSLocation;
        }
    }
    public class FLR : SpaceEvent //Solar Flare
    {
        private string classType;
        public string ClassType { get => classType; set => classType = value; }
        private string sourceLocation;
        public string SourceLocation { get => sourceLocation; set => sourceLocation = value; }
        private int activeRegionNum;
        public int ActiveRegionNum { get => activeRegionNum; set => activeRegionNum = value; }
        private string instruments;
        public string Instruments { get => instruments; set => instruments = value; }

        public FLR(string FLRClassType, string FLRSourceLocation, int FLRActiveRegionNum, string FLRInstruments, string FLRName, string FLRDate, string FLRLink): base(FLRName, FLRDate, FLRLink)
        {
            ClassType = FLRClassType;
            SourceLocation = FLRSourceLocation;
            ActiveRegionNum = FLRActiveRegionNum;
            Instruments = FLRInstruments;
        }
    }
    public class SEP : SpaceEvent //Solar Energetic Particle
    {
        private string instruments;
        public string Instruments { get => instruments; set => instruments = value; }
        public SEP(string SEPInstruments, string SEPName, string SEPDate, string SEPLink): base(SEPName, SEPDate, SEPLink) 
        {
            Instruments = SEPInstruments;
        }
    }
    public class MPC : SpaceEvent //Magnetopause Crossing
    {
        private string instruments;
        public string Instruments { get => instruments; set => instruments = value; }
        public MPC(string MPCInstruments, string MPCName, string MPCDate, string MPCLink) : base(MPCName, MPCDate, MPCLink)
        {
            Instruments = MPCInstruments;
        }
    }
    public class RBE : SpaceEvent //Radiation Belt Enhancement
    {
        private string instruments;
        public string Instruments { get => instruments; set => instruments = value; }
        public RBE(string RBEInstruments, string RBEName, string RBEDate, string RBELink) : base(RBEName, RBEDate, RBELink)
        {
            Instruments = RBEInstruments;
        }
    }
    public class HSS : SpaceEvent //Hight Speed Stream
    {
        private string instruments;
        public string Instruments { get => instruments; set => instruments = value; }
        public HSS(string HSSInstruments, string HSSName, string HSSDate, string HSSLink) : base(HSSName, HSSDate, HSSLink)
        {
            Instruments = HSSInstruments;
        }
    }
}
