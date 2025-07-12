using System;
using System.Collections.Generic;
using AAgOpenGPS.ViewModels;
using AAgOpenGPS.Models;

namespace AAgOpenGPS.Android
{
    public class CHeadLine
    {
        //pointers to mainform controls
        //private readonly FormGPS mf;
       public Main mf { get; }

        public List<CHeadPath> tracksArr = new List<CHeadPath>();

        public int idx;

        public List<vec3> desList = new List<vec3>();

        public CHeadLine(Main _f)
        {
            //constructor
            mf = _f;
        }

        //for calculating for display the averaged new line

    }

    public class CHeadPath
    {
        public List<vec3> trackPts = new List<vec3>();
        public string name = "";
        public double moveDistance = 0;
        public int mode = 0;
        public int a_point = 0;
    }
}
