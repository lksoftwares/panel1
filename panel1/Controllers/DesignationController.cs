using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using panel1.Classes;
using Panel1.Classes;
using Panel1.Model;
using System.Data;

namespace Panel1.Controllers
{
    //[EnableCors("ReactConnection")]

    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        private readonly Connection _connection;
        private readonly InsertMethod _insertMethod;
        private readonly ILogger<DesignationController> _logger;

        public DesignationController(ILogger<DesignationController> logger,Connection connection, InsertMethod insertMethod)
        {
            _logger = logger;
            _connection = connection;
            _insertMethod = insertMethod;
        }
        [HttpGet]
        [Route("getDesignation")]
        public IActionResult GetDesignation()
        {
            string getDesignationQuery = "select * from Designation_mst";
            DataTable designationTable = _connection.ExecuteQueryWithResult(getDesignationQuery);
            var DesignationList = new List<DesignationModel>();
            foreach (DataRow row in designationTable.Rows)
            {
                DesignationList.Add(new DesignationModel
                {
                    Designation_id = Convert.ToInt32(row["Designation_id"]),
                    Designation = row["Designation"].ToString(),
                    status = row["status"].ToString()


                });
            }
            _logger.LogInformation("Designation List ");
            return Ok(DesignationList);

        }


        [HttpPost]
        [Route("addDesignation")] 
        public async Task<IActionResult> AddDesignation([FromBody] DesignationModel designation)
        {
            //string insertquery = $"insert into Designation_mst(Designation, status) Values('{designation.Designation}', '{designation.status}')";

            try
            {
                // await _connection.ExecuteQueryWithoutResultAsync(insertquery);

                //var insertMethod = new InsertMethod(_connection);
                //insertMethod.Insert("Designation_mst", new[] { "Designation",  "status" },
                //                    new[] { designation.Designation,  designation.status });
             //   _insertMethod.InsertEntityIntoTable(designation, "Designation_mst");

                return Ok("Designation Added Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        //[HttpPost]
        //[Route("addDesignation")]
        //public IActionResult AddDesignation([FromBody] DesignationModel designation)
        //{
        //    string insertquery = $"insert into Designation_mst(Designation,status)Values('{designation.Designation}','{designation.status}')";
        //    try
        //    {
        //        _connection.ExecuteQueryWithoutResult(insertquery);
        //        return Ok("Designation Added Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error:{ex.Message}");
        //    }
        //}
        [HttpPut]
        [Route("updateDesignation/{id}")]
        public IActionResult UpdateDesignation(int id, [FromBody] DesignationModel designation)
        {
            try
            {
                var duplicacyChecker = new CheckDuplicacy(_connection);

                bool isDuplicate = duplicacyChecker.CheckDuplicate("Designation_mst",
                 new[] { "Designation" },
                 new[] { designation.Designation },
                 "Designation_id");

                if (isDuplicate)
                {
                    return BadRequest("Duplicate Designation exists.");
                }
                var updateMethod = new UpdateMethod(_connection);
                updateMethod.Update("Designation_mst",
                                    new[] { "Designation", "status" },
                                    new[] { designation.Designation, designation.status },
                                    "Designation_id", id);
                //string updatequery = $"update Designation_mst set Designation='{designation.Designation}',status='{designation.status}' where Designation_id={id}";

                //_connection.ExecuteQueryWithoutResult(updatequery);
                return Ok("Updated Successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error:{ex.Message}");
            }
        }

    }
}
