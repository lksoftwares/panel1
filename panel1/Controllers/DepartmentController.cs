using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using panel1.Classes;
using panel1.Helpers;
using panel1.Middleware;
using Panel1.Classes;
using Panel1.Model;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using LkDataConnection;
//using panel1.Middleware;


namespace Panel1.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {

        private readonly TokenValidator _tokenValidator;
        private readonly Classes.Connection _connection;
        private readonly CheckDuplicacy _duplicacyChecker;
        private readonly InsertMethod _insertMethod;
        private readonly DeleteMethod _deletemethod;
        private LkDataConnection.DataAccess _dc;
        private LkDataConnection.SqlQueryResult _query;


        public DepartmentController(TokenValidator tokenValidator, Classes.Connection connection, InsertMethod insertMethod,DeleteMethod deletemethod)
            
        {
            _tokenValidator = tokenValidator;
            _connection = connection;
            _duplicacyChecker= new CheckDuplicacy(connection);
            _insertMethod = insertMethod;
            _deletemethod = deletemethod;
            DataAccessMethod();
        }

        private void DataAccessMethod()
        {
            LkDataConnection.Connection.ConnectionStr = _connection.GetSqlConnection().ConnectionString;
            LkDataConnection.Connection.Connect();
            _dc = new LkDataConnection.DataAccess();
            _query = new LkDataConnection.SqlQueryResult();
        }

        //public DepartmentController(Connection connection)
        //{
        //    _connection = connection;
        //}

        //[TokenAuthorization]
        //[HttpGet]
        //[Route("GetAllDepartment")]
        //public IActionResult GetAllDepartment()
        //{
        //    var query = $"select * from Department_mst";
        //    DataTable depTable = _connection.ExecuteQueryWithResult(query);
        //    var departmentList = new List<DepartmentModel>();
        //   foreach(DataRow  row in depTable.Rows)
        //    {
        //        departmentList.Add(new DepartmentModel
        //        {
        //            dep_id = Convert.ToInt32(row["dep_id"]),
        //            Department = row["Department"].ToString(),
        //            Short_Name = row["Short_Name"].ToString(),
        //            Description = row["Description"].ToString(),
        //            status = row["status"].ToString()
        //        });
        //    }
        //    return Ok(departmentList);
        //}


