using AgOpenGPS.Core;
using AgOpenGPS.Core.Models;
using System;
using System.Globalization;
using System.Text;
using AAgOpenGPS.ViewModels;
using AAgOpenGPS.Models;

namespace AAgOpenGPS.Android
{
    public class CNMEA
    {
        
        //our current fix
        public vec2 fix = new vec2(0, 0);

        //other GIS Info
        public double altitude, speed, vtgSpeed = float.MaxValue;

        public double headingTrueDual, headingTrue, hdop, age, headingTrueDualOffset;

        public int fixQuality, ageAlarm;
        public int satellitesTracked;

       // private readonly FormGPS mf;
        public Main mf { get; }

       // public CNMEA(FormGPS f)
        public CNMEA(Main _f)
        {
            //constructor, grab the main form reference
            mf = _f; 
           // mfAppModel.LocalPlane = new LocalPlane(new Wgs84(0, 0), mfAppModel.SharedFieldProperties); hennie
          // ageAlarm = Properties.Settings.Default.setGPS_ageAlarm; hennie
        }

        public void AverageTheSpeed()
        {
            //average the speed
            //if (speed > 70) speed = 70;
          //  mfavgSpeed = (mfavgSpeed * 0.75) + (speed * 0.25); hennie
        }

        public void DefineLocalPlane(Wgs84 origin, bool setSim)
        {
        /****
            mfAppModel.LocalPlane = new LocalPlane(origin, mfAppModel.SharedFieldProperties);
            if (setSim && mftimerSim.Enabled)
            {
                mfAppModel.CurrentLatLon = origin;
                mfsim.CurrentLatLon = origin;

                Properties.Settings.Default.setGPS_SimLatitude = mfAppModel.LocalPlane.Origin.Latitude;
                Properties.Settings.Default.setGPS_SimLongitude = mfAppModel.LocalPlane.Origin.Longitude;
                Properties.Settings.Default.Save();
            }
            GeoCoord geoCoord = mfAppModel.LocalPlane.ConvertWgs84ToGeoCoord(mfAppModel.CurrentLatLon);
            mfworldGrid.checkZoomWorldGrid(geoCoord);
            ******/
        }

    }
}
