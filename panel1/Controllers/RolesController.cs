using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using panel1.Model;
using Panel1.Classes;
using Panel1.Model;
using System.Data;



namespace Panel1.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly Connection _connection;

        public RolesController(Connection connection)
        {
            _connection = connection;
        }


        [HttpGet]
        [Route("getallrole")]
        public IActionResult GetAllRole()
        {
            var query = $"select * from Roles_R";
            DataTable depTable = _connection.ExecuteQueryWithResult(query);
            var RoleList = new List<RoleModel>();
            foreach (DataRow row in depTable.Rows)
            {
                RoleList.Add(new RoleModel
                {
                    RoleID = Convert.ToInt32(row["RoleID"]),
                    RoleName = row["RoleName"].ToString(),
                 
                });
            }
            return Ok(RoleList);
        }

          [HttpPost]
        [Route("AddRole")]
        public IActionResult AddRole([FromBody] RoleModel role)
        {
            try
            {
                var duplicacyChecker = new CheckDuplicacy(_connection);

                bool isDuplicate = duplicacyChecker.CheckDuplicate("Roles_R",
                    new[] { "RoleName" },
                    new[] { role.RoleName });

                if (isDuplicate)
                {
                    return BadRequest("RoleName already exists.");
                }

                //string insertRoleQuery = $"INSERT INTO Roles_R (RoleName) " +
                //                        $"VALUES ('{role.RoleName}')";
              //  _connection.ExecuteQueryWithoutResult(insertRoleQuery);

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("RoleName");
                dataTable.Rows.Add(role.RoleName);

                _connection.InsertDataTable("Roles_R", dataTable);


                return Ok("RoleName Added Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }



        [HttpPut]
        [Route("updateRole/{id}")]
        public IActionResult UpdateRole(int id, [FromBody] RoleModel role)
        {
            string updateRoleQuery = $"Update Roles_R set RoleName='{role.RoleName}' where RoleID='{id}'";
            try
            {
                var duplicacyChecker = new CheckDuplicacy(_connection);

                bool isDuplicate = duplicacyChecker.CheckDuplicate("Designation_mst",
                 new[] { "RoleName" },
                 new[] { role.RoleName },
                 "RoleID");

                if (isDuplicate)
                {
                    return BadRequest("Duplicate Rolename exists.");
                }
                _connection.ExecuteQueryWithoutResult(updateRoleQuery);
                return Ok("RoleName Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error{ex.Message}");
            }
        }
        
        [AllowAnonymous]
        [HttpDelete]
        [Route("deleteRole/{id}")]
        public IActionResult DeleteRoleName(int id)
        {
            string deleteDepQuery = $"Delete from Roles_R where RoleID='{id}'";
            try
            {
                _connection.ExecuteQueryWithoutResult(deleteDepQuery);
                return Ok("RoleName Deleted successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error{ex.Message}");
            }
        }
    }
}
