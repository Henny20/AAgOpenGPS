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
		connection.CreateTable<Boundary>();
		connection.CreateTable<BoundaryPolygon>();
		
		// SEED		
		var boundaryList = new List<Boundary>
		{
			new Boundary { OuterBoundary = new BoundaryPolygon { Area = 100.0 } },
			new Boundary { OuterBoundary = new BoundaryPolygon { Area = 200.0 } },
			new Boundary { OuterBoundary = new BoundaryPolygon { Area = 300.0 } }
		};
		connection.BulkInsert(boundaryList);
		
		// CODE
		var outputList = connection.Query<Boundary>("SELECT * FROM Boundary").ToList();
		
		// OUTPUT
		FiddleHelper.WriteTable(outputList);
	}
	
	[Table("Boundary")]
	public class Boundary
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int Id { get; set; }
		public BoundaryPolygon OuterBoundary { get; set; }
		public List<BoundaryPolygon> InnerBoundaries { get; } = new List<BoundaryPolygon>();
		public double Area => OuterBoundary?.Area ?? 0.0;
	}

	[Table("BoundaryPolygon")]
	public class BoundaryPolygon
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int Id { get; set; }
		public double Area { get; set; }
	}
	
	public class GeoPolygonWithHeading
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int Id { get; set; }
		public double Area { get; set; }
		public int Count { get; set; }
	}
}

