using System.ComponentModel.DataAnnotations;
using LkDataConnection;

namespace Panel1.Model
{
    public class DepartmentModel
    {
        [Key]
        public int? dep_id { get; set; }
      
        public string Department { get; set; }
        [SkipInsert]
        public string Short_Name { get; set; }
        public string? Description { get; set; }
        public string status { get; set; }


    }
}
