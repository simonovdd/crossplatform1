using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simonov.models
{
    public class Group
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; }

        public int Count()
        {
            return Students.Count;
        }
    }
}