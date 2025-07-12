using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using SQLite;
using AAgOpenGPS.Models;

namespace AAgOpenGPS;

class DataAccess
{

    public static void InitializeDatabase()
    {
        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "file.db");
        File.Delete(dbpath);
       // string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "file.db");
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "file.db");
        using (var db = new SQLiteConnection(path))
        {
            db.CreateTable<Setting>();
            db.CreateTable<Vehicle>();
        }
        Seed.Insert();
    }
}
