using AAgOpenGPS;
using System.Collections.Generic;
using System.Diagnostics;
using AgOpenGPS.Core;
using AgOpenGPS.Core.Models;
using Avalonia.Media;
using Avalonia.Threading;
       
namespace AAgOpenGPS.ViewModels;    
   //helper class
    public partial class MainViewModel
    {
       public double fixHeading = 0.0;
      public Wgs84 CurrentLatLon { get; set; }
       public GeoDir FixHeading { get; set; }

       public LocalPlane LocalPlane { get; set; }
       
       
        public string displayFieldName = "none"; //TODO
        public double gpsHz = 10;
        
        public bool isMetric = true;

        //whether or not to use Stanley control
        public bool isStanleyUsed = true;
        
        public int makeUTurnCounter = 0; //GUI.Designer.cs
        public int hardwareLineCounter = 0;
        public bool isHardwareMessages = false;
        public bool isLogElevation = false;
        
        public bool isBtnAutoSteerOn = false;
        
        public double lightbarDistance; //OpenGL.Designer.cs
          public int steerModuleConnectedCounter = 0;
          
        public bool isJobStarted = false;
        
        public bool isHeadlandOn = false;
        public bool isSectionControlledByHeadland = false;
           
        public enum btnStates { Off, Auto, On } //Section.Designer.cs
         public btnStates manualBtnState = btnStates.Off;
        public btnStates autoBtnState = btnStates.Off;

        
        public bool isPatchesChangingColor = false; //Controls.Designer.cs
        
      //  public List<List<vec3>> patchSaveList = new List<List<vec3>>(); //SaveOpen.Designer.cs
        //GUI.Designer.cs
     //   public CFeatureSettings featureSettings = new CFeatureSettings(); 
        public bool isLightbarOn = true, isGridOn, isFullScreen;
        public bool isUTurnAlwaysOn, isSpeedoOn, isSideGuideLines = true;
        public bool isPureDisplayOn = true, isSkyOn = true, isRollMeterOn = false, isTextureOn = true;
        
          public uint sentenceCounter = 0;
        /////
         public DispatcherTimer timerSim = new();

        
       public double secondsSinceStart;
      
        public Color frameNightColor;
        public Color sectionColorDay;
        public Color fieldColorDay;
        
         private readonly Stopwatch swFrame = new Stopwatch();
         
            public double frameTime = 0;
            
   

   //  public CCamera camera;

        /// <summary>
        /// create world grid
        /// </summary>
        /***public WorldGrid worldGrid;***/

        /// <summary>
        /// The NMEA class that decodes it
        /// </summary>
       // public CNMEA pn;

        /// <summary>
        /// an array of sections
        /// </summary>
    //    public CSection[] section; 

        /// <summary>
        /// an array of patches to draw
        /// </summary>
        //public CPatches[] triStrip;
   //    public List<CPatches> triStrip;
//
        /// <summary>
        /// AB Line object
        /// </summary>
    //   public CABLine ABLine; 

        /// <summary>
        /// TramLine class for boundary and settings
        /// </summary>
    //   public CTram tram;

        /// <summary>
        /// Contour Mode Instance
        /// </summary>
      //  public CContour ct; 

        /// <summary>
        /// Contour Mode Instance
        /// </summary>
      //  public CTrack trk;

        /// <summary>
        /// ABCurve instance
        /// </summary>
     //  public CABCurve curve;

        /// <summary>
        /// Auto Headland YouTurn
        /// </summary>
     //  public CYouTurn yt;

        /// <summary>
        /// Our vehicle only
        /// </summary>
      // public CVehicle vehicle;

        /// <summary>
        /// Just the tool attachment that includes the sections
        /// </summary>
      //  public CTool tool;

        /// <summary>
        /// All the structs for recv and send of information out ports
        /// </summary>
   //    public CModuleComm mc;

        /// <summary>
        /// The boundary object
        /// </summary>
      // public CBoundary bnd;

        /// <summary>
        /// Building a headland instance
        /// </summary>
     //  public CHeadLine hdl;

        /// <summary>
        /// The internal simulator
        /// </summary>
      // public CSim sim;

        /// <summary>
        /// Heading, Roll, Pitch, GPS, Properties
        /// </summary>
      // public CAHRS ahrs;

        /// <summary>
        /// Recorded Path
        /// </summary>
      //  public CRecordedPath recPath;

        /// <summary>
        /// Most of the displayed field data for GUI
        /// </summary>
      //  public CFieldData fd;
        
        // public CGuidance gyd;
}
