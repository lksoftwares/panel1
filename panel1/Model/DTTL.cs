using System.ComponentModel.DataAnnotations;

namespace panel1.Model
{
    public class DTTL
    {
        [Key]
        public int Designation_id { get; set; }
        public string Designation { get; set; }
        public string status { get; set; }

        internal static object OrderBy(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }
}
