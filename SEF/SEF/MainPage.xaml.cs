using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using static SEF.Filter;
using System.Diagnostics;
using System.IO;

namespace SEF
{
    public partial class MainPage : ContentPage
    {
        public Filter fil;
        public string filteredEvent;
        public static Dictionary<string, ShowPage> SelectPageToShow = new Dictionary<string, ShowPage>(){ };
        public MainPage()
        {
            InitializeComponent();
            SelectPageToShow.Add("Coronal Mass Ejection", ShowCMEPage);
            SelectPageToShow.Add("Geomagnetic storm", ShowGSTPage);
            SelectPageToShow.Add("Interplanetary shock", ShowIPSPage);
            SelectPageToShow.Add("Solar Flare", ShowFLRPage);
            SelectPageToShow.Add("Solar Energetic Particle", ShowSEPPage);
            SelectPageToShow.Add("Magnetopause Crossing", ShowMPCPage);
            SelectPageToShow.Add("Radiation Belt Enhancement", ShowRBEPage);
            SelectPageToShow.Add("Hight Speed Stream", ShowHSSPage);
            fil = new Filter(start: DateTime.Now, end: DateTime.Now);
            Title = "Space Event Finder";
            eventsOptions.ItemsSource = Filter.GetEventShortcut.Keys.ToList();
            BindingContext = fil;
            try
            {
                fil.APIKey = GetAPIKeyFromFile();
            }
            catch (Exception) { }
        }

