using System.ComponentModel.DataAnnotations;

namespace Panel1.Model
{
    public class EmployeeModel
    {
        [Key]
        public int Emp_id { get; set; }
        public string Name { get; set; }
        public string Emp_Code { get; set; }
        public int dep_id { get; set; }
        public int Designation_id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int Contact_No { get; set; }
        public int Alternate_No { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public IFormFile? image { get; set; }
        public string? ImagePath { get; set; }
        public string Bank_Details { get; set; }
        public string Qualification { get; set; }
        public string password { get; set; }
        public int RoleID { get; set; }
        public string JoiningDate { get; set; }
        public string? Pan { get; set; }
        public int? AdharNo { get; set; }
        public string? FamilyId { get; set; }
        public string? SalaryGrade { get; set; }
        public string? status { get; set; }

        public string? PFNo { get; set; }
        public string? ESI_Insurance_No { get; set; }
        public string? DOL { get; set; }
        public string? ImageUrl { get; set; }
    }
}
