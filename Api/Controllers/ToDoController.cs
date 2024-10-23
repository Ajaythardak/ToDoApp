using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication1.Controllers
{
    
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private IConfiguration _configuration;
        public ToDoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get_tasks")]
        public JsonResult get_tasks() 
        {
            string query = "select * from todo";
            DataTable table = new DataTable();
            string SqlDatasource = _configuration.GetConnectionString("mydb");
            MySqlDataReader myReader;
            using (MySqlConnection myCon = new MySqlConnection(SqlDatasource)) 
            {
                myCon.Open();
                using(MySqlCommand myCommand = new MySqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); 
                }
            }
            return new JsonResult(table);
        }

        [HttpPost("add_task")]
        public JsonResult add_task([FromForm] string task)
        {
            string query = "insert into todo (taskName) values (@task)";
            DataTable table = new DataTable();
            string SqlDatasource = _configuration.GetConnectionString("mydb");
            MySqlDataReader myReader;
            using (MySqlConnection myCon = new MySqlConnection(SqlDatasource))
            {
                myCon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@task", task);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                } 
            }
            return new JsonResult("Task Added Successfully");
        }

        [HttpPost("remove_task")]
        public JsonResult remove_task([FromForm] string id)
        {
            string query = "delete from todo where id =@id";
            DataTable table = new DataTable();
            string SqlDatasource = _configuration.GetConnectionString("mydb");
            MySqlDataReader myReader;
            using (MySqlConnection myCon = new MySqlConnection(SqlDatasource))
            {
                myCon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                }
            }
            return new JsonResult("Task Deleted Successfully.");
        }

    }
}
