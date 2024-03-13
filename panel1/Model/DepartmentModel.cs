using System.ComponentModel.DataAnnotations;

namespace Panel1.Model
{
    public class DepartmentModel
    {
        [Key]
        public int dep_id { get; set; }
        public string Department { get; set; }
        public string Short_Name { get; set; }
        public string Description { get; set; }
        public string status { get; set; }


    }
}
