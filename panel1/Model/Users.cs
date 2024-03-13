using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Panel.Models
{
    public class Users
    {
        [Key]
        public int User_ID { get; set; }
        public string? emailOrUsername { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? image { get; set; }
        public string? Email { get; set; }
        public int RoleID { get; set; }
        public string? status { get; set; }
        public string? userRole  { get; set; }


    }
}
