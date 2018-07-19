using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPU_Faculty_Scheduler
{
    public class Schedule
    {

        // List to hold all of the created Course Blocks.
        List<CourseBlock> classList = new List<CourseBlock>();

        // Method to swap a course with another time block (or course).
        void swapCourse()
        {

        }

        // Method to confirm the above is aloud to happen.
        bool swapCourseValid()
        {
            /* random output so it will compile. */
            return true;
        }

        // Method to swap a professor with another course blocks professor.
        void swapProfessor()
        {

        }

        // Method to confirm the above is aloud to happen.
        bool swapProfessorValid()
        {
            /* random output so it will compile. */
            return true;
        }

        // Method to swap a room with another course blocks room.
        void swapRoom()
        {

        }

        // Method to confirm the above is aloud to happen.
        bool swapRoomValid()
        {
            /* random output so it will compile. */
            return true;
        }

        // Method to swap the class taught between two course blocks.
        void swapClass()
        {

        }

        // Method to confirm the above is aloud to happen.
        bool swapClassValid()
        {
            /* random output so it will compile. */
            return true;
        }

        public bool isValid()
        {
            return true;
        }

        public void CreateSchedule(List<Professor> profList, List<Course> courseList,List<Room> roomList)
        {
            List<CourseBlock> blockList = new List<CourseBlock>();
            int time = 0; //time starts at 0
            HashSet<RoomTime> roomTimeSet = new HashSet<RoomTime>();
            /* Time blocks
             * M |T |W |T |F |S |S
             * 0 |4 |0 |4 |14|14
             * 1 |5 |8 |11|15|18
             * 2 |6 |9 |12|16|19
             * 3 |7 |10|13|17|20
             */
             foreach (Course course in courseList)
            {
                time = time % 20; //if the time reaches 20 then reset it

                if (course.sections > 0) //if the course still has more sections left
                {
                    foreach (Room room in roomList) //for each room
                    {
                        if (roomTimeSet.Contains(new RoomTime(room.id, time))) //if that room is already in use
                        {
                            continue;
                        } else if (course.needsComputers && !room.hasComputers) //computer validity check
                        {
                            continue; //if the room doesn't have computers and the class needs them, skip it
                        } else foreach (Professor prof in profList)
                            {
                                //more validity checking and logic
                                roomTimeSet.Add(new RoomTime(room.id, time)); //this room is now in use at this time
                                blockList.Add(new CourseBlock(prof, course, room)); //add your new courseblock
                                time++; //increment time forwards
                                course.sections--; //increment the sections of the course down 1
                            }
                        
                    }
                }
            }
        }
        class RoomTime //just a container for room and time
        {
            int room;
            int time;
            public RoomTime(int room_, int time_)
            {
                room = room_;
                time = time_;
            }
        }
    }
}
