using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Http;
using StudentExercises.Models;

namespace StudentInstructors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly IConfiguration _config;

        public InstructorsController(IConfiguration config)
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

        // GET api/Instructor?language=JavaScript
        [HttpGet]
        public async Task<IActionResult> Get(string language)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = "SELECT * FROM Instructor";

                if (language != null)
                {
                    sql += $" WHERE Language='{language}'";
                }

                var fullInstructors = await conn.QueryAsync<Instructor>(sql);
                return Ok(fullInstructors);
            }

        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetInstructor")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $"SELECT * FROM Instructor WHERE Id = {id}";

                var theSingleInstructor = (await conn.QueryAsync<Instructor>(sql)).Single();
                return Ok(theSingleInstructor);
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Instructor Instructor)
        {
            string sql = $@"INSERT INTO Instructor
            (FirstName, LastName, SlackHandle, Specialty, CohortId)
            VALUES
            (
                '{Instructor.FirstName}'
                , '{Instructor.LastName}'
                , '{Instructor.SlackHandle}'
                , '{Instructor.Specialty}'
                , '{Instructor.CohortId}'
            );
            select MAX(Id) from Instructor";

            using (IDbConnection conn = Connection)
            {
                var newInstructorId = (await conn.QueryAsync<int>(sql)).Single();
                Instructor.Id = newInstructorId;
                return CreatedAtRoute("GetInstructor", new { id = newInstructorId }, Instructor);
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Instructor Instructor)
        {
            string sql = $@"
            UPDATE Instructor
            SET FirstName = '{Instructor.FirstName}',
                LastName = '{Instructor.LastName}',
                SlackHandle = '{Instructor.SlackHandle}',
                Specialty = '{Instructor.Specialty}',
                CohortId = {Instructor.CohortId}
            WHERE Id = {id}";

            try
            {
                using (IDbConnection conn = Connection)
                {
                    int rowsAffected = await conn.ExecuteAsync(sql);
                    if (rowsAffected > 0)
                    {
                        return new StatusCodeResult(StatusCodes.Status204NoContent);
                    }
                    throw new Exception("No rows affected");
                }
            }
            catch (Exception)
            {
                if (!InstructorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            string sql = $@"DELETE FROM Instructor WHERE Id = {id}";

            using (IDbConnection conn = Connection)
            {
                int rowsAffected = await conn.ExecuteAsync(sql);
                if (rowsAffected > 0)
                {
                    return new StatusCodeResult(StatusCodes.Status204NoContent);
                }
                throw new Exception("No rows affected");
            }

        }

        private bool InstructorExists(int id)
        {
            string sql = $@"SELECT Id
                                ,FirstName
                                ,LastName
                                ,SlackHandle
                                ,Specialty
                                ,CohortId 
                            FROM Instructor 
                            WHERE Id = {id}";
            using (IDbConnection conn = Connection)
            {
                return conn.Query<Instructor>(sql).Count() > 0;
            }
        }
    }
}