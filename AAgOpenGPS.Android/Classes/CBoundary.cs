using System.Collections.Generic;
using AAgOpenGPS.ViewModels;

namespace AAgOpenGPS.Android
{
    public partial class CBoundary
    {
        //copy of the mainform address
       // private readonly FormGPS mf;
       public Main mf { get; set; }


        public List<CBoundaryList> bndList = new List<CBoundaryList>();

        //constructor
       // public CBoundary(FormGPS _f)
        public CBoundary(Main _f)
        {
            mf = _f;
            turnSelected = 0;
            mf.isHeadlandOn = false; //TODO
            mf.isSectionControlledByHeadland = false; //Properties.Settings.Default.setHeadland_isSectionControlled; hennie
        }
    }
}
