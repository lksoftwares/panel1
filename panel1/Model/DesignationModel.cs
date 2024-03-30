using System.ComponentModel.DataAnnotations;
using LkDataConnection;

namespace Panel1.Model
{
    public class DesignationModel
    {
        [SkipInsert]
        [Key]
        public int Designation_id { get; set; }
        //[ColumnMapping("Designation")]
        public string Designation { get; set; }
        public string status { get; set; }
    }
}
