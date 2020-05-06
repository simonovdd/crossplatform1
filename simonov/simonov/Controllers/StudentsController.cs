using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using simonov.models;
using Microsoft.AspNetCore.Authorization;

namespace simonov.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly Context _context;

        public StudentsController(Context context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public IEnumerable<Student> GetStudents()
        {
            return _context.getAllStudents().ToList();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public Student GetStudent(long id)
        {
            return _context.getAllStudents().FirstOrDefault(s => s.Id == id);
        }

        [HttpGet("Headman")]
        [Authorize]
        public IEnumerable<Student> GetHeadmans()
        {
            return _context.getHeadmans();

        }

        [HttpGet("Find/{name}")]
        [Authorize]
        public IEnumerable<string> FindStudent(string name)
        {
            return _context.getStudentsGroup(name);
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(long id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Student>> PostStudent(Student student, long id)
        {
            var G = await _context.Groups.FindAsync(id);

            if (G == null)
                return BadRequest();

            G.Students.Add(student);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(long id)
        {
            var student = _context.getAllStudents().FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Groups.Where(g => g.Students.FirstOrDefault(s => s.Id == id) != null).FirstOrDefault().Students.Remove(student);
            await _context.SaveChangesAsync();

            return student;
        }

        private bool StudentExists(long id)
        {
            return _context.getAllStudents().Any(e => e.Id == id);
        }
    }
}
