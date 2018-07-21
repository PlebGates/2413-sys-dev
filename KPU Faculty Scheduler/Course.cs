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
        public bool needsComputers;

        // Default Constructor.
        public Course()
        {

        }

        // Main Constructor.
        public Course(String name_, int sections_, bool needsComputers_)
        {
            name = name_;
            sections = sections_;
            needsComputers = needsComputers_;
        }

        public String getName()
        {
            return name;
        }

        public int getSection()
        {
            return sections;
        }

        public bool getComp()
        {
            return needsComputers;
        }
    }
}
