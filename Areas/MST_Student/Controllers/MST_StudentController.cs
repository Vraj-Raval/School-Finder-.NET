using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using StudentDemo.Areas.LOC_City.Models;
using StudentDemo.Areas.LOC_Country.Models;
using StudentDemo.Areas.LOC_State.Models;
using StudentDemo.Areas.MST_Branch.Models;
using StudentDemo.Areas.MST_Student.Models;

namespace StudentDemo.Areas.MST_Student.Controllers
{
    [Area("MST_Student")]
    [Route("MST_Student/{Controller}/{Action}")]
    public class MST_StudentController : Controller
    {
        #region Configuration
        private IConfiguration Configuration;
        public MST_StudentController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region Index
        public IActionResult Index(string? StudentName, string? Email, string? BranchName, bool filter = false)
        {
            string conn = Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection com = new(conn);
            com.Open();
            SqlCommand cmd = com.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (Convert.ToBoolean(filter))
            {
                cmd.CommandText = "PR_StudentFilter";
                cmd.Parameters.AddWithValue("StudentName", StudentName);
                cmd.Parameters.AddWithValue("Email", Email);
                cmd.Parameters.AddWithValue("BranchName", BranchName);
            }
            else
            {
                cmd.CommandText = "PR_Student_SelectAll";
            }
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new();
            dt.Load(reader);
            return View("Index", dt);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int StudentID)
        {
            string str = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Student_DeleteByPK";
            cmd.Parameters.Add("@StudentID", SqlDbType.Int).Value = StudentID;
            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                TempData["message"] = "Data Delete Done";
            }
            conn.Close();
            return RedirectToAction("Index");
        }
        #endregion

        #region Create
        public IActionResult Create(int? StudentID)
        {
            #region ComboBox

            string connstr = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn1 = new SqlConnection(connstr);
            conn1.Open();
            SqlCommand cmd1 = conn1.CreateCommand();
            SqlCommand cmd2 = conn1.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "PR_Branch_SelectByComboBox";
            cmd2.CommandText = "PR_City_SelectByComboBox";
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            dt1.Load(sdr1);
            sdr1.Close(); 

            SqlDataReader sdr2 = cmd2.ExecuteReader(); 
            dt2.Load(sdr2);
            sdr2.Close(); 
            conn1.Close();

            List<MST_Branch_DropDownModel> list = new List<MST_Branch_DropDownModel>();
            List<LOC_City_DropDownModel> list1 = new List<LOC_City_DropDownModel>();

            foreach (DataRow dr in dt1.Rows)
            {
                MST_Branch_DropDownModel vlst = new MST_Branch_DropDownModel();
                vlst.BranchID = Convert.ToInt32(dr["BranchID"]);
                vlst.BranchName = Convert.ToString(dr["BranchName"]);
                list.Add(vlst);
            }

            foreach (DataRow dr in dt2.Rows)
            {
                LOC_City_DropDownModel vlst1 = new LOC_City_DropDownModel();
                vlst1.CityID = Convert.ToInt32(dr["CityID"]);
                vlst1.CityName = Convert.ToString(dr["CityName"]);
                list1.Add(vlst1);
            }

            ViewBag.BranchList = list;
            ViewBag.CityList = list1;
            #endregion



            #region SelectByPK
            if (StudentID != null)
            {
                string str = this.Configuration.GetConnectionString("myConnectionStrings");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Student_SelectByPK";
                cmd.Parameters.Add("@StudentID", SqlDbType.Int).Value = StudentID;
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                conn.Close();
                MST_StudentModel modelMST_Student = new MST_StudentModel();
                foreach (DataRow dr in dt.Rows)
                {

                    modelMST_Student.StudentID = Convert.ToInt32(dr["StudentID"]);
                    modelMST_Student.StudentName = Convert.ToString(dr["StudentName"]);
                    modelMST_Student.BranchID = Convert.ToInt32(dr["BranchID"]);
                    modelMST_Student.CityID = Convert.ToInt32(dr["CityID"]);
                    modelMST_Student.MobileNoStudent = Convert.ToString(dr["MobileNoStudent"]);
                    modelMST_Student.MobileNoFather = Convert.ToString(dr["MobileNoFather"]);
                    modelMST_Student.Address = Convert.ToString(dr["Address"]);
                    modelMST_Student.Email = Convert.ToString(dr["Email"]);
                    modelMST_Student.Gender = Convert.ToString(dr["Gender"]);
                    modelMST_Student.IsActive = Convert.ToString(dr["IsActive"]);
                    modelMST_Student.age = Convert.ToInt32(dr["age"]);
                    modelMST_Student.BirthDate = Convert.ToDateTime(dr["BirthDate"]);
                    
                    modelMST_Student.Password = Convert.ToString(dr["Password"]);

                }
                return View("Create", modelMST_Student);
            }
            return View("Create");

            #endregion


        }
        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(MST_StudentModel modelMST_Student)
        {
            string str = this.Configuration.GetConnectionString("myConnectionStrings");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            
            if (modelMST_Student.StudentID == null)
            {
                cmd.CommandText = "PR_Student_Insert";

            }
            else
            {
                cmd.CommandText = "PR_Student_UpdateByPK";
                cmd.Parameters.Add("@StudentID", SqlDbType.Int).Value = modelMST_Student.StudentID;

            }

            cmd.Parameters.Add("@StudentName", SqlDbType.VarChar).Value = modelMST_Student.StudentName;
            cmd.Parameters.Add("@BranchID", SqlDbType.VarChar).Value = modelMST_Student.BranchID;
            cmd.Parameters.Add("@CityID", SqlDbType.VarChar).Value = modelMST_Student.CityID;
            cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = modelMST_Student.Email;
            cmd.Parameters.Add("@MobileNoStudent", SqlDbType.VarChar).Value = modelMST_Student.MobileNoStudent;
            cmd.Parameters.Add("@MobileNoFather", SqlDbType.VarChar).Value = modelMST_Student.MobileNoFather;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = modelMST_Student.Address;
            cmd.Parameters.Add("@Gender", SqlDbType.VarChar).Value = modelMST_Student.Gender;
            cmd.Parameters.Add("@BirthDate", SqlDbType.VarChar).Value = modelMST_Student.BirthDate;
            cmd.Parameters.Add("@IsActive", SqlDbType.VarChar).Value = modelMST_Student.IsActive;
            cmd.Parameters.Add("@age", SqlDbType.VarChar).Value = modelMST_Student.age;
            cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = modelMST_Student.Password;
           


            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelMST_Student.StudentID == null)
                {
                    TempData["Success1"] = ("Student Added Successfully");

                }
                else
                {
                    TempData["Success2"] = ("Student Updated Successfully");

                }

            }
            
            conn.Close();

            return RedirectToAction("Index");
        }
        #endregion
    }
}
