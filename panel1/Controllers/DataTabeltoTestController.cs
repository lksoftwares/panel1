using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using panel1.Model;
using System.Data;
using panel1.Controllers;
using Panel1.Classes;

namespace panel1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataTabeltoTestController : ControllerBase
    {
        private readonly Connection _connection;
        public DataTabeltoTestController(Connection connection)
        {
            _connection = connection;
        }
        [HttpGet]
        public ActionResult<IEnumerable<DTTL>> Get()
        {
            DataTable dataTable = GetDesignation();

            List<DataRow> dataList = dataTable.AsEnumerable().ToList();


            List<DTTL> customList = dataList.Select(row => new DTTL
            {
                Designation_id = row.Field<int>("Designation_id"),
                Designation = row.Field<string>("Designation"),
                status = row.Field<string>("status"),
            }).ToList();

            return Ok (customList);
        }

        private DataTable GetDesignation()
        {
            string getDesignationQuery = "select * from Designation_mst";
            DataTable designationTable = _connection.ExecuteQueryWithResult(getDesignationQuery);

            return designationTable;
        }
    }
}
