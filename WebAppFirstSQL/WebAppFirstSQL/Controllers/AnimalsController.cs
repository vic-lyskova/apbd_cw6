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
        
        reader.Close();

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
        if (animal.Description.Length == 0)
        {
            command.Parameters.AddWithValue("@animalDescription", DBNull.Value);
        }
        else
        {
            command.Parameters.AddWithValue("@animalDescription", animal.Description);
        }
        command.Parameters.AddWithValue("@animalCategory", animal.Category);
        command.Parameters.AddWithValue("@animalArea", animal.Area);

        command.ExecuteNonQuery();

        return Created();
    }

    
    [HttpPut("{idAnimal:int}")]
    public IActionResult UpdateAnimal(int idAnimal, AddAnimal animal)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        using SqlCommand selectToCheckCommand = new SqlCommand();
        selectToCheckCommand.Connection = connection;

        selectToCheckCommand.CommandText = "SELECT * FROM Animal WHERE IdAnimal=" + idAnimal;

        var reader = selectToCheckCommand.ExecuteReader();

        if (!reader.Read())
        {
            return NotFound("No animal with id " + idAnimal);
        }
        
        reader.Close();

        using SqlCommand updateCommand = new SqlCommand();
        updateCommand.Connection = connection;

        updateCommand.CommandText =
            "UPDATE Animal " +
            "SET Name=@animalName, Description=@animalDescription, Category=@animalCategory, Area=@animalArea " +
            "WHERE IdAnimal=" + idAnimal;

        updateCommand.Parameters.AddWithValue("@animalName", animal.Name);
        if (animal.Description.Length == 0)
        {
            updateCommand.Parameters.AddWithValue("@animalDescription", DBNull.Value);
        }
        else
        {
            updateCommand.Parameters.AddWithValue("@animalDescription", animal.Description);
        }
        updateCommand.Parameters.AddWithValue("@animalCategory", animal.Category);
        updateCommand.Parameters.AddWithValue("@animalArea", animal.Area);

        updateCommand.ExecuteNonQuery();

        return NoContent();
    }


    [HttpDelete("{idAnimal:int}")]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        using SqlCommand selectToCheckCommand = new SqlCommand();
        selectToCheckCommand.Connection = connection;

        selectToCheckCommand.CommandText = "SELECT * FROM Animal WHERE IdAnimal=" + idAnimal;

        var reader = selectToCheckCommand.ExecuteReader();

        if (!reader.Read())
        {
            return NotFound("No animal with id " + idAnimal);
        }
        
        reader.Close();

        using SqlCommand deleteCommand = new SqlCommand();
        deleteCommand.Connection = connection;

        deleteCommand.CommandText = "DELETE FROM Animal WHERE IdAnimal=" + idAnimal;
        
        deleteCommand.ExecuteNonQuery();
        
        return NoContent();
    }
}