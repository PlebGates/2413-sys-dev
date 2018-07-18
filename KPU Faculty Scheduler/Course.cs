using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPU_Faculty_Scheduler
{
    public class Course
    {
        // Variables to hold the name of the course, id, and section #.
        public int id;
        public String name;
        public int sections;

        // Bool variable for whether this course needs a computer lab or not.
        public Boolean needsComputers;

        // Default Constructor.
        public Course()
        {

        }

        // Main Constructor.
        public Course(int id_, int sections_, String name_, Boolean needsComputers_)
        {
            id = id_;
            sections = sections_;
            name = name_;
            needsComputers = needsComputers_;
        }
    }
}
