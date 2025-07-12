using System.IO;

namespace AAgOpenGPS.Models
{
    public class FieldDescription
    {
        public FieldDescription(
            DirectoryInfo directoryInfo,
            string area,
            string distance)
        {
            DirectoryInfo = directoryInfo;
            FieldArea = area;
            FieldDistance = distance;
        }

        public DirectoryInfo DirectoryInfo { get; }
        public string FieldName => DirectoryInfo.Name;
        public string FieldArea{ get; set; }
        public string FieldDistance { get; set; }

    }

}
