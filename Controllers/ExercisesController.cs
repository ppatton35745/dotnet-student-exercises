using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercises.Models;

namespace StudentExercises.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ExercisesController(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get(string language)
        {

            using (IDbConnection conn = Connection)
            {
                string sql = "SELECT * FROM Exercise";

                if (language != null)
                {
                    sql += $" WHERE Language = '{language}'";
                }

                var fullExercises = await conn.QueryAsync<Exercise>(
                    sql);
                return Ok(fullExercises);
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $"SELECT * FROM Exercise WHERE ID = {id}";

                var theSingleExercise = (await conn.QueryAsync<Exercise>(sql)).Single();
                return Ok(theSingleExercise);
            }
        }

        private void Single()
        {
            throw new NotImplementedException();
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Exercise exercise)
        {
            string sql = $@"INSERT INTO Exercise
            (Name, Language)
            VALUES
            ('{exercise.Name}', '{exercise.Language}');
            select MAX(Id) from Exercise";

            using (IDbConnection conn = Connection)
            {
                var newExerciseId = (await conn.QueryAsync<int>(sql)).Single();
                exercise.Id = newExerciseId;
                return CreatedAtRoute("GetExercise", new { id = newExerciseId }, exercise);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
