using SQLite;

namespace AAgOpenGPS.Models
{
    public enum Type
    {
        Tractor = 0,
        Harvester = 1,
        Articulated = 2
    }

    public class Vehicle
    {

        /***public Vehicle(
                int id,
                int type,
                string name,
                double antennaHeight,
                double antennaPivot,
               double antennaOffset,
               double wheelbase,
               double trackWidth
               )
        {
            Id = id;
            Type = type;
            Name = name;
            AntennaHeight = antennaHeight;
            AntennaPivot = antennaPivot;
            AntennaOffset = antennaOffset;
            Wheelbase = wheelbase;
            TrackWidth = trackWidth;
        }****/
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public double AntennaHeight { get; set; }
        public double AntennaPivot { get; set; }
        public double AntennaOffset { get; set; }
        public double Wheelbase { get; set; }
        public double TrackWidth { get; set; }
    }
}
