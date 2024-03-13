using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panel1.Classes;
using Panel1.Model;
using System.Data;



namespace Panel1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly Connection _connection;

        public DepartmentController(Connection connection)
        {
            _connection = connection;
        }
      

        [HttpGet]
        [Route("GetAllDepartment")]
        public IActionResult GetAllDepartment()
        {
            var query = $"select * from Department_mst";
            DataTable depTable = _connection.ExecuteQueryWithResult(query);
            var departmentList = new List<DepartmentModel>();
           foreach(DataRow  row in depTable.Rows)
            {
                departmentList.Add(new DepartmentModel
                {
                    dep_id = Convert.ToInt32(row["dep_id"]),
                    Department = row["Department"].ToString(),
                    Short_Name = row["Short_Name"].ToString(),
                    Description = row["Description"].ToString(),
                    status = row["status"].ToString()
                });
            }
            return Ok(departmentList);
        }

        //[HttpPost]
        //[Route("AddDepartment")]
        //public IActionResult AddDepartment([FromBody]DepartmentModel department)
        //{
        //    string insertDepQuery = $"insert into Department_mst(Department,Short_Name,Description,status)Values('{department.Department}','{department.Short_Name}','{department.Description}','{department.status}')";
        //    try
        //    {
        //        _connection.ExecuteQueryWithoutResult(insertDepQuery);
        //        return Ok("Department Addded Successfully");
        //    }
        //    catch(Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error:{ex.Message}");
        //    }
        //}


        [HttpPost]
        [Route("AddDepartment")]
        public IActionResult AddDepartment([FromBody] DepartmentModel department)
        {
            try
            {
                var duplicacyChecker = new CheckDuplicacy(_connection);

                bool isDuplicate = duplicacyChecker.CheckDuplicate("Department_mst",
                    new[] { "Department", "Short_Name" },
                    new[] { department.Department, department.Short_Name });

                if (isDuplicate)
                {
                    return BadRequest("Department already exists.");
                }

                string insertDepQuery = $"INSERT INTO Department_mst (Department, Short_Name, Description, status) " +
                                        $"VALUES ('{department.Department}', '{department.Short_Name}', " +
                                        $"'{department.Description}', '{department.status}')";

                _connection.ExecuteQueryWithoutResult(insertDepQuery);

                return Ok("Department Added Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }



        [HttpPut]
        [Route("updateDepartment/{id}")]
        public IActionResult UpdateDepartment(int id ,[FromBody] DepartmentModel department)
        {
            string updatedepQuery = $"Update Department_mst set Department='{department.Department}',Short_Name='{department.Short_Name}',Description='{department.Description}',status='{department.status}' where dep_id='{id}'";
            try
            {
                _connection.ExecuteQueryWithoutResult(updatedepQuery);
                return Ok("department Updated Successfully");
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error{ex.Message}");
            }
        }
        [AllowAnonymous]
        [HttpDelete]
        [Route("deleteDepartment/{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            string deleteDepQuery = $"Delete from Department_mst where dep_id='{id}'";
            try
            {
                _connection.ExecuteQueryWithoutResult(deleteDepQuery);
                return Ok("Department Deleted successfully");

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error{ex.Message}");
            }
        }
    }
}
