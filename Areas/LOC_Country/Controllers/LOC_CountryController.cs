using Microsoft.AspNetCore.Mvc;
using StudentDemo.Areas.LOC_Country.Models;
using System.Data;
using System.Data.SqlClient;

namespace StudentDemo.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    [Route("LOC_Country/{Controller}/{Action}")]
    public class LOC_CountryController : Controller
    {
        #region Configuration
        private IConfiguration Configuration;
        public LOC_CountryController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region Index
        
        #endregion
        public IActionResult Index(string? CountryName, string? CountryCode, bool filter = false)
        {
            string MyCONNstr = this.Configuration.GetConnectionString("myConnectionStrings");
            DataTable dtable = new DataTable();
            SqlConnection SQLConn = new SqlConnection(MyCONNstr); SQLConn.Open();
            SqlCommand cmd = SQLConn.CreateCommand(); cmd.CommandType = CommandType.StoredProcedure;
            if (Convert.ToBoolean(filter))
            {
                cmd.CommandText = "PR_Country_Search"; cmd.Parameters.AddWithValue("@CountryName", CountryName);
                cmd.Parameters.AddWithValue("@CountryCode", CountryCode);
            }
            else
            {
                cmd.CommandText = "PR_COUNTRY_SELECT_ALL";
            }
            SqlDataReader objStr = cmd.ExecuteReader();
            dtable.Load(objStr);
            return View("Index", dtable);
        }

        #region Delete
        public IActionResult Delete(int CountryID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType=CommandType.StoredProcedure;
            cmd.CommandText = "PR_COUNTRY_DELETE";
            cmd.Parameters.Add("@CountryID",SqlDbType.Int).Value = CountryID;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                TempData["message"] = "Data Delete Done";
            }
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion

        #region Create
        public IActionResult Create(int? CountryID)
        {
            if(CountryID != null)
            {
                string str = this.Configuration.GetConnectionString("myConnectionStrings");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_COUNTRY_SELECT_BY_PRIMARY_KEY";
                cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                conn.Close();
                LOC_CountryModel modelLOC_Country = new LOC_CountryModel();
                foreach (DataRow dr in dt.Rows)
                {
                    
                    modelLOC_Country.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_Country.CountryName = Convert.ToString(dr["CountryName"]);
                    modelLOC_Country.CountryCode = Convert.ToString(dr["CountryCode"]);
                }
                return View("Create", modelLOC_Country);




            }
            
            
                return View("Create");
            
            
        }
        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(LOC_CountryModel modelLOC_Country)
        {
            string str = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (modelLOC_Country.CountryID == null)
            {
                cmd.CommandText = "PR_COUNTRY_INSERT";

            }
            else
            {
                cmd.CommandText = "PR_COUNTRY_UPDATE";
                cmd.Parameters.Add("@CountryID",SqlDbType.Int).Value=modelLOC_Country.CountryID;

            }
            cmd.Parameters.Add("@CountryName", SqlDbType.VarChar).Value = modelLOC_Country.CountryName;
            cmd.Parameters.Add("@CountryCode", SqlDbType.VarChar).Value = modelLOC_Country.CountryCode;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_Country.CountryID == null)
                {
                    TempData["Success1"] = ("Country Added Successfully");

                }
                else
                {
                    TempData["Success2"] = ("Country Updated Successfully");

                }

            }
            conn.Close();
            
            return RedirectToAction("Index");
        }
        #endregion

    }
}
    