using System;
//using Avalonia.Media:

namespace AgOpenGPS.Core.Models
{
    public class BackgroundPicture
    {
        public BackgroundPicture(bool isGeoMap)
        {
            IsGeoMap = isGeoMap;
        }

        public bool IsGeoMap { get; }
        public GeoBoundingBox GeoBoundingBox { get; set; }
        public Object Picture { get; set; } //hack

    }
}
