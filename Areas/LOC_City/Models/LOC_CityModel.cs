using Microsoft.Build.Framework;
using System.ComponentModel;

namespace StudentDemo.Areas.LOC_City.Models
{
    public class LOC_CityModel
    {
        public int? CityID { get; set; }
        [Required]
        [DisplayName("City Name")]
        public string CityName { get; set; }
        [Required]
        [DisplayName("StateID")]
        public int StateID { get; set; }
        [Required]
        [DisplayName("City Code")]
        public string CityCode { get; set; }
        [Required]
        [DisplayName("CountryID")]

        public int CountryID { get; set; }
    }
    public class LOC_City_DropDownModel
    {
        public int CityID { get; set; }
        public string CityName { get; set; }
    }
}
