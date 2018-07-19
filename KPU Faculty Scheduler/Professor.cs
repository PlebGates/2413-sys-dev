using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPU_Faculty_Scheduler
{
    public class Professor
    {
        // Variables to hold the professors name, and their ID.
        public int id;
        public String name;

        // List to hold the classes that the professor can teach.
        public List<String> classList;

        // Default Constructor.
        public Professor()
        {

        }

        // Main Constructor.
        public Professor(int id_, String name_)
        {
            id = id_;
            name = name_;
        }
    }
}
