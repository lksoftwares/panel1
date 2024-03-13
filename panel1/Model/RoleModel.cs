using panel1.Helpers;

namespace panel1.Model
{
    public class RoleModel
    {
        public int? RoleID { get; set; }

        //[MyCustomValidation(Text = "new", ErrorMessage = "Error from Model Class...")]
        //[MyCustomValidation(Text = "new", ErrorMessage = "Error from Model Class...")]

        public string RoleName { get; set; }
    }
}
