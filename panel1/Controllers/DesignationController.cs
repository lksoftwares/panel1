using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public DesignationController(Connection connection)
        {
            _connection = connection;
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
            return Ok(DesignationList);

        }
        [HttpPost]
        [Route("addDesignation")] 
        public async Task<IActionResult> AddDesignation([FromBody] DesignationModel designation)
        {
            string insertquery = $"insert into Designation_mst(Designation, status) Values('{designation.Designation}', '{designation.status}')";

            try
            {
                await _connection.ExecuteQueryWithoutResultAsync(insertquery);
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
            string updatequery = $"update Designation_mst set Designation='{designation.Designation}',status='{designation.status}' where Designation_id={id}";
            try
            {
                _connection.ExecuteQueryWithoutResult(updatequery);
                return Ok("Updated Successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error:{ex.Message}");
            }
        }

    }
}

//using Microsoft.AspNetCore.Mvc;
//using Panel1.Classes;
//using System;
//using System.Data;
//using Newtonsoft.Json;

//[Route("api/[controller]")]
//[ApiController]
//public class DesignationController : ControllerBase
//{
//    private readonly Connection _connection;
//    private readonly object JsonConvert;

//    public DesignationController(Connection connection)
//    {
//        _connection = connection;
//    }

//    [HttpGet]
//    [Route("getDesignation")]
//    public IActionResult GetDesignation()
//    {
//        string getDesignationQuery = "select * from Designation_mst";
//        DataTable designationTable = _connection.ExecuteQueryWithResult(getDesignationQuery);

//        string jsonResult = JsonConvert.SerializeObject(designationTable);

//        return Content(jsonResult, "application/json");
//    }
//}


