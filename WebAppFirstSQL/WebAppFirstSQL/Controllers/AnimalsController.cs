using System.Data;
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
    public IActionResult GetAnimals(string orderBy = "name")
    {
        switch (orderBy.ToLower())
        {
            case "name": break;
            case "description": break;
            case "category": break;
            case "area": break;
            default:
                return BadRequest();
        }

        //Open connection
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        //Define command
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM ANIMAL ORDER BY " + orderBy;

        //Execute command
        var reader = command.ExecuteReader();

        var animals = new List<Animal>();

        var idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        var nameOrdinal = reader.GetOrdinal("Name");
        var descriptionOrdinal = reader.GetOrdinal("Description");
        var categoryOrdinal = reader.GetOrdinal("Category");
        var areaOrdinal = reader.GetOrdinal("Area");

        while (reader.Read())
        {
            string? description = reader.IsDBNull(descriptionOrdinal) ? null : reader.GetString(descriptionOrdinal);

            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOrdinal),
                Name = reader.GetString(nameOrdinal),
                Description = description,
                Category = reader.GetString(categoryOrdinal),
                Area = reader.GetString(areaOrdinal)
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

        command.CommandText = "INSERT INTO ANIMAL VALUES (@animalName, @animalDescription, @animalCategory, @animalArea);";
        
        command.Parameters.AddWithValue("@animalName", animal.Name);
        command.Parameters.AddWithValue("@animalDescription", animal.Description);
        command.Parameters.AddWithValue("@animalCategory", animal.Category);
        command.Parameters.AddWithValue("@animalArea", animal.Area);

        command.ExecuteNonQuery();

        return Created();
    }
}