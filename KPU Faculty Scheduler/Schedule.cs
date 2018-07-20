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
        void swapCourseBlock(CourseBlock a, CourseBlock b)
        {
                int atime = a.time; //get the value of a's timeblock
                int btime = b.time; //get the value of b's timeblock
                int indexa = classList.FindIndex(c => c == a); //get the index of a from the class list
                int indexb = classList.FindIndex(c => c == b); //get the index of b from the class list
                classList[indexa].time = btime; //set the time of a to b's time
                classList[indexb].time = atime; //set the time of b to a's time
        }

        // Method to confirm the above is aloud to happen.
        bool swapCourseValid(CourseBlock a, CourseBlock b)
        {
            /* random output so it will compile. */
            return true;
        }

        // Method to swap a professor with another course blocks professor.
        void swapProfessor(CourseBlock a, CourseBlock b)
        {
            Professor aprof = a.professor; //get a's professor
            Professor bprof = b.professor; //get b's professor
            int indexa = classList.FindIndex(c => c == a); //get the index of a from the class list
            int indexb = classList.FindIndex(c => c == b); //get the index of b from the class list
            classList[indexa].professor = bprof; //set a's prof to b's prof
            classList[indexb].professor = aprof; //set b's prof to a's prof
        }

        // Method to confirm the above is aloud to happen.
        bool swapProfessorValid()
        {
            /* random output so it will compile. */
            return true;
        }

        // Method to swap a room with another course blocks room.
        void swapRoom(CourseBlock a, CourseBlock b)
        {
            Room aroom = a.room; //get a's room
            Room broom = b.room; //get b's room
            int indexa = classList.FindIndex(c => c == a); //get the index of a from the class list
            int indexb = classList.FindIndex(c => c == b); //get the index of b from the class list
            classList[indexa].room = broom; //set a's room to b's room
            classList[indexb].room = aroom; //set b's room to a's room
        }

        // Method to confirm the above is aloud to happen.
        bool swapRoomValid()
        {
            /* random output so it will compile. */
            return true;
        }

        // Method to swap the class taught between two course blocks.
        void swapCourse(CourseBlock a, CourseBlock b)
        {
            Course acourse = a.course; //get a's course
            Course bcourse = b.course; //get b's course
            int indexa = classList.FindIndex(c => c == a); //get the index of a from the class list
            int indexb = classList.FindIndex(c => c == b); //get the index of b from the class list
            classList[indexa].course = bcourse; //set a's course to b's course
            classList[indexb].course = acourse; //set b's course to a's course
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

        public bool CreateSchedule(List<Professor> profList, List<Course> courseList,List<Room> roomList)
        {
            bool done = false;
            //TODO: add timer to stop this method if it takes too long and throw error?
            while (!done) //if the loop hasn't resulted in a valid schedule, try again
            {
                List<CourseBlock> blockList = new List<CourseBlock>();
                int time = 0; //time starts at 0
                HashSet<IDTime> IDTimeSet = new HashSet<IDTime>();
                HashSet<IDTime> profTimeSet = new HashSet<IDTime>();
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
                        foreach (Room room in roomList) //for each room
                        {
                            if (IDTimeSet.Contains(new IDTime(room.id, time))) //if that room is already in use
                            {
                                continue;
                            }
                            else if (course.needsComputers && !room.hasComputers) //computer validity check
                            {
                                continue; //if the room doesn't have computers and the class needs them, skip it
                            }
                            else foreach (Professor prof in profList)
                                {
                                //if professor is already teaching at that time
                                if (profTimeSet.Contains(new IDTime(prof.id,time)))
                                {
                                    continue;
                                }
                                //if professor teaches two classes that day
                                //if professor teaches four classes in total
                                int day = timeToDay(time);
                                int classCountDay = 0;
                                int classCountTotal = 0;
                                foreach (IDTime proftime in profTimeSet)
                                {
                                    if (proftime.id == prof.id)
                                    {
                                        classCountTotal++;
                                        if (timeToDay(proftime.time) == day)
                                        {
                                            classCountDay++;
                                        }
                                    }
                                }

                                if (classCountTotal >= 4)
                                {
                                    continue;
                                }
                                if (classCountDay >= 2)
                                {
                                    continue;
                                }
                                    //more validity checking and logic
                                    IDTimeSet.Add(new IDTime(room.id, time)); //this room is now in use at this time
                                profTimeSet.Add(new IDTime(prof.id, time)); //this professor is now teaching at this time
                                    blockList.Add(new CourseBlock(prof, room, course, time)); //add your new courseblock
                                    time++; //increment time forwards
                                }
                        }
                }

                if (validityCheck(blockList)) //check if the generated schedule is valid
                {
                    this.classList = blockList;
                    done = true;
                    return true; //completed and valid
                }
            }
            return false; //completed invalid
            
        }
        public bool validityCheck(List<CourseBlock> list)
        {
            HashSet<IDTime> roomTimes = new HashSet<IDTime>(); //set of IDTimes
            HashSet<Professor> profSet = new HashSet<Professor>();
            HashSet<IDTime> classTimes = new HashSet<IDTime>();
            foreach (CourseBlock block in list)
            {
                profSet.Add(block.professor); //add each professor into a set to check if they're teaching more than 4 classes or more than 2 in a day
                
                //foreach block check if the professor can teach it
                if (!block.checkCanTeach())
                {
                    return false;
                }

                //foreach block check if the computers have the correct value
                if (!block.checkComputers())
                {
                    return false;
                }

                //foreach block add its time to the IDTime set
                if (!roomTimes.Add(new IDTime(block.room.id, block.time)))
                { //adding an element to a hashset returns false if it already exists
                    return false;
                }

                //foreach block check if it's a 3rd or 4th year class in the wrong timeslot
                //if the 6th character of a name (ex INFO |3|110) denotes 3rd or 4th year and the time isn't acceptable
                int[] acceptableTimes = { 2, 3, 6, 7, 9, 10, 12, 13, 16, 17, 19, 20 }; //all 4-7 and 7-10 blocks
                if ((block.course.name[5] == '3' || block.course.name[5] == '4') && !(acceptableTimes.Contains(block.time)))
                {
                    return false;
                }

                //foreach block check if there's another section at the same time
                //using the same method of checking if a room is in use at the same time, do it with a class id instead
                if (!classTimes.Add(new IDTime(block.course.id,block.time)))
                {
                    return false;
                }
            }
            
            
            foreach(Professor prof in profSet)
            {
                //check if any professor is teaching more than 4 classes
                if (!classesTotal(list,prof))
                {
                    return false;
                }
                //check if any professor is teaching more than 2 classes in a day
                if (!classesInDay(list, prof))
                {
                    return false;
                }
            }
            
            return true;
        }

        //check if the prof is teaching more than 4 classes total
        public bool classesTotal(List<CourseBlock> list, Professor prof)
        {
            int sum = 0;
            foreach (CourseBlock block in list) //for each courseblock in the list
            {
                if (block.professor == prof) //if the teacher is the selected prof
                {
                    sum++; //increment our sum
                }
            }
            if (sum > 4) //if the sum is more than 4 classes in total
            {
                return false; //fail the check
            }
            else return true; //else pass the check
        }
        //check if the prof is teaching more than 2 classes in any given day
        public bool classesInDay(List<CourseBlock> list, Professor prof)
        { 
            /* Time blocks
                 * M |T |W |T |F |S |S
                 * 0 |4 |0 |4 |14|14
                 * 1 |5 |8 |11|15|18
                 * 2 |6 |9 |12|16|19
                 * 3 |7 |10|13|17|20
                 */
            int[] days = { 0, 0, 0, 0, 0, 0 }; //6 teaching days
            foreach (CourseBlock block in list)
            {
                if (block.professor == prof)
                {
                    if (block.time == 0) //first block on monday/wednesday
                    {
                        days[0]++; //one class on monday
                        days[2]++; //one class on wednesday
                    }
                    else if (block.time >= 1 && block.time <= 3) //other classes on monday
                    {
                        days[0]++;
                    }
                    else if (block.time == 4) //first block on tues/thursday
                    {
                        days[1]++; //one class on tuesday
                        days[3]++; //one class on thursday
                    }
                    else if (block.time >= 5 && block.time <= 7) //other classes on tues
                    {
                        days[1]++;
                    }
                    else if (block.time >= 8 && block.time <= 10) //other classes on wed
                    {
                        days[2]++;
                    }
                    else if (block.time >= 11 && block.time <= 13) //other classes on wed
                    {
                        days[3]++;
                    }
                    else if (block.time == 14) //first block on fri/sat
                    {
                        days[4]++; //one class on fri
                        days[5]++; //one class on sat
                    }
                    else if (block.time >= 15 && block.time <= 17) //other classes on fri
                    {
                        days[4]++;
                    }
                    else if (block.time >= 18 && block.time <= 20) //other classes on sat
                    {
                        days[5]++;
                    }
                }
            }
            for (int n = 0; n < 6; n++)
            {
                if (days[n] > 2)
                {
                    return false; //if teaching more than 2 blocks on any day return false
                }
            }
            return true; //not teaching more than 2 blocks a day
        }
        public int timeToDay(int time)
        {
            int day = 0;
            /* Time blocks
                 * M |T |W |T |F |S |S
                 * 0 |4 |0 |4 |14|14
                 * 1 |5 |8 |11|15|18
                 * 2 |6 |9 |12|16|19
                 * 3 |7 |10|13|17|20
                 */

            return day;
        }
        class IDTime //just a container for room and time for use in IDTime sets
        {
            public int id;
            public int time;
            public IDTime(int room_, int time_)
            {
                id = room_;
                time = time_;
            }
        }
    }
}
