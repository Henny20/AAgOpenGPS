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
		connection.CreateTable<VehicleConfig>();
		
		// SEED		
		var list = new List<VehicleConfig>
		{
			new VehicleConfig { Type = VehicleType.Tractor, Color = new ColorRgb(255, 0, 0), Opacity = 0.8, AntennaHeight = 2.5, AntennaPivot = 1.0, AntennaOffset = 0.5, Wheelbase = 3.0, TrackWidth = 1.5 },
			new VehicleConfig { Type = VehicleType.Harvester, Color = new ColorRgb(0, 255, 0), Opacity = 0.9, AntennaHeight = 2.0, AntennaPivot = 1.2, AntennaOffset = 0.6, Wheelbase = 3.5, TrackWidth = 1.8 },
			new VehicleConfig { Type = VehicleType.Articulated, Color = new ColorRgb(0, 0, 255), Opacity = 0.7, AntennaHeight = 2.2, AntennaPivot = 1.1, AntennaOffset = 0.4, Wheelbase = 4.0, TrackWidth = 2.0 }
		};
		connection.BulkInsert(list);
		
		// CODE
		var outputList = connection.Query<VehicleConfig>("SELECT * FROM VehicleConfig").ToList();
		
		// OUTPUT
		FiddleHelper.WriteTable(outputList);
	}
	
	public enum VehicleType
	{
		Tractor = 0,
		Harvester = 1,
		Articulated = 2
	}

	[Table("VehicleConfig")]
	public class VehicleConfig
	{
		[Key]
		public int Id { get; set; }
		public VehicleType Type { get; set; }
		public ColorRgb Color { get; set; }
		public double Opacity { get; set; }
		public double AntennaHeight { get; set; }
		public double AntennaPivot { get; set; }
		public double AntennaOffset { get; set; }
		public double Wheelbase { get; set; }
		public double TrackWidth { get; set; }
	}

	public class ColorRgb
	{
		public int R { get; set; }
		public int G { get; set; }
		public int B { get; set; }

		public ColorRgb(int r, int g, int b)
		{
			R = r;
			G = g;
			B = b;
		}
	}
}

