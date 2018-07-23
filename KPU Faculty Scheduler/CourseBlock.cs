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
        public CourseBlock(int id_, int time_)
        {
            id = id_;
            time = time_;
        }

        public CourseBlock(Professor prof_, Room room_, Course course_)
        {
            professor = prof_;
            room = room_;
            course = course_;
        }
        public CourseBlock(Professor prof_, Room room_, Course course_, int time_)
        {
            professor = prof_;
            room = room_;
            course = course_;
            time = time_;
        }

        // Method to confirm whether a professor can teach a course or not.
        public bool checkCanTeach()
        {
            return professor.classList.Contains(course.name.ToString());
        }


        // Method to check if the course requires computers and, if so, the room has computers.
        public bool checkComputers()
        {
            if (this.course.needsComputers == true && this.room.hasComputers == false)
            {
                return false;
            }
            else return true; //this allows classes that don't need computers to be held in rooms with computers
            //TODO: should this be changed?
        }
    }
}
