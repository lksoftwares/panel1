using panel1.Classes;
using System.ComponentModel.DataAnnotations;

namespace Panel1.Model
{
    public class DesignationModel
    {
        [SkipInsert]
        [Key]
        public int Designation_id { get; set; }
        public string Designation { get; set; }
        public string status { get; set; }
    }
}
