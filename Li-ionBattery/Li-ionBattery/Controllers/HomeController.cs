using CsvHelper;
using CsvHelper.Configuration;
using Li_ionBattery.Models;
using Li_ionBattery.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Li_ionBattery.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment Environment;
        private IConfiguration Configuration;
        private readonly DataBaseConn db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DataBaseConn _db, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.Environment = hostingEnvironment;
            this.db = _db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CsvUpload2NdRound()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CsvUpload2NdRound(IFormFile formFile2)
        {
            //try
            //{
            //    var data = new MemoryStream();
            //    formFile2.CopyTo(data);

            //    data.Position = 0;
            //    using (var reader = new StreamReader(data))
            //    {
            //        var bad = new List<string>();
            //        var conf = new CsvConfiguration(CultureInfo.InvariantCulture)
            //        {
            //            HasHeaderRecord = true,
            //            HeaderValidated = null,
            //            MissingFieldFound = null,
            //            BadDataFound = context =>
            //            {
            //                bad.Add(context.RawRecord);
            //            }
            //        };
            //        using (var csvReader = new CsvReader(reader, conf))
            //        {
            //            while (csvReader.Read())
            //            {
            //                var ID = csvReader.GetField(0).ToString();
            //                var Resistance = csvReader.GetField(1).ToString();
            //                var Voltage = csvReader.GetField(2).ToString();
            //                var Temperature = csvReader.GetField(3).ToString();
            //                var DateTimeValue = csvReader.GetField(4).ToString();
            //                if (ID != "ID")
            //                {
            //                    int BatteryID = int.Parse(ID);
            //                    var UpdateBattery = db.Li_ionBattery.Single(ID => ID.BatteryId == BatteryID);
            //                    UpdateBattery.Resistance2 = Resistance;
            //                    UpdateBattery.Voltage2 = Voltage;
            //                    UpdateBattery.Temperature2 = Temperature;
            //                    UpdateBattery.DateTime2 = DateTimeValue;
            //                    UpdateBattery.ModifiedDate = DateTime.Now;
            //                    db.SaveChanges();
            //                }
            //                else
            //                {

            //                }
            //            }
            //        }
            //        TempData["Message"] = "Data Inserted";
            //    }
            //    return View("CsvUpload");
            //}
            //catch (Exception ex)
            //{
            //    return View("CsvUpload", ex.InnerException.Message);
            //    throw;
            //}
            return View();
        }
        public IActionResult CsvUpload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CsvUpload(IFormFile postedFile)
        {
            try
            {
                string filePath = string.Empty;
                if (postedFile != null)
                {
                    string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }

                    //Create a DataTable.
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[9] { (new  DataColumn("ID")),
                                new DataColumn("Resistance"),
                                new DataColumn("Voltage"),
                                new DataColumn("Temperature"),
                                new DataColumn("DateTime"),
                                new DataColumn("Resistance2"),
                                new DataColumn("Voltage2"),
                                new DataColumn("Temperature2"),
                                new DataColumn("DateTime2")});


                    //Read the contents of CSV file.
                    string csvData = System.IO.File.ReadAllText(filePath);

                    //Execute a loop over the rows.
                    foreach (string row in csvData.Split('\r'))
                    {
                        if (row.Contains("ID"))
                        {

                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(row))
                            {
                                dt.Rows.Add();
                                int i = 0;

                                //Execute a loop over the columns.
                                foreach (string cell in row.Split(','))
                                {
                                    dt.Rows[dt.Rows.Count - 1][i] = cell;
                                    i++;
                                }
                            }
                        }
                    }

                    string conString = this.Configuration.GetConnectionString("Mycon");
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            //Set the database table name.
                            sqlBulkCopy.DestinationTableName = "dbo.Li_ionBattery";

                            //[OPTIONAL]: Map the DataTable columns with that of the database table
                            sqlBulkCopy.ColumnMappings.Add("ID", "BatteryId");
                            sqlBulkCopy.ColumnMappings.Add("Resistance", "Resistance1");
                            sqlBulkCopy.ColumnMappings.Add("Voltage", "Voltage1");
                            sqlBulkCopy.ColumnMappings.Add("Temperature", "Temperature1");
                            sqlBulkCopy.ColumnMappings.Add("DateTime", "DateTime1");
                            sqlBulkCopy.ColumnMappings.Add("Resistance2", "Resistance2");
                            sqlBulkCopy.ColumnMappings.Add("Voltage2", "Voltage2");
                            sqlBulkCopy.ColumnMappings.Add("Temperature2", "Temperature2");
                            sqlBulkCopy.ColumnMappings.Add("DateTime2", "DateTime2");
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                return View(ex.InnerException.Message);
                throw;
            }

           
        }
        //try
        //{
        //    var data = new MemoryStream();
        //    formFile.CopyToAsync(data);

        //    data.Position = 0;
        //    using (var reader = new StreamReader(data))
        //    {
        //        var bad = new List<string>();
        //        var conf = new CsvConfiguration(CultureInfo.InvariantCulture)
        //        {
        //            HasHeaderRecord = true,
        //            HeaderValidated = null,
        //            MissingFieldFound = null,
        //            BadDataFound = context =>
        //            {
        //                bad.Add(context.RawRecord);
        //            }
        //        };
        //        using (var csvReader = new CsvReader(reader, conf))
        //        {
        //            while (csvReader.Read())
        //            {
        //                var ID = csvReader.GetField(0).ToString();
        //                if (ID != "ID")
        //                {
        //                    var Resistance = csvReader.GetField(1).ToString();
        //                    var Voltage = csvReader.GetField(2).ToString();
        //                    var Temperature = csvReader.GetField(3).ToString();
        //                    var DateTimeValue = csvReader.GetField(4).ToString();
        //                    db.Add(new Battery
        //                    {
        //                        BatteryId = int.Parse(ID),
        //                        Resistance1 =Resistance,
        //                        Voltage1 = Voltage,
        //                        Temperature1 = Temperature,
        //                        DateTime1 = DateTimeValue,
        //                        UplodedDate = DateTime.Now

        //                    });
        //                    db.SaveChanges();
        //                }
        //                else
        //                {

        //                }
        //            }
        //        }
        //        TempData["Message"] = "Data Inserted";
        //    }
        //    return View();
        //}
        //catch (Exception ex)
        //{
        //    return View(ex.InnerException.Message);
        //    throw;
        //}
        // }
        [Route("BatteryStatus")]
        public IActionResult BatteryStatus()
        {
            try
            {
                var data = db.Li_ionBattery.ToList();

                return View(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}