using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPU_Faculty_Scheduler
{
    public class CourseBlock
    {
        // Variables to hold time and id.
        public int id;
        public int time;

        // Create a new instance of Professor, Room, and Course.
        public Professor professor = new Professor();
        public Room room = new Room();
        public Course course = new Course();

        // Default Constructor.
        public CourseBlock()
        {

        }

        // Main Constructor.
        CourseBlock(int id_, int time_)
        {
            id = id_;
            time = time_;
        }

        // Method to confirm whether a professor can teach a course or not.
        bool checkCanTeach()
        {
            /* random output so it will compile. */
            return true;
        }


        // Method to check if the course requires computers and, if so, the room has computers.
        bool checkComputers()
        {
            /* random output so it will compile. */
            return true;
        }
    }
}
