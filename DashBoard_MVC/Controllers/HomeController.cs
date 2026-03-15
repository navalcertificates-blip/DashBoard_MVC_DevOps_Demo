using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using DashBoard_MVC.Models;
using System.Configuration;


namespace DashBoard_MVC.Controllers
{
    public class HomeController : Controller
    {
        SqlCommand scmd = new SqlCommand();
        SqlDataReader sdr;
        SqlConnection scon = new SqlConnection();
        private readonly ILogger<HomeController> _logger;
        List<Insurance> insurances = new List<Insurance>();

        public HomeController()
        { 
        }
        public HomeController(ILogger<HomeController> logger)
        { 
            _logger = logger;
            //scon.ConnectionString = DashBoard_MVC.Properties.Resources.ConnectionString.ToString();
            scon.ConnectionString = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        }

        [HttpGet]
        public ActionResult Index()
        {
            //FetchData();
            //return View(insurances);
            return (View(GetInsurances(null)));
        }

        [HttpPost]
        public ActionResult Index(string name)
        {
            //FetchDataWithName(insurance.CustomerName);
            return View(GetInsurances(name));
        }

        public ActionResult IndexWithName(string name)
        {
            FetchDataWithName(name);
            return View(insurances);
        }

        private List<Insurance> GetInsurances(string name)
        {
            List<Insurance> list = new List<Insurance>();
            try
            {
                scon.ConnectionString = DashBoard_MVC.Properties.Resources.ConnectionString1.ToString();
                scon.Open();
                scmd.Connection = scon;

                scmd.CommandText = "exec [dbo].[uspGet_Warranty_Name] '" + name + "'";
                
                scmd.Parameters.AddWithValue("@fromData", "1-Jan-2025");
                scmd.Parameters.AddWithValue("@tomData", "1-Jan-2026");
                sdr = scmd.ExecuteReader();

                while (sdr.Read())
                {
                    list.Add(new Insurance()
                    {
                        CustomerName = sdr["CustomerName"].ToString(),
                        Address = sdr["Address"].ToString(),
                        PhoneNo = sdr["PhoneNo"].ToString(),
                        VehicleNo = sdr["VehicleNo"].ToString()
                    }
                    );
                }
                scon.Close();
                return list;
            }
            catch (Exception exc)
            {
                return null;
            }
        }

        private void FetchDataWithName(string name)
        {
            try
            {
                //name = "naval";
                scon.ConnectionString = DashBoard_MVC.Properties.Resources.ConnectionString1.ToString();
                scon.Open();
                scmd.Connection = scon;

                scmd.CommandText = "exec [dbo].[uspGet_Warranty_Name] '" + name + "'";
                scmd.Parameters.AddWithValue("@fromData", "1-Jan-2025");
                scmd.Parameters.AddWithValue("@tomData", "1-Jan-2026");
                sdr = scmd.ExecuteReader();

                while (sdr.Read())
                {
                    insurances.Add(new Insurance()
                    {
                        CustomerName = sdr["CustomerName"].ToString(),
                        Address = sdr["Address"].ToString(),
                        PhoneNo = sdr["PhoneNo"].ToString(),
                        VehicleNo = sdr["VehicleNo"].ToString()
                    }
                    );
                }
                scon.Close();
            }
            catch (Exception exc)
            {

            }
        }

        private void FetchData()
        {
            try
            {
                scon.ConnectionString = DashBoard_MVC.Properties.Resources.ConnectionString1.ToString();
                scon.Open();
                scmd.Connection = scon;
                
                scmd.CommandText = "exec [dbo].[uspGet_Warranty_Notification_DayWise] '1-Jan-2025', '1-Jan-2026'";
                scmd.Parameters.AddWithValue("@fromData", "1-Jan-2025");
                scmd.Parameters.AddWithValue("@tomData", "1-Jan-2026");
                sdr = scmd.ExecuteReader();

                while (sdr.Read())
                {
                    insurances.Add(new Insurance() { CustomerName = sdr["CustomerName"].ToString() ,
                        Address = sdr["Address"].ToString(),
                        PhoneNo = sdr["PhoneNo"].ToString(),
                        VehicleNo = sdr["VehicleNo"].ToString()
                    }                        
                    );
                }
                scon.Close();
            }
            catch (Exception exc)
            {

            }
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}