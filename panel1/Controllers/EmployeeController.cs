using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using panel1.Classes;
using Panel1.Classes;
using Panel1.Model;
using System.Data;
using System.Security.Cryptography;
namespace Panel1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly Connection _connection;

        public object CryptoJS { get; private set; }
        private readonly InsertMethod _insertMethod;
        public EmployeeController(Connection connection, InsertMethod insertMethod)
        {
            _connection = connection;
            _insertMethod = insertMethod;
        }


        [HttpGet]
        [Route("GetAllEmployee")]
       public IActionResult GetAllEmployees()
        {
            string getquery = $"select * from Emp_Details";
            try
            {
                DataTable employeeTable = _connection.ExecuteQueryWithResult(getquery);
                var EmployeeList = new List<EmployeeModel>();
                foreach (DataRow row in employeeTable.Rows)
                {
                    EmployeeList.Add(new EmployeeModel
                    {

                        Emp_id = Convert.ToInt32(row["Emp_id"]),
                        Name = row["Name"].ToString(),
                       // Emp_Code = row["Emp_Code"].ToString(),
                        // dep_id =Convert.ToInt32(row["dep_id"]),
                        // Designation_id = Convert.ToInt32(row["Designation_id"]),
                        Address1 = row["Address1"].ToString(),
                        Address2 = row["Address2"].ToString(),
                        //Contact_No = Convert.ToInt32(row["Emp_Code"]),
                        //Alternate_No = Convert.ToInt32(row["Alternate_No"]),
                        DOB = row["DOB"].ToString(),
                        Email = row["Email"].ToString(),
                        Gender = row["Gender"].ToString(),
                        ImagePath = row["image"].ToString(),
                        Bank_Details = row["Bank_Details"].ToString(),
                        Qualification = row["Qualification"].ToString(),
                       // password = row["password"].ToString(),
                        //RoleID = Convert.ToInt32(row["RoleID"]),
                        Pan = row["Pan"].ToString(),
                        //AdharNo = Convert.ToInt32(row["AdharNo"]),
                        // FamilyId = row["FamilyId"].ToString(),
                        //SalaryGrade = row["SalaryGrade"].ToString(),
                        status = row["status"].ToString(),

                        PFNo = row["PFNo"].ToString(),
                        // ESI_Insurance_No = row["ESI_Insurance_No"].ToString(),
                        // DOL = row["DOL"].ToString()



                    });
                }
                return Ok(EmployeeList);

            

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error:{ex.Message}");
            }

        }

        [HttpPost]
        [Route("addEmployee")]
        public IActionResult AddEmployeeDetails([FromForm] EmployeeModel employee)
        {
           // string hashedPassword = HashedPassword.HashPassword(employee.password);

            //string insertquery = $"insert into Emp_Details(Name,Emp_Code,dep_id,Designation_id,Address1,Address2,Contact_No,Alternate_No,DOB,Email,Gender,image,Bank_Details,Qualification,password,RoleID,JoiningDate,Pan,AdharNo,FamilyId,SalaryGrade,status,PFNo,ESI_Insurance_No,DOL)" +
            //    $"" +
            //    $"Values('{employee.Name}','{employee.Emp_Code}','{employee.dep_id}','{employee.Designation_id}','{employee.Address1}','{employee.Address2}','{employee.Contact_No}','{employee.Alternate_No}','{employee.DOB}','{employee.Email}','{employee.Gender}','{employee.image}','{employee.Bank_Details}','{employee.Qualification}','{hashedPassword}','{employee.RoleID}','{employee.JoiningDate}','{employee.Pan}','{employee.AdharNo}','{employee.FamilyId}','{employee.SalaryGrade}','{employee.status}','{employee.PFNo}','{employee.ESI_Insurance_No}','{employee.DOL}')";
            try
            {
                _insertMethod.InsertOrUpdateEntity(employee, "Emp_Details");

                //_connection.ExecuteQueryWithoutResult(insertquery);
                return Ok("Employee Added Successfully");
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error:{ex.Message}");
            }
        }

        //[HttpPost]
        //[Route("addEmployee")]
        //public IActionResult AddEmployeeDetails([FromForm] EmployeeModel employee)
        //{
        //    try
        //    {
        //        if (employee.image != null && employee.image.Length > 0)
        //        {
        //            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(employee.image.FileName);

        //            var filePath = Path.Combine("wwwroot/images", fileName);

        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                employee.image.CopyTo(fileStream);
        //            }

        //            employee.ImagePath = filePath; 
        //        }

        //        string insertquery = $"INSERT INTO Emp_Details(Name, Emp_Code, dep_id, Designation_id, Address1, Address2, Contact_No, Alternate_No, DOB, Email, Gender, image, Bank_Details, Qualification, password, RoleID, JoiningDate, Pan, AdharNo, FamilyId, SalaryGrade, status, PFNo, ESI_Insurance_No, DOL)" +
        //            $"VALUES('{employee.Name}', '{employee.Emp_Code}', '{employee.dep_id}', '{employee.Designation_id}', '{employee.Address1}', '{employee.Address2}', '{employee.Contact_No}', '{employee.Alternate_No}', '{employee.DOB}', '{employee.Email}', '{employee.Gender}', '{employee.ImagePath}', '{employee.Bank_Details}', '{employee.Qualification}', '{employee.password}', '{employee.RoleID}', '{employee.JoiningDate}', '{employee.Pan}', '{employee.AdharNo}', '{employee.FamilyId}', '{employee.SalaryGrade}', '{employee.status}', '{employee.PFNo}', '{employee.ESI_Insurance_No}', '{employee.DOL}')";

        //        _connection.ExecuteQueryWithoutResult(insertquery);

        //        return Ok("Employee Added Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        //    }
        //}

        //[HttpPut]
        //[Route("updateEmployee/{id}")]
        //public IActionResult UpdateDepartment(int id,[FromForm] EmployeeModel employee)
        //{
        //    string hashedPassword = HashedPassword.HashPassword(employee.password);

        //    string updatedepQuery = $"Update Emp_Details set Name='{employee.Name}',Emp_Code='{employee.Emp_Code}',dep_id='{employee.dep_id}',Designation_id='{employee.Designation_id}',Address1='{employee.Address1}',Address2='{employee.Address2}',Contact_No='{employee.Contact_No}',Alternate_No='{employee.Alternate_No}',DOB='{employee.DOB}',Email='{employee.Email}',Gender='{employee.Gender}',image='{employee.image}',Bank_Details='{employee.Bank_Details}',Qualification='{employee.Qualification}',password='{hashedPassword}',RoleID='{employee.RoleID}',JoiningDate='{employee.JoiningDate}',Pan='{employee.Pan}',AdharNo='{employee.AdharNo}',FamilyId='{employee.FamilyId}',SalaryGrade='{employee.SalaryGrade}',status='{employee.status}',PFNo='{employee.PFNo}',ESI_Insurance_No='{employee.ESI_Insurance_No}',DOL='{employee.DOL}' where Emp_id={id}";
        //    try
        //    {
        //        _connection.ExecuteQueryWithoutResult(updatedepQuery);
        //        return Ok("Employee Updated Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error{ex.Message}");
        //    }
        //}


        //[HttpPut]
        //[Route("updateEmployee/{Emp_id}")]
        //public IActionResult UpdateEmployee(int Emp_id, [FromForm] EmployeeModel employee)
        //{
        //    //string hashedPassword = HashedPassword.HashPassword(employee.password);

        //    //if (employee.image != null)
        //    //{
        //    //    employee.ImagePath = ImagesHandler.SaveImage(employee.image);
        //    //}
        //    _insertMethod.InsertOrUpdateEntity(employee, "Emp_Details", Emp_id, "Emp_id");

        //    //string updateEmpQuery = $"UPDATE Emp_Details SET Name='{employee.Name}', Emp_Code='{employee.Emp_Code}', dep_id='{employee.dep_id}', Designation_id='{employee.Designation_id}', Address1='{employee.Address1}', Address2='{employee.Address2}', Contact_No='{employee.Contact_No}', Alternate_No='{employee.Alternate_No}', DOB='{employee.DOB}', Email='{employee.Email}', Gender='{employee.Gender}', image='{employee.ImagePath}', Bank_Details='{employee.Bank_Details}', Qualification='{employee.Qualification}', password='{hashedPassword}', RoleID='{employee.RoleID}', JoiningDate='{employee.JoiningDate}', Pan='{employee.Pan}', AdharNo='{employee.AdharNo}', FamilyId='{employee.FamilyId}', SalaryGrade='{employee.SalaryGrade}', status='{employee.status}', PFNo='{employee.PFNo}', ESI_Insurance_No='{employee.ESI_Insurance_No}', DOL='{employee.DOL}' WHERE Emp_id={id}";

        //    try
        //    {
        //      //  _connection.ExecuteQueryWithoutResult(updateEmpQuery);
        //        return Ok("Employee Updated Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error {ex.Message}");
        //    }
        //}
        [HttpPut]
        [Route("updateEmployee/{Emp_id}")]
        public IActionResult UpdateEmployee(int Emp_id, [FromForm] EmployeeModel employee)
        {
            try
            {
                string oldImagePath = _connection.GetOldImagePathFromDatabase(Emp_id);
              //  Console.WriteLine($"here is the connectionstring:{_connection}");
                if (!string.IsNullOrEmpty(oldImagePath))
                {
                    ImagesHandler.DeleteImage(oldImagePath);
                }
              //  Console.WriteLine($"here is the _insertMethod:{_insertMethod}");

                // Update employee details including handling image upload
                _insertMethod.InsertOrUpdateEntity(employee, "Emp_Details", Emp_id, "Emp_id");

                return Ok("Employee Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error {ex.Message}");
            }
        }

      

        [HttpDelete]
        [Route("deleteEmployee/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            string deleteQuery = $"Delete from Emp_Details where Emp_id='{id}'";
            try
            {
                _connection.ExecuteQueryWithoutResult(deleteQuery);
                return Ok("Employee Deleted successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error{ex.Message}");
            }
        }

    }


   
}