        private async void Search_Clicked(object sender, EventArgs e)
        {
            if (!fil.IsOK())
            {
                await DisplayAlert("Cannot start search", "Some values are inappropriate", "Ok");
                return;
            }
            filteredEvent = eventsOptions.SelectedItem.ToString();
            SpaceEvent.AllEvents.Clear();
            GetData((Button)sender);
        }
        public async void GetData(Button button)
        {
            try
            {
                using(HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.nasa.gov/DONKI/");
                    HttpResponseMessage response = await client.GetAsync($"{Filter.GetEventShortcut[filteredEvent]}/?api_key={fil.APIKey}&startDate={fil.StartDate.ToString("yyyy-MM-dd")}&endDate={fil.EndDate.ToString("yyyy-MM-dd")}");

                    if (response.IsSuccessStatusCode)
                    {
                        dynamic data = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(), typeof(object));
                        int index = 1;
                        foreach (dynamic ev in data)
                        {
                            SpaceEvent.AllEvents.Add((SpaceEvent)ChooseEventTypeToParse[filteredEvent](ev, index));
                            UpdateLW();
                            index++;
                        }
                    }
                    else if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode.ToString() == "Forbidden")
                        {
                            await DisplayAlert("Invalid APIKey", $"API Key: {fil.APIKey}\nis invalid", "Ok");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "NotFound")
                {
                    await DisplayAlert("Service unavailable", $"Service is currently unavailable", "Ok");
                }
                else
                {
                    await DisplayAlert("Service unavailable", ex.Message, "Ok");
                }
            }
        }
        private void Details_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                SelectPageToShow[filteredEvent](SpaceEvent.AllEvents.IndexOf((SpaceEvent)button.BindingContext), filteredEvent);
                
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetAPIKeyFromFile()
        {
            string key = "";
            string[] lines = File.ReadAllLines(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "APIKey.txt"));
            foreach (var item in lines)
            {
                key = item;
            }
            return key;
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
        public void ShowCMEPage(int index, string type) => Navigation.PushAsync(new CMEPage(index, type));
        public void ShowGSTPage(int index, string type) => Navigation.PushAsync(new GSTPage(index, type));
        public void ShowIPSPage(int index, string type) => Navigation.PushAsync(new IPSPage(index, type));
        public void ShowFLRPage(int index, string type) => Navigation.PushAsync(new FLRPage(index, type));
        public void ShowSEPPage(int index, string type) => Navigation.PushAsync(new SEPPage(index, type));
        public void ShowMPCPage(int index, string type) => Navigation.PushAsync(new MPCPage(index, type));
        public void ShowRBEPage(int index, string type) => Navigation.PushAsync(new RBEPage(index, type));
        public void ShowHSSPage(int index, string type) => Navigation.PushAsync(new HSSPage(index, type));
        public delegate void ShowPage(int index, string type);
    }

    public class Filter: INotifyPropertyChanged
    {
        public delegate SpaceEvent ParseEventType(dynamic data, int index);
        public static Dictionary<string, string> GetEventShortcut = new Dictionary<string, string>() 
        {
            { "Coronal Mass Ejection", "CMEAnalysis"},
            { "Geomagnetic storm", "GST"},
            { "Interplanetary shock", "IPS"},
            { "Solar Flare", "FLR"},
            { "Solar Energetic Particle", "SEP"},
            { "Magnetopause Crossing", "MPC"},
            { "Radiation Belt Enhancement", "RBE"},
            { "Hight Speed Stream", "HSS"},
        };
        public static Dictionary<string, ParseEventType> ChooseEventTypeToParse = new Dictionary<string, ParseEventType>()
        {
            { "Coronal Mass Ejection", CME.ParseDataToObject},
            { "Geomagnetic storm", GST.ParseDataToObject},
            { "Interplanetary shock", IPS.ParseDataToObject},
            { "Solar Flare", FLR.ParseDataToObject},
            { "Solar Energetic Particle", SEP.ParseDataToObject},
            { "Magnetopause Crossing", MPC.ParseDataToObject},
            { "Radiation Belt Enhancement", RBE.ParseDataToObject},
            { "Hight Speed Stream", HSS.ParseDataToObject},
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

        public event PropertyChangedEventHandler PropertyChanged;

        public Filter() { }

        public Filter(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
            APIKey = string.Empty;
        }

        private string eventOption;
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
                WriteAPIKeyToFile(value);
                OnPropertyChanged(nameof(APIKey));
            }
        }
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public bool IsOK() => APIKey != string.Empty && EventOption != string.Empty && DateTime.Compare(StartDate, EndDate) <= 0;
        public void WriteAPIKeyToFile(string key)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "APIKey.txt")))
            {
                try
                {
                    sw.Write(key);
                    sw.Flush();
                }
                catch (Exception) { }
            }
        }
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
        public SpaceEvent(string eventName, DateTime eventDate, string eventLink)
        {
            ID = Guid.NewGuid();
            Name = eventName;
            Date = eventDate;
            link = eventLink;
        }
        public static List<SpaceEvent> AllEvents = new List<SpaceEvent>();
        private DateTime date;
        public DateTime Date { get => date; set => date = value; }
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

        public CME(double CMELatitude, double CMELongitude, double CMEHalfAngle, double CMESpeed, string CMEType, string CMENote, string CMEName, DateTime CMEDate, string CMELink): base(CMEName, CMEDate, CMELink)
        {
            Latitude = CMELatitude;
            Longitude = CMELongitude;
            HalfAngle = CMEHalfAngle;
            Speed = CMESpeed;
            Type = CMEType;
            Note = CMENote;
        }
        public CME() { }

        public string Note { get => note; set => note = value; }
        public static CME ParseDataToObject(dynamic data, int index)
        => new CME((double)data.latitude, (double)data.longitude, (double)data.halfAngle, (double)data.speed, (string)data.type, (string)data.note, $"CME-{index}", (DateTime)data.time21_5, (string)data.link);
    }

    public class GST : SpaceEvent //Geomagnetic storm
    {
        private List<KpIndex> kpIndexes;
        public List<KpIndex> KpIndexes { get => kpIndexes; set => kpIndexes = value; }

        public GST(List<KpIndex> GMSKpIndex, string GMSName, DateTime GMSDate, string GMSLink) : base(GMSName, GMSDate, GMSLink)
        {
            kpIndexes = GMSKpIndex;
        }
        public GST() { }
        public static GST ParseDataToObject(dynamic data, int index)
        {
            var kp = new List<KpIndex>();
            foreach (var item in data.allKpIndex)
            {
                kp.Add(new KpIndex((double)item.kpIndex, (DateTime)item.observedTime, (string)item.source));
            }
            return new GST(kp, $"GST-{index}", (DateTime)data.startTime, (string)data.link);
        }

    }
    public class KpIndex
    {
        private double index;
        public double Index { get => index; set => index = value; }
        private DateTime observedTime;
        public DateTime ObservedTime { get => observedTime; set => observedTime = value; }
        private string source;
        public string Source { get => source; set => source = value; }
        public KpIndex() { }
        public KpIndex(double kpIndex, DateTime timeObserved, string indexSource)
        {
            Index = kpIndex;
            ObservedTime = timeObserved;
            Source = indexSource;
        }
    }

    public class IPS : SpaceEvent //Interplanetary shock
    {
        private string location;
        public string Location { get => location; set => location = value; }
        private string instruments;
        public string Instruments { get => instruments; set => instruments = value; }
        public IPS(string IPSLocation, string IPSInstruments, string IPSName, DateTime IPSDate, string IPSLink): base(IPSName, IPSDate, IPSLink)
        {
            Location = IPSLocation;
            Instruments = IPSInstruments;
        }
        public IPS() { }
        public static IPS ParseDataToObject(dynamic data, int index)
        {
            string instruments = "";
            foreach (var item in data.instruments)
            {
                instruments += $"{item.displayName}\n";
            }

            Console.WriteLine(instruments.Substring(0, instruments.Length - 1));
            return new IPS((string)data.location, instruments, $"IPS-{index}", (DateTime)data.eventTime, (string)data.link);
        }
        public override string ToString() => Name;
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

        public FLR(string FLRClassType, string FLRSourceLocation, int FLRActiveRegionNum, string FLRInstruments, string FLRName, DateTime FLRDate, string FLRLink): base(FLRName, FLRDate, FLRLink)
        {
            ClassType = FLRClassType;
            SourceLocation = FLRSourceLocation;
            ActiveRegionNum = FLRActiveRegionNum;
            Instruments = FLRInstruments;
        }
        public FLR() { }
        public static FLR ParseDataToObject(dynamic data, int index)
        {
            string instruments = "";
            foreach (var item in data.instruments)
            {
                instruments += $"{item.displayName}\n";
            }

            Console.WriteLine();
            return new FLR((string)data.classType, (string)data.sourceLocation, (int)data.activeRegionNum, (string)instruments.Substring(0, instruments.Length - 1), $"FLR-{index}", (DateTime)data.beginTime, (string)data.link);
        }
    }
    public class SEP : SpaceEvent //Solar Energetic Particle
    {
        private string instruments;
        public string Instruments { get => instruments; set => instruments = value; }
        public SEP(string SEPInstruments, string SEPName, DateTime SEPDate, string SEPLink): base(SEPName, SEPDate, SEPLink) 
        {
            Instruments = SEPInstruments;
        }
        public SEP() { }
        public static SEP ParseDataToObject(dynamic data, int index)
        {
            string instruments = "";
            foreach (var item in data.instruments)
            {
                instruments += $"{item.displayName}\n";
            }

            Console.WriteLine();
            return new SEP((string)instruments.Substring(0, instruments.Length - 1), $"SEP-{index}", (DateTime)data.eventTime, (string)data.link);
        }
    }
    public class MPC : SpaceEvent //Magnetopause Crossing
    {
        private string instruments;
        public string Instruments { get => instruments; set => instruments = value; }
        public MPC(string MPCInstruments, string MPCName, DateTime MPCDate, string MPCLink) : base(MPCName, MPCDate, MPCLink)
        {
            Instruments = MPCInstruments;
        }
        public MPC() { }
        public static MPC ParseDataToObject(dynamic data, int index)
        {
            string instruments = "";
            foreach (var item2 in data.instruments)
            {
                instruments += $"{item2.displayName}\n";
            }

            Console.WriteLine();
            return new MPC((string)instruments.Substring(0, instruments.Length - 1), $"MPC-{index}", (DateTime)data.eventTime, (string)data.link);
        }
    }
    public class RBE : SpaceEvent //Radiation Belt Enhancement
    {
        private string instruments;
        public string Instruments { get => instruments; set => instruments = value; }
        public RBE(string RBEInstruments, string RBEName, DateTime RBEDate, string RBELink) : base(RBEName, RBEDate, RBELink)
        {
            Instruments = RBEInstruments;
        }
        public RBE() { }
        public static RBE ParseDataToObject(dynamic data, int index)
        {
            string instruments = "";
            foreach (var item in data.instruments)
            {
                instruments += $"{item.displayName}\n";
            }

            Console.WriteLine();
            return new RBE((string)instruments.Substring(0, instruments.Length - 1), $"RBE-{index}", (DateTime)data.eventTime, (string)data.link);
        }
    }
    public class HSS : SpaceEvent //Hight Speed Stream
    {
        private string instruments;
        public string Instruments { get => instruments; set => instruments = value; }
        public HSS(string HSSInstruments, string HSSName, DateTime HSSDate, string HSSLink) : base(HSSName, HSSDate, HSSLink)
        {
            Instruments = HSSInstruments;
        }
        public HSS() { }
        public static HSS ParseDataToObject(dynamic data, int index)
        {
            string instruments = "";
            foreach (var item in data.instruments)
            {
                instruments += $"{item.displayName}\n";
            }

            Console.WriteLine();
            return new HSS((string)instruments.Substring(0, instruments.Length - 1), $"HSS-{index}", (DateTime)data.eventTime, (string)data.link);
        }
    }
}
