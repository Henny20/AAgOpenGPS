using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using AAgOpenGPS.Models;
using AgOpenGPS.Core.ViewModels;
//using Dapper;
using SQLite;
//using Microsoft.Data.Sqlite;

namespace AAgOpenGPS.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public static MainViewModel? Instance { get; private set; }
    
       // private static MainViewModel instance = new MainViewModel();
  //  public static MainViewModel getInstance() { 
  //     return instance;
  // }
  
 // private MainViewModel _instance;
  //  public MainViewModel Instance
 //   {
  //      get { return _instance ?? (_instance = new Instance()); }
  //  }
    
    public ObservableCollection<FieldDescriptionViewModel> Fields { get; }
    //public ObservableCollection<string> Vehicles { get; }
    public ObservableCollection<Vehicle> Vehicles { get; }

    public MainViewModel()
    {
        Instance = this;
        
        var _fields = new List<FieldDescriptionViewModel>
        {
            new FieldDescriptionViewModel(new DirectoryInfo("Perceel1"), "foo", "something"),
            new FieldDescriptionViewModel(new DirectoryInfo("Perceel2"), "somewhere", "rt")
        };
        Fields = new ObservableCollection<FieldDescriptionViewModel>(_fields);

        //Vehicles = new ObservableCollection<string> { "Default Vehicle", "John Deere 5M" };
        IEnumerable<Vehicle> _vehicles = null;
        string dbpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "file.db");
        using (var connection = new SQLiteConnection(dbpath))
        {
            _vehicles = connection.Query<Vehicle>("SELECT * FROM Vehicle");
            foreach (var item in _vehicles)
            {
                System.Console.WriteLine(item.Name);
            }
        }
        //string json = JsonConvert.SerializeObject(_fields);
        Vehicles = new ObservableCollection<Vehicle>(_vehicles);
        
        // doe iets
        /***
        vehicle = new CVehicle(this);

            tool = new CTool(this);

            //create a new section and set left and right positions
            //created whether used or not, saves restarting program

        //    section = new CSection[MAXSECTIONS];
          //  for (int j = 0; j < MAXSECTIONS; j++) section[j] = new CSection();

            triStrip = new List<CPatches>
            {
                new CPatches(this)
            };

            //our NMEA parser
            pn = new CNMEA(this);

            //create the ABLine instance
            ABLine = new CABLine(this);

            //new instance of contour mode
            ct = new CContour(this);

            //new instance of contour mode
            curve = new CABCurve(this);

            //new track instance
            trk = new CTrack(this);

            //new instance of contour mode
            hdl = new CHeadLine(this);

            ////new instance of auto headland turn
            yt = new CYouTurn(this);

            //module communication
            mc = new CModuleComm(this);

            //boundary object
            bnd = new CBoundary(this);

            //nmea simulator built in.
            sim = new CSim(this);

            ////all the attitude, heading, roll, pitch reference system
            ahrs = new CAHRS();

            //A recorded path
            recPath = new CRecordedPath(this);

            //fieldData all in one place
            fd = new CFieldData(this);

              //instance of tram
          //  tram = new CTram(this);

            //the new steer algorithms
           gyd = new CGuidance(this);
 **********/

    }
    //new field
    [RelayCommand]
    private void NewField()
    {
        Fields.Add(new FieldDescriptionViewModel(new DirectoryInfo("Perceel3"), "10", "10"));
    }
    [RelayCommand]
    private void AddDate() { }

    [RelayCommand]
    private void AddTime() { }

    [ObservableProperty]
    private string _addField = "";

    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";

    //Type radio
    [ObservableProperty]
    private bool _fourWDIsChecked = true;

    [ObservableProperty]
    private bool _tractorIsChecked = false;

    [ObservableProperty]
    private bool _harvesterIsChecked = false;
    //Antenna nuds
    [ObservableProperty]
    private int _antennaHeight = 10;

    [ObservableProperty]
    private int _antennaPivot = 300;

    [ObservableProperty]
    private int _antennaOffset = 0;

    [ObservableProperty]
    private int _vehicleTrack = 0;

    [ObservableProperty]
    private int _wheelbase = 0;

    //Guidance nuds
    [ObservableProperty]
    private int _aBLength;

    [ObservableProperty]
    private int _lineWidth;

    [ObservableProperty]
    private int _snapdistance;
    //Switches
    [ObservableProperty]
    private bool _setAutoSectionsSteer = false;
    [ObservableProperty]
    private bool _selectSteerSwitch;
    [ObservableProperty]
    private bool _setManualSectionsSteer;
    [ObservableProperty]
    private bool _selectWorkSwitch = false;
    [ObservableProperty]
    private bool _setManualSections = false;
    [ObservableProperty]
    private bool _workSwActiveLow = false;
    /***
    
       nudTrailingHitchLength.Controls[0].Enabled = false;
            nudDrawbarLength.Controls[0].Enabled = false;
            nudTankHitch.Controls[0].Enabled = false;
            nudTractorHitchLength.Controls[0].Enabled = false;

            nudLookAhead.Controls[0].Enabled = false;
            nudLookAheadOff.Controls[0].Enabled = false;
            nudTurnOffDelay.Controls[0].Enabled = false;
            nudOffset.Controls[0].Enabled = false;
            nudOverlap.Controls[0].Enabled = false;
            nudCutoffSpeed.Controls[0].Enabled = false;

            

            nudMinCoverage.Controls[0].Enabled = false;
            nudDefaultSectionWidth.Controls[0].Enabled = false;

            nudSection01.Controls[0].Enabled = false;
            nudSection02.Controls[0].Enabled = false;
            nudSection03.Controls[0].Enabled = false;
            nudSection04.Controls[0].Enabled = false;
            nudSection05.Controls[0].Enabled = false;
            nudSection06.Controls[0].Enabled = false;
            nudSection07.Controls[0].Enabled = false;
            nudSection08.Controls[0].Enabled = false;
            nudSection09.Controls[0].Enabled = false;
            nudSection10.Controls[0].Enabled = false;
            nudSection11.Controls[0].Enabled = false;
            nudSection12.Controls[0].Enabled = false;
            nudSection13.Controls[0].Enabled = false;
            nudSection14.Controls[0].Enabled = false;
            nudSection15.Controls[0].Enabled = false;
            nudSection16.Controls[0].Enabled = false;
            nudNumberOfSections.Controls[0].Enabled = false;

            nudZone1To.Controls[0].Enabled = false;
            nudZone2To.Controls[0].Enabled = false;
            nudZone3To.Controls[0].Enabled = false;
            nudZone4To.Controls[0].Enabled = false;
            nudZone5To.Controls[0].Enabled = false;
            nudZone6To.Controls[0].Enabled = false;

            nudRaiseTime.Controls[0].Enabled = false;
            nudLowerTime.Controls[0].Enabled = false;

            nudUser1.Controls[0].Enabled = false;
            nudUser2.Controls[0].Enabled = false;
            nudUser3.Controls[0].Enabled = false;
            nudUser4.Controls[0].Enabled = false;

            nudTramWidth.Controls[0].Enabled = false;

            nudDualHeadingOffset.Controls[0].Enabled = false;
            nudDualReverseDistance.Controls[0].Enabled = false;

            nudOverlap.Controls[0].Enabled = false;
            nudOffset.Controls[0].Enabled = false;

            nudTrailingToolToPivotLength.Controls[0].Enabled = false;

            nudFixJumpDistance.Controls[0].Enabled = false;
    
    ****/
    //settings
    [ObservableProperty]
    private bool _displayImperial = false;
    
    [ObservableProperty]
    private bool _displayMetric = true;

    //DRoll
    [RelayCommand]
    private void ResetIMU() { }

    [RelayCommand]
    private void ZeroRoll() { }

    [RelayCommand]
    private void RemoveZeroOffset() { }
    ///  

    [ObservableProperty]
    private int _guidanceLookAhead;

    [ObservableProperty]
    private int _lightbarCmPerPixel = 0;
    //Steer Config
    [RelayCommand]
    private void ZeroWAS()
    {
        System.Console.WriteLine("Hello!");
    }
    
    [RelayCommand]
    private void AutoSteer() { }

}
