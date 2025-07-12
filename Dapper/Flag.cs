CREATE TABLE Flag (
    FlagID INT PRIMARY KEY IDENTITY(1,1),
    Latitude FLOAT,
    Longitude FLOAT,
    Northing FLOAT,
    Easting FLOAT,
    FlagColor NVARCHAR(50),
    UniqueNumber INT,
    Notes NVARCHAR(255)
);

// HEADER DIRECTIVE
// @nuget: Dapper
// @nuget: Microsoft.Data.SqlClient
// @nuget: Z.Dapper.Plus

// USING DIRECTIVE
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using Z.Dapper.Plus;

public class Program
{	
	public static string ConnectionString = FiddleHelper.GetConnectionStringSqlServer();

	public static void Main()
	{
		// CONNECTION
		var connection = new SqlConnection(ConnectionString);
		
		// CREATE TABLE
		connection.CreateTable<Flag>();
		
		// SEED		
		var list = new List<Flag>();
		list.Add(new Flag(new Wgs84(34.0522, -118.2437), new GeoCoordDir(new GeoCoord(100.0, 200.0), GeoDir.North), FlagColor.Red, 1, "First Flag"));
		list.Add(new Flag(new Wgs84(40.7128, -74.0060), new GeoCoordDir(new GeoCoord(150.0, 250.0), GeoDir.East), FlagColor.Blue, 2, "Second Flag"));
		list.Add(new Flag(new Wgs84(37.7749, -122.4194), new GeoCoordDir(new GeoCoord(120.0, 220.0), GeoDir.West), FlagColor.Green, 3, "Third Flag"));
		connection.BulkInsert(list);
		
		// CODE
		var outputList = connection.Query<Flag>("SELECT * FROM Flag").ToList();
		
		// OUTPUT
		FiddleHelper.WriteTable(outputList);
	}
	
	[Table("Flag")]
	public class Flag
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int FlagID { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double Northing { get; set; }
		public double Easting { get; set; }
		public string FlagColor { get; set; }
		public int UniqueNumber { get; set; }
		public string Notes { get; set; }

		public Flag(Wgs84 wgs84, GeoCoordDir geoCoordDir, FlagColor flagColor, int uniqueNumber, string notes)
		{
			Latitude = wgs84.Latitude;
			Longitude = wgs84.Longitude;
			Northing = geoCoordDir.Coord.Northing;
			Easting = geoCoordDir.Coord.Easting;
			FlagColor = flagColor.ToString();
			UniqueNumber = uniqueNumber;
			Notes = notes;
		}
	}

	public class Wgs84
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public Wgs84(double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}
	}

	public class GeoCoord
	{
		public double Northing { get; set; }
		public double Easting { get; set; }

		public GeoCoord(double northing, double easting)
		{
			Northing = northing;
			Easting = easting;
		}
	}

	public class GeoCoordDir
	{
		public GeoCoord Coord { get; set; }
		public GeoDir Direction { get; set; }

		public GeoCoordDir(GeoCoord coord, GeoDir direction)
		{
			Coord = coord;
			Direction = direction;
		}
	}

	public enum GeoDir
	{
		North,
		South,
		East,
		West
	}

	public enum FlagColor
	{
		Red,
		Blue,
		Green
	}
}

