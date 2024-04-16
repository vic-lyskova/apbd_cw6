using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebAppFirstSQL.DTOs;
using WebAppFirstSQL.Models;

namespace WebAppFirstSQL.Controllers;

[ApiController]
// [Route("api/animals")]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AnimalsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpGet]
    public IActionResult GetAnimals()
    {
        //Open connection
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        //Define command
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM ANIMAL;";

        //Execute command
        var reader = command.ExecuteReader();

        List<Animal> animals = new List<Animal>();

        int idAnimalOriginal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");
        
        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOriginal),
                Name = reader.GetString(nameOrdinal)
            });
        }
            
        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimal animal)
    {
        //Open connection
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        //Define command
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        
        command.CommandText = "INSERT INTO ANIMAL VALUES (@animalName, @, @...);";
        command.Parameters.AddWithValue("@animalName", animal.Name);

        command.ExecuteNonQuery();
        
        return Created("", null);
    }
}