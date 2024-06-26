using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentDemo.Areas.MST_Branch.Models
{
    public class MST_BranchModel
    {
        
        public int? BranchID { get; set; }
        [Required]
        [DisplayName("Branch Name")]
        public string BranchName { get; set; }
        [Required]
        [DisplayName("Branch Code")]
        public string BranchCode { get; set; }

    }
    public class MST_Branch_DropDownModel
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; }
    }
}
