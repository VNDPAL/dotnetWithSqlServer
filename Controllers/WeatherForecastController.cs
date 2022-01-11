using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace dotnetSQLApi.Controllers;

[ApiController]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IConfiguration _config;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration confing)
    {
        _logger = logger;
        _config = confing;
    }

    [HttpGet]
    [Route("getWeather")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet]
    [Route("GetNew")]
    public JsonResult GetNew()
    {
        string query = @"select * from dbo.testTable";
        DataTable table = new DataTable();
        string SqlDataSource = _config.GetConnectionString("testAppCon");
        SqlDataReader myReader;
        using (SqlConnection con = new SqlConnection(SqlDataSource))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                myReader = cmd.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                con.Close();
            }
        }
        return new JsonResult(table);
    }
}
