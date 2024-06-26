using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentDemo.Areas.LOC_State.Models
{
    public class LOC_StateModel
    {
        
        public int? StateID { get; set; }
        [Required]
        [DisplayName("State Name")]
        public string StateName { get; set; }
        [Required]
        [DisplayName("State Code")]
        public string StateCode  { get; set; }
        [Required]
        public int CountryID { get; set; }
    }
    public class LOC_State_DropDownModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
    }
}
