
using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using panel1.Controllers;
using Panel1.Classes;
namespace panel1.Classes
{
    [ApiController]
    [Route("[controller]")]
    public class jsontestController : ControllerBase
    {
        private readonly Panel1.Classes.Connection _connection;
        public jsontestController(Connection connection )
        {
            _connection = connection;
        }
        [HttpGet]
        public ActionResult<string> GetJson()
        {
           
           DataTable dt = GetDesignation();

            string json = DataTableToJson(dt);

            return Ok(json);
        }
        private DataTable GetDesignation()
        {
            string getDesignationQuery = "select * from Designation_mst";
            DataTable designationTable = _connection.ExecuteQueryWithResult(getDesignationQuery);
            return designationTable;
        }
        private string DataTableToJson(DataTable dt)
        {
            string json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            return json;
        }
    }
}
