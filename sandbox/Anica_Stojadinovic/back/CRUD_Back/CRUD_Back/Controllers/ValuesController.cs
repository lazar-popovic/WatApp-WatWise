using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {


        private readonly List<Student> students = new List<Student>
        {
        new Student { id = 1, name = "Anica Stojadinovic", numberi = "30-2016" },
        new Student { id = 2, name = "Stefan Stijovic", numberi = "20-2016" },
        new Student { id = 3, name = "Rea Spasojkic", numberi = "27-2015" }
        };

        [HttpGet]
        public IEnumerable<Student> GetStudents()
        {
            return students;
        }

        [HttpPost]
        public void Create(Student student)
        {
            student.id = students.Count + 1;
            students.Add(student);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = students.FirstOrDefault(u => u.id == id);

            if (student == null)
            {
                return NotFound();
            }

            students.Remove(student);

            return NoContent();
        }


    }

   
}
