using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace simonov.models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        //public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }

        public IEnumerable<Student> getAllStudents()
        {
            var res = new List<Student>();
            foreach (var G in Groups)
            {
                res.AddRange(G.Students);
            }
            return res;
        }

        public IEnumerable<Student> getHeadmans()
        {
            return
                from Student in getAllStudents()
                where Student.headman == true
                select Student;
        }

        public IEnumerable<Group> getBigGroups(int h)
        {
            return Groups.Where(g => g.Students.Count > h);
        }

        public IEnumerable<string> getStudentsGroup(string name)
        {
            return Groups.Where(g => g.Students.FirstOrDefault(s => s.Name == name) != null).Select(g => g.Name);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .OwnsMany(property => property.Students);
        }
    }
 }