        //[TokenValidation]
        [HttpGet]
        [Route("GetDepartment")]
         //[MiddlewareFilter(typeof(TestMiddleware))]
       // [TestMiddleware]
        public IActionResult GetAllDepartment()
        {
            var query = $"select * from Department_mst";
            DataTable depTable = _connection.ExecuteQueryWithResult(query);
            var departmentList = new List<DepartmentModel>();

            foreach (DataRow row in depTable.Rows)
            {
                departmentList.Add(new DepartmentModel
                {
                   // dep_id = Convert.ToInt32(row["dep_id"]),
                    Department = row["Department"].ToString(),
                    Short_Name = row["Short_Name"].ToString(),
                    //Description = row["Description"].ToString(),
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


        //[HttpPost]
        //[Route("AddDepartment")]
        //public IActionResult AddDepartment([FromBody] DepartmentModel department)
        //{
        //    try
        //    {
        //        var duplicacyChecker = new CheckDuplicacy(_connection);

        //        bool isDuplicate = duplicacyChecker.CheckDuplicate("Department_mst",
        //            new[] { "Department", "Short_Name" },
        //            new[] { department.Department, department.Short_Name });

        //        if (isDuplicate)
        //        {
        //            return BadRequest("Department already exists.");
        //        }

        //        string insertDepQuery = $"INSERT INTO Department_mst (Department, Short_Name, Description, status) " +
        //                                $"VALUES ('{department.Department}', '{department.Short_Name}', " +
        //                                $"'{department.Description}', '{department.status}')";

        //        _connection.ExecuteQueryWithoutResult(insertDepQuery);

        //        return Ok("Department Added Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        //    }
        //}

        //[HttpPost]
        //[Route("AddDepartment")]
        //public IActionResult AddDepartment([FromBody] DepartmentModel department)
        //{
        //    try
        //    {
        //       // var duplicacyChecker = new CheckDuplicacy(_connection);

        //        bool isDuplicate = _duplicacyChecker.CheckDuplicate("Department_mst",
        //            new[] { "Department", "Short_Name" },
        //            new[] { department.Department, department.Short_Name });

        //        if (isDuplicate)
        //        {
        //            return BadRequest("Department already exists.");
        //        }

        //        //var insertMethod = new InsertMethod(_connection);
        //        //insertMethod.Insert("Department_mst", new[] { "Department", "Short_Name", "Description", "status" },
        //        //                    new[] { department.Department, department.Short_Name, department.Description, department.status });
        //        DataTable dataTable = new DataTable();
        //        dataTable.Columns.Add("Department");
        //        dataTable.Columns.Add("Short_Name");
        //        //dataTable.Columns.Add("Description");
        //        dataTable.Columns.Add("status");
        //        //dataTable.Rows.Add(department.Department,department.Short_Name,department.Description,department.status);
        //        dataTable.Rows.Add(department.Department, department.Short_Name,  department.status);

        //        _connection.InsertDataTable("Department_mst", dataTable);
        //        return Ok("Department Added Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        //    }
        //}
        //----------------------Correct model------------------
        //public IActionResult AddDepartment([FromBody] DepartmentModel department)
        //{
        //    try
        //    {
        //        _insertMethod.InsertEntityIntoTable(department, "Department_mst");
        //        return Ok("Department Added Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        //    }
        //}
        [HttpPost]
        [Route("AddDepartment")]
        [HttpPost]
        public IActionResult AddDepartment([FromBody] DepartmentModel department)
        {
            try
            {
                //_insertMethod.InsertOrUpdateEntity(department, "Department_mst");
                //LkDataConnection.Connection.ConnectionStr = _connection.GetSqlConnection().ConnectionString;
                //LkDataConnection.Connection.Connect();
                //LkDataConnection.DataAccess _dc = new LkDataConnection.DataAccess();
                //LkDataConnection.SqlQueryResult _query = new LkDataConnection.SqlQueryResult();

                _query = _dc.InsertOrUpdateEntity(department, "Department_mst", -1);

                return Ok("Department Added Successfully");
            }
            catch (Exception ex)
            {
                // Return error response
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
            }
        }

        //[httpput]
        //[route("updatedepartment/{id}")]
        //public iactionresult updatedepartment(int id, [frombody] departmentmodel department)
        //{
        //    string updatedepquery = $"update department_mst set department='{department.department}',short_name='{department.short_name}',description='{department.description}',status='{department.status}' where dep_id='{id}'";
        //    try
        //    {
        //        _connection.executequerywithoutresult(updatedepquery);
        //        return ok("department updated successfully");
        //    }
        //    catch (exception ex)
        //    {
        //        return statuscode(statuscodes.status500internalservererror, $"error{ex.message}");
        //    }
        //}
        [HttpPut]
        [Route("updateDepartment/{dep_id}")]
        public IActionResult UpdateDepartment(int dep_id, [FromBody] DepartmentModel department)
        {
            try
            {
                // Access the dep_id directly from the department object
                //if (dep_id !=department.dep_id)
                //{
                //    return BadRequest("Department ID mismatch.");
                //}

                // Call InsertOrUpdateEntity method of InsertMethod to perform the update

                //_insertMethod.InsertOrUpdateEntity(department, "Department_mst", dep_id,"dep_id");
                _query = _dc.InsertOrUpdateEntity(department, "Department_mst", dep_id,"dep_id");

                return Ok("Department updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }



        //[HttpPut]
        //[Route("updateDepartment/{id}")]
        //public IActionResult UpdateDepartment(int id, [FromBody] DepartmentModel department)
        //{
        //    try
        //    {
        //        //var duplicacyChecker = new CheckDuplicacy(_connection);

        //        bool isDuplicate = _duplicacyChecker.CheckDuplicate("Department_mst",
        //         new[] { "Department", "Short_Name" },
        //         new[] { department.Department, department.Short_Name },
        //         "dep_id");

        //        if (isDuplicate)
        //        {
        //            return BadRequest("Duplicate department exists.");
        //        }

        //        //var updateMethod = new UpdateMethod(_connection);
        //        //updateMethod.Update("Department_mst",
        //        //                    new[] { "Department", "Short_Name", "Description", "status" },
        //        //                    new[] { department.Department, department.Short_Name, department.Description, department.status },
        //        //                    "dep_id", id);
        //        return Ok("department Updated Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error{ex.Message}");
        //    }

        //}
        [AllowAnonymous]
        [HttpDelete]
        [Route("deleteDepartment/{dep_id}")]
        public IActionResult DeleteDepartment(int dep_id)
        {
            //string deleteDepQuery = $"Delete from Department_mst where dep_id='{id}'";
            try
            {
                //   _connection.ExecuteQueryWithoutResult(deleteDepQuery);
                //      var deleteMethod = new DeleteMethod(_connection);
                //deleteMethod.delete("Department_mst",
                //                    "dep_id", id);
                _deletemethod.DeleteEntity("Department_mst", dep_id,"dep_id");

                return Ok("Department Deleted successfully");

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error{ex.Message}");
            }
        }
    }
}
