using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using StudentDemo.Areas.LOC_City.Models;
using StudentDemo.Areas.LOC_Country.Models;
using StudentDemo.Areas.LOC_State.Models;

namespace StudentDemo.Areas.LOC_City.Controllers
{
    [Area("LOC_City")]
    [Route("LOC_City/{Controller}/{Action}")]
    public class LOC_CityController : Controller
    {
        #region Configuration
        private IConfiguration Configuration;
        public LOC_CityController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region Index
        public IActionResult Index(string? CityName, string? CityCode)
        {
            string MyCONNstr = this.Configuration.GetConnectionString("myConnectionStrings"); DataTable dtable = new DataTable();
            SqlConnection SQLConn = new SqlConnection(MyCONNstr); SQLConn.Open();
            SqlCommand cmd = SQLConn.CreateCommand(); cmd.CommandType = CommandType.StoredProcedure;
            if (Convert.ToBoolean(filter))
            {
                cmd.CommandText = "PR_CityFilter"; cmd.Parameters.AddWithValue("@CityName", CityName);
                cmd.Parameters.AddWithValue("@CityCode", CityCode);
            }
            else
            {
                cmd.CommandText = "PR_CITY_SELECT_ALL";
            }
            SqlDataReader objStr = cmd.ExecuteReader();
            dtable.Load(objStr);
            return View("Index", dtable);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int CityID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_CITY_DELETE";
            cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                TempData["message"] = "Data Delete Done";
            }
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion

        #region Create
        public IActionResult Create(int? CityID)
        {
            #region ComboBox

            string connstr = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn1 = new SqlConnection(connstr);
            conn1.Open();
            SqlCommand cmd1 = conn1.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "Pr_Country_SelectByComboBox";
            DataTable dt1 = new DataTable();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            dt1.Load(sdr1);
            conn1.Close();
            List<LOC_Country_DropDownModel> list = new List<LOC_Country_DropDownModel>();
            foreach (DataRow dr in dt1.Rows)
            {
                LOC_Country_DropDownModel vlst = new LOC_Country_DropDownModel();
                vlst.CountryID = Convert.ToInt32(dr["CountryID"]);
                vlst.CountryName = Convert.ToString(dr["CountryName"]);
                list.Add(vlst);
            }
            ViewBag.CountryList = list;
            List<LOC_State_DropDownModel> list1 = new List<LOC_State_DropDownModel>();
            ViewBag.StateList = list1;

            #endregion

            #region SelectByPK
            if (CityID != null)
            {
                string str = this.Configuration.GetConnectionString("myConnectionStrings");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_CITY_SELECT_BY_PRIMARY_KEY";
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                conn.Close();
                LOC_CityModel modelLOC_City = new LOC_CityModel();
                foreach (DataRow dr in dt.Rows)
                {

                    modelLOC_City.CityID = Convert.ToInt32(dr["CityID"]);
                    modelLOC_City.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_City.StateID = Convert.ToInt32(dr["StateID"]);
                    modelLOC_City.CityName = Convert.ToString(dr["CityName"]);
                    modelLOC_City.CityCode = Convert.ToString(dr["CityCode"]);
                    
                }
                return View("Create", modelLOC_City);
            }
            return View("Create");

            #endregion
        }
        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(LOC_CityModel modelLOC_City)
        {
            string str = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (modelLOC_City.CityID == null)
            {
                cmd.CommandText = "PR_CITY_INSERT";

            }
            else
            {
                cmd.CommandText = "PR_CITY_UPDATE";
                cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = modelLOC_City.CityID;

            }
            cmd.Parameters.Add("@CityName", SqlDbType.VarChar).Value = modelLOC_City.CityName;
            cmd.Parameters.Add("@CityCode", SqlDbType.VarChar).Value = modelLOC_City.CityCode;
            cmd.Parameters.Add("@CountryID", SqlDbType.VarChar).Value = modelLOC_City.CountryID;
            cmd.Parameters.Add("@StateID", SqlDbType.VarChar).Value = modelLOC_City.StateID;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_City.CityID == null)
                {
                    TempData["Success1"] = ("City Added Successfully");

                }
                else
                {
                    TempData["Success2"] = ("City Updated Successfully");

                }

            }
            conn.Close();

            return RedirectToAction("Index");
        }
        #endregion

        #region DropdownByCountry
        [HttpPost]
        public IActionResult DropdownByCountry(int CountryID)
        {

            string connstr = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn1 = new SqlConnection(connstr);
            conn1.Open();
            SqlCommand cmd1 = conn1.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "Pr_State_SelectByComboBox";
            cmd1.Parameters.AddWithValue("CountryID", CountryID);
            DataTable dt1 = new DataTable();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            dt1.Load(sdr1);
            sdr1.Close();
            conn1.Close();
            List<LOC_State_DropDownModel> list = new  List<LOC_State_DropDownModel>();
            foreach(DataRow dr in dt1.Rows)
            {
                LOC_State_DropDownModel vlst = new LOC_State_DropDownModel();
                vlst.StateID = Convert.ToInt32(dr["StateID"]);
                vlst.StateName = Convert.ToString(dr["StateName"]);
                list.Add(vlst);
            }
            var vModel = list;
            return Json(vModel);

        }
        #endregion
    }
}
