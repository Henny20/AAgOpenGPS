using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;
using Avalonia;
using Avalonia.Android;
using System.Collections.Generic;

using Android.Views;

namespace AAgOpenGPS.Android;

[Activity(
    Label = "AAgOpenGPS.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{

        public CCamera camera;

        /// <summary>
        /// create world grid
        /// </summary>
        /***public WorldGrid worldGrid;***/

        /// <summary>
        /// The NMEA class that decodes it
        /// </summary>
        public CNMEA pn;

        /// <summary>
        /// an array of sections
        /// </summary>
        public CSection[] section; 

        /// <summary>
        /// an array of patches to draw
        /// </summary>
        //public CPatches[] triStrip;
        public List<CPatches> triStrip;

        /// <summary>
        /// AB Line object
        /// </summary>
       public CABLine ABLine; 

        /// <summary>
        /// TramLine class for boundary and settings
        /// </summary>
       public CTram tram;

        /// <summary>
        /// Contour Mode Instance
        /// </summary>
        public CContour ct; 

        /// <summary>
        /// Contour Mode Instance
        /// </summary>
        public CTrack trk;

        /// <summary>
        /// ABCurve instance
        /// </summary>
       public CABCurve curve;

        /// <summary>
        /// Auto Headland YouTurn
        /// </summary>
       public CYouTurn yt;

        /// <summary>
        /// Our vehicle only
        /// </summary>
       public CVehicle vehicle;

        /// <summary>
        /// Just the tool attachment that includes the sections
        /// </summary>
        public CTool tool;

        /// <summary>
        /// All the structs for recv and send of information out ports
        /// </summary>
       public CModuleComm mc;

        /// <summary>
        /// The boundary object
        /// </summary>
       public CBoundary bnd;

        /// <summary>
        /// Building a headland instance
        /// </summary>
       public CHeadLine hdl;

        /// <summary>
        /// The internal simulator
        /// </summary>
       public CSim sim;

        /// <summary>
        /// Heading, Roll, Pitch, GPS, Properties
        /// </summary>
       public CAHRS ahrs;

        /// <summary>
        /// Recorded Path
        /// </summary>
        public CRecordedPath recPath;

        /// <summary>
        /// Most of the displayed field data for GUI
        /// </summary>
        public CFieldData fd;
        
         public CGuidance gyd;
         
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        AAgOpenGPS.RegisteredServices.GLService = new AndroidGLService();
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
             .AfterSetup(_ =>
                 {
                     Pages.EmbedGL.Implementation = new EmbedGLAndroid();
                 });
    }
    
    
    protected override void OnCreate(Bundle bundle)
    {
        base.OnCreate(bundle);
        new Main();
        
    }   
    /****
    private const float TOUCH_SCALE_FACTOR = 180.0f / 320;
	private float previousX;
	private float previousY;

	public override bool OnTouchEvent(MotionEvent e)
	{
		// MotionEvent reports input details from the touch screen
		// and other input controls. In this case, you are only
		// interested in events where the touch position changed.

		float x = e.GetX();
		float y = e.GetY();

		switch (e.Action)
		{
		    case MotionEventActions.Move:

		        float dx = x - previousX;
		        float dy = y - previousY;

		        // reverse direction of rotation above the mid-line
		        if (y > Height / 2)
		        {
		            dx = dx * -1;
		        }

		        // reverse direction of rotation to left of the mid-line
		        if (x < Width / 2)
		        {
		            dy = dy * -1;
		        }

		        renderer.SetAngle(
		            renderer.GetAngle() +
		            ((dx + dy) * TOUCH_SCALE_FACTOR));
		        RequestRender();
		        break;
		}

		previousX = x;
		previousY = y;
		return true;
	} 
	*************/
   
}
