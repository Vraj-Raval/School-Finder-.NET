using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

using StudentDemo.Areas.LOC_State.Models;
using StudentDemo.Areas.LOC_Country.Models;

namespace StudentDemo.Areas.LOC_State.Controllers
{
    [Area("LOC_State")]
    [Route("LOC_State/{Controller}/{Action}")]
    public class LOC_StateController : Controller
    {
        #region Configuration
        private IConfiguration Configuration;
        public LOC_StateController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region Index
        public IActionResult Index(string? StateName, string? StateCode, bool filter = false)
        {
            string MyCONNstr = this.Configuration.GetConnectionString("myConnectionStrings"); DataTable dtable = new DataTable();
            SqlConnection SQLConn = new SqlConnection(MyCONNstr); SQLConn.Open();
            SqlCommand cmd = SQLConn.CreateCommand(); cmd.CommandType = CommandType.StoredProcedure;
            if (Convert.ToBoolean(filter))
            {
                cmd.CommandText = "PR_State_Search"; cmd.Parameters.AddWithValue("@StateName", StateName);
                cmd.Parameters.AddWithValue("@StateCode", StateCode);
            }
            else
            {
                cmd.CommandText = "PR_State_SELECT_ALL";
            }
            SqlDataReader objStr = cmd.ExecuteReader();
            dtable.Load(objStr);
            return View("Index", dtable);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int StateID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_STATE_DELETE";
            cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = StateID;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                TempData["message"] = "Data Delete Done";
            }
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion

        #region Create
        public IActionResult Create(int? StateID)
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
            foreach(DataRow dr in dt1.Rows)
            {
                LOC_Country_DropDownModel vlst = new LOC_Country_DropDownModel();
                vlst.CountryID = Convert.ToInt32(dr["CountryID"]);
                vlst.CountryName = Convert.ToString(dr["CountryName"]);
                list.Add(vlst);

            }
            ViewBag.CountryList = list;

            #endregion

            #region SelectBYPK
            if (StateID != null)
            {
                string str = this.Configuration.GetConnectionString("myConnectionStrings");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_STATE_SELECT_BY_PRIMARY_KEY";
                cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = StateID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                conn.Close();
                LOC_StateModel modelLOC_State = new LOC_StateModel();
                foreach (DataRow dr in dt.Rows)
                {

                    modelLOC_State.CountryID = Convert.ToInt32(dr["CountryID"]);
                    modelLOC_State.StateName = Convert.ToString(dr["StateName"]);
                    modelLOC_State.StateCode = Convert.ToString(dr["StateCode"]);
                    modelLOC_State.StateID = Convert.ToInt32(dr["StateID"]);
                }
                return View("Create", modelLOC_State);
            }
            return View("Create");
            #endregion
        }
        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(LOC_StateModel modelLOC_State)
        {
            string str = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (modelLOC_State.StateID == null)
            {
                cmd.CommandText = "PR_STATE_INSERT";

            }
            else
            {
                cmd.CommandText = "PR_STATE_UPDATE";
                cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = modelLOC_State.StateID;

            }
            cmd.Parameters.Add("@StateName", SqlDbType.VarChar).Value = modelLOC_State.StateName;
            cmd.Parameters.Add("@StateCode", SqlDbType.VarChar).Value = modelLOC_State.StateCode;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = modelLOC_State.CountryID;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelLOC_State.StateID == null)
                {
                    TempData["Success1"] = ("State Added Successfully");

                }
                else
                {
                    TempData["Success2"] = ("State Updated Successfully");

                }

            }
            conn.Close();

            return RedirectToAction("Index");
        }
        #endregion

    }
}
