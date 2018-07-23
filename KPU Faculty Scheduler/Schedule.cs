using CsvHelper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace KPU_Faculty_Scheduler
{
    public class Schedule
    {

        // List to hold all of the created Course Blocks.
        public List<CourseBlock> classList = new List<CourseBlock>();

        // Method to swap a course with another time block (or course).
        public void swapCourseBlock(CourseBlock a, CourseBlock b)
        {
                int atime = a.time; //get the value of a's timeblock
                int btime = b.time; //get the value of b's timeblock
                int indexa = classList.FindIndex(c => c == a); //get the index of a from the class list
                int indexb = classList.FindIndex(c => c == b); //get the index of b from the class list
                classList[indexa].time = btime; //set the time of a to b's time
                classList[indexb].time = atime; //set the time of b to a's time
        }

        // Method to confirm the above is aloud to happen.
        public bool swapCourseValid(CourseBlock a, CourseBlock b)
        {
            List<CourseBlock> tempList = classList; //create a temp list
            swapCourseBlock(a, b); //swap the blocks
            return isValid(tempList); //see if the temp list is valid
        }

        // Method to swap a professor with another course blocks professor.
        public void swapProfessor(CourseBlock a, CourseBlock b)
        {
            Professor aprof = a.professor; //get a's professor
            Professor bprof = b.professor; //get b's professor
            int indexa = classList.FindIndex(c => c == a); //get the index of a from the class list
            int indexb = classList.FindIndex(c => c == b); //get the index of b from the class list
            classList[indexa].professor = bprof; //set a's prof to b's prof
            classList[indexb].professor = aprof; //set b's prof to a's prof
        }

        // Method to confirm the above is aloud to happen.
        public bool swapProfessorValid(CourseBlock a, CourseBlock b)
        {
            List<CourseBlock> tempList = classList; //create a temp list
            swapProfessor(a, b); //swap the profs
            return isValid(tempList); //see if the temp list is valid
        }

        // Method to swap a room with another course blocks room.
        public void swapRoom(CourseBlock a, CourseBlock b)
        {
            Room aroom = a.room; //get a's room
            Room broom = b.room; //get b's room
            int indexa = classList.FindIndex(c => c == a); //get the index of a from the class list
            int indexb = classList.FindIndex(c => c == b); //get the index of b from the class list
            classList[indexa].room = broom; //set a's room to b's room
            classList[indexb].room = aroom; //set b's room to a's room
        }

        // Method to confirm the above is aloud to happen.
        public bool swapRoomValid(CourseBlock a, CourseBlock b)
        {
            List<CourseBlock> tempList = classList; //create a temp list
            swapRoom(a, b); //swap the rooms
            return isValid(tempList); //see if the temp list is valid
        }

        // Method to swap the class taught between two course blocks.
        public void swapCourse(CourseBlock a, CourseBlock b)
        {
            Course acourse = a.course; //get a's course
            Course bcourse = b.course; //get b's course
            int indexa = classList.FindIndex(c => c == a); //get the index of a from the class list
            int indexb = classList.FindIndex(c => c == b); //get the index of b from the class list
            classList[indexa].course = bcourse; //set a's course to b's course
            classList[indexb].course = acourse; //set b's course to a's course
        }

        // Method to confirm the above is aloud to happen.
        public bool swapClassValid(CourseBlock a, CourseBlock b)
        {
            List<CourseBlock> tempList = classList; //create a temp list
            swapCourse(a, b); //swap the courses
            return isValid(tempList); //see if the temp list is valid
        }

        public bool CreateSchedule(List<Professor> profList, List<Course> courseList,List<Room> roomList)
        {
            bool done = false;
            int fail = 0;
            //TODO: add timer to stop this method if it takes too long and throw error?
            while (fail < 10) //if the loop hasn't resulted in a valid schedule, try again
            {
                List<CourseBlock> blockList = new List<CourseBlock>();
                int time = 0; //time starts at 0
                HashSet<IDTime> RoomTimeSet = new HashSet<IDTime>();
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
                    done = false;
                    while (!done)
                    {
                        //if the 6th character of a name (ex INFO |3|110) denotes 3rd or 4th year and the time isn't acceptable
                        int[] acceptableTimes = { 2, 3, 6, 7, 9, 10, 12, 13, 16, 17, 19, 20 }; //all 4-7 and 7-10 blocks
                        while ((course.name[5] == '3' || course.name[5] == '4') && !(acceptableTimes.Contains(time)))
                        {
                            time++;
                        }
                        foreach (Room room in roomList) //for each room
                        {
                            if (RoomTimeSet.Contains(new IDTime(room.id, time))) //if that room is already in use
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
                                    if (profTimeSet.Contains(new IDTime(prof.id, time)))
                                    {
                                        continue;
                                    }
                                    //if professor teaches two classes that day
                                    //if professor teaches four classes in total
                                    int day = timeToDay(time); //get the current day
                                    int classCountDay = 0; //classes during this day
                                    int classCountTotal = 0; //total classes for prof
                                    foreach (IDTime proftime in profTimeSet) //for each set proftime
                                    {
                                        if (proftime.id == prof.id) //if the current professor is assigned to that time
                                        {
                                            classCountTotal++; //add to the total classes
                                            if (timeToDay(proftime.time) == day) //if the proftime is teaching that day
                                            {
                                                classCountDay++; //add to classes that day
                                            }
                                            else if (day > 10) //if the current time is a double block and wasn't the current selected block
                                            {
                                                if (day / 10 == timeToDay(proftime.time)) //if the first digit of the double block matches the selected time
                                                {
                                                    classCountDay++; //it's on the same day
                                                }
                                                else if (day % 10 == timeToDay(proftime.time)) //if the second digit of the double block matches the selected time
                                                {
                                                    classCountDay++; //it's on the same day
                                                }
                                            }
                                            else if (timeToDay(proftime.time) > 10) //if the selected time is a double block
                                            {
                                                if (timeToDay(proftime.time) / 10 == day) //if the first digit of the selected time matches the current day
                                                {
                                                    classCountDay++; //it's on the same day
                                                }
                                                else if (timeToDay(proftime.time) % 10 == day) //if the second digit of the selected time matches the current day
                                                {
                                                    classCountDay++; //it's on the same day
                                                }
                                            }
                                        }
                                    }

                                    if (classCountTotal >= 4) //four classes or more already being taught?
                                    {
                                        continue; //move to the next prof
                                    }
                                    if (classCountDay >= 2) //two or more classes being taught today?
                                    {
                                        continue; //move to next prof
                                    }

                                    RoomTimeSet.Add(new IDTime(room.id, time)); //this room is now in use at this time
                                    profTimeSet.Add(new IDTime(prof.id, time)); //this professor is now teaching at this time
                                    blockList.Add(new CourseBlock(prof, room, course, time)); //add your new courseblock
                                    
                                    done = true; //this course has been put into a slot
                                    break;
                                }
                            if (done == true)
                            {
                                break;
                            }
                        }
                        time++; //increment time forwards
                    }
                        
                }

                if (isValid(blockList)) //check if the generated schedule is valid
                {
                    this.classList = blockList;
                    return true; //completed and valid
                }
                else fail++;
            }
            return false; //completed invalid
            
        }
        public bool isValid(List<CourseBlock> list)
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
            //return day # 1-6 for single day classes
            //return day1day2 for double day classes (ie a mon/wed class returns 13 because mon is 1 and wed is 3)
            //to convert use two operations, day/10 to get the first digit, day % 10 to get the second
            int day = 0;
            /* Time blocks
                 * M |T |W |T |F |S |S
                 * 0 |4 |0 |4 |14|14
                 * 1 |5 |8 |11|15|18
                 * 2 |6 |9 |12|16|19
                 * 3 |7 |10|13|17|20
                 */
               
            switch(time)
            {
                case 0:
                    day = 13;
                    break;
                case 1: 
                case 2:
                case 3:
                    day = 1;
                    break;
                case 4:
                    day = 24;
                    break;
                case 5:
                case 6:
                case 7:
                    day = 2;
                    break;
                case 8:
                case 9:
                case 10:
                    day = 3;
                    break;
                case 11:
                case 12:
                case 13:
                    day = 4;
                    break;
                case 14:
                    day = 56;
                    break;
                case 15:
                case 16:
                case 17:
                    day = 5;
                    break;
                case 18:
                case 19:
                case 20:
                    day = 6;
                    break;
                default: day = 0;
                    break;
            }

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
        public void extractToXslx(FileInfo filepath)
        {
            ExcelData xlData = new ExcelData(); //create new excel data
            xlData.setFileName(filepath.FullName);

            /*
            //check if file is in use
            if (ExcelClass.IsFileinUse(new FileInfo(xlData.getFileName()))) //if the excel file is open
            {
                //collect garbage and close the file
                GC.Collect();
                GC.WaitForPendingFinalizers();
                MessageBox.Show("Please close any open instances of the spreadsheet before attempting to save to it.");
                //System.Environment.Exit(1);
            } */

            // creating COM objects for the excel sheet
            Excel.Application xlApp = new Excel.Application(); //open the excel com object
            xlApp.SheetsInNewWorkbook = 1;
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Add(); //open the target workbook
            int row, col;

            HashSet<Room> roomSet = new HashSet<Room>();//get every room in the list and add to set
            HashSet<Professor> profSet = new HashSet<Professor>(); //get every professor and add to set
            HashSet<Course> courseSet = new HashSet<Course>(); //get every course and add to set

            var roomSheet = (Excel.Worksheet)xlWorkbook.Sheets.Add(xlWorkbook.Sheets[1], Type.Missing, Type.Missing, Type.Missing);
            roomSheet.Name = "Rooms";

            //switch worksheet and write all rooms
            roomSheet.Cells[1, 1] = "ID";
            roomSheet.Cells[1, 2] = "Building";
            roomSheet.Cells[1, 3] = "Number";
            roomSheet.Cells[1, 4] = "Computers";
            row = 2;
            col = 1;
            foreach(Room room in roomSet)
            {

                roomSheet.Cells[row, col] = room.id;
                roomSheet.Cells[row, col + 1] = room.building;
                roomSheet.Cells[row, col + 2] = room.roomNum;
                roomSheet.Cells[row, col + 3] = room.hasComputers;
                row++;
            }
            Marshal.ReleaseComObject(roomSheet); //release roomsheet because we no longer need it
            //switch worksheet and write all profs
            var profSheet = (Excel.Worksheet)xlWorkbook.Sheets.Add(xlWorkbook.Sheets[1], Type.Missing, Type.Missing, Type.Missing);
            roomSheet.Name = "Professors";
            
            profSheet.Cells[1, 1] = "ID";
            profSheet.Cells[1, 2] = "Name";
            profSheet.Cells[1, 3] = "Classes";
            row = 2;
            col = 1;
            foreach (Professor prof in profSet)
            {
                
                roomSheet.Cells[row, 1] = prof.id;
                roomSheet.Cells[row, 2] = prof.name;
                col = 3;
                foreach (string course in prof.classList)
                {
                    roomSheet.Cells[row, col++] = course;
                }
                row++;
            }
            Marshal.ReleaseComObject(profSheet); //release sheet
            //switch worksheet and write all courses
            var courseSheet = (Excel.Worksheet)xlWorkbook.Sheets.Add(xlWorkbook.Sheets[1], Type.Missing, Type.Missing, Type.Missing);
            courseSheet.Name = "Classes";
            profSheet.Cells[1, 1] = "ID";
            profSheet.Cells[1, 2] = "Name";
            profSheet.Cells[1, 3] = "Sections";
            profSheet.Cells[1, 4] = "Computers";
            row = 2;
            col = 1;
            foreach (Course course in courseSet)
            {

                courseSheet.Cells[row, 1] = course.id;
                courseSheet.Cells[row, 2] = course.name;
                courseSheet.Cells[row, 3] = course.sections;
                courseSheet.Cells[row, 4] = course.needsComputers;
                row++;
            }

            Marshal.ReleaseComObject(courseSheet);
            //switch worksheet and write all blocks
            var blockSheet = (Excel.Worksheet)xlWorkbook.Sheets.Add(xlWorkbook.Sheets[1], Type.Missing, Type.Missing, Type.Missing);
            courseSheet.Name = "Blocks";
            profSheet.Cells[1, 1] = "ID";
            profSheet.Cells[1, 2] = "Time";
            profSheet.Cells[1, 3] = "Professor ID";
            profSheet.Cells[1, 4] = "Course ID";
            profSheet.Cells[1, 4] = "Room ID";
            row = 2;
            col = 1;
            foreach (CourseBlock block in classList)
            {

                courseSheet.Cells[row, 1] = block.id;
                courseSheet.Cells[row, 2] = block.time;
                courseSheet.Cells[row, 3] = block.professor.id;
                courseSheet.Cells[row, 4] = block.course.id;
                courseSheet.Cells[row, 4] = block.room.id;
                row++;
            }

            xlWorkbook.SaveAs(filepath);
            //////////////////////////////////////cleanup///////////////////////////////////////////
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(blockSheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);

        }
        public void readFromXslx()
        {
            ExcelData xlData = new ExcelData(); //create new excel data
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*", //excel files, all files - open file window
                FilterIndex = 1 //set index to xlsx
            }; //new file dialog box
               //if (dialog.ShowDialog() == DialogResult.OK) //when ok button is clicked
            if (dialog.ShowDialog() == true) //when ok button is clicked
            {
                xlData.setFileName(dialog.FileName); //set exceldata filename to the selected spreadsheet
            }
            if (dialog.FileName == "") //if selections was cancelled
            {
                //System.Environment.Exit(1); //skip the rest of the program
            }

            //check if file is in use
            if (ExcelClass.IsFileinUse(new FileInfo(xlData.getFileName()))) //if the excel file is open
            {
                //collect garbage and close the file
                GC.Collect();
                GC.WaitForPendingFinalizers();
                MessageBox.Show("Please close any open instances of the spreadsheet before opening.");
            }

            // creating COM objects for the excel sheet
            Excel.Application xlApp = new Excel.Application(); //open the excel com object
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(xlData.getFileName()); //open the target workbook
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1]; //open the first worksheet

            int row, col;
            //read rooms
            HashSet<Room> roomSet = new HashSet<Room>(); //create set for rooms
            xlWorksheet = (Excel.Worksheet)xlWorkbook.Sheets["Rooms"]; //go to rooms sheet
            row = 2;
            while (xlWorksheet.Cells[row, 1] != null)//if cell is not null
            {
                Room room = new Room(); //create new room
                room.id = Int32.Parse((string)xlWorksheet.Cells[row, 1]); //parse id to int
                room.building = xlWorksheet.Cells[row, 2]; //get building
                room.roomNum = xlWorksheet.Cells[row, 3]; //this might throw errors, maybe parse to int
                room.hasComputers = xlWorksheet.Cells[row, 4]; //parse boolean???

                roomSet.Add(room); //add room to set
                row++; //next row
            }
            

            //read courses
            HashSet<Course> courseSet = new HashSet<Course>(); //create set for courses
            xlWorksheet = (Excel.Worksheet)xlWorkbook.Sheets["Courses"]; //go to courses sheet
            row = 2;
            while (xlWorksheet.Cells[row, 1] != null)//if cell is not null
            {
                Course course = new Course(); //create new course
                course.id = Int32.Parse((string)xlWorksheet.Cells[row, 1]); //parse id to int
                course.name = xlWorksheet.Cells[row, 2]; //get name
                course.sections= xlWorksheet.Cells[row, 3]; //this might throw errors, maybe parse to int
                course.needsComputers = xlWorksheet.Cells[row, 4]; //parse boolean???

                courseSet.Add(course); //add room to set
                row++; //next row
            }

            //read professors
            HashSet<Professor> profSet = new HashSet<Professor>(); //create set for courses
            xlWorksheet = (Excel.Worksheet)xlWorkbook.Sheets["Professors"]; //go to profs sheet
            row = 2;
            while (xlWorksheet.Cells[row, 1] != null)//if cell is not null
            {
                col = 3;
                Professor prof = new Professor(); //create new prof
                prof.id = Int32.Parse((string)xlWorksheet.Cells[row, 1]); //parse id to int
                prof.name = xlWorksheet.Cells[row, 2]; //get name
                while (xlWorksheet.Cells[row,col] != null) //while there are still classes to get
                {
                    prof.classList.Add(xlWorksheet.Cells[row, col]); //add to the prof's list
                    col++; //increment
                }

                profSet.Add(prof); //add room to set
                row++; //next row
            }
            //read blocks
            List<CourseBlock> tempList = new List<CourseBlock>();
            xlWorksheet = (Excel.Worksheet)xlWorkbook.Sheets["Blocks"]; //go to blocks sheet
            row = 2;
            while(xlWorksheet.Cells[row, 1] != null)
            {
                int profid = 0; //reset values
                int roomid = 0;
                int courseid = 0;
                CourseBlock block = new CourseBlock(); //create new block

                //parse data
                block.id = Int32.Parse((string)xlWorksheet.Cells[row, 1]); //parse id to int
                block.time = xlWorksheet.Cells[row, 2]; //get time
                profid = xlWorksheet.Cells[row, 3]; //this might throw errors, maybe parse to int
                courseid = xlWorksheet.Cells[row, 4];
                roomid = xlWorksheet.Cells[row, 5]; 

                //
                foreach (Course course in courseSet) //set course
                {
                    if (course.id == courseid)
                    {
                        block.course = course;
                        break;
                    }
                }
                foreach (Professor prof in profSet) //set prof
                {
                    if (prof.id == profid)
                    {
                        block.professor = prof;
                        break;
                    }
                }
                foreach (Room room in roomSet) //set room
                {
                    if (room.id == roomid)
                    {
                        block.room = room;
                        break;
                    }
                }
                tempList.Add(block);
                row++;
            }

            if (isValid(tempList))
            {
                classList = tempList;
            }
            //////////////////////////////////////cleanup///////////////////////////////////////////
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }
        public void extractToCsv(FileInfo filepath)
        {
            HashSet<Room> roomSet = new HashSet<Room>();
            HashSet<Professor> profSet = new HashSet<Professor>();
            HashSet<Course> courseSet = new HashSet<Course>();
            HashSet<csvRecord> blockSet = new HashSet<csvRecord>();
            //take classList and get every room, course, and prof 
            foreach (CourseBlock block in classList)
            {
                roomSet.Add(block.room);
                profSet.Add(block.professor);
                courseSet.Add(block.course);
                blockSet.Add(new csvRecord(block)); //get every courseblock present
            }
            //create csv writer
            using (TextWriter writer = new StreamWriter(filepath.FullName)) //to ensure it's closed
            {
                CsvWriter csv = new CsvHelper.CsvWriter(writer);
                csv.WriteRecord("rooms"); 
                csv.NextRecord(); 
                csv.WriteRecords(roomSet); //write rooms to file
                csv.WriteRecord("courses");
                csv.NextRecord();
                csv.WriteRecords(courseSet); //write courses to file
                csv.WriteRecord("professors");
                csv.NextRecord();
                csv.WriteRecords(profSet); //write profs to file
                csv.WriteRecord("blocks");
                csv.NextRecord();
                csv.WriteRecords(blockSet); //write every courseblock to file
            }
        }
        public void readFromCsv(FileInfo filepath)
        {
            HashSet<Professor> profSet = new HashSet<Professor>(); //create profset
            HashSet<Room> roomSet = new HashSet<Room>(); //create roomset
            HashSet<Course> courseSet = new HashSet<Course>(); //create courseset
            HashSet<csvRecord> csvSet = new HashSet<csvRecord>(); //create blockset
            string readmode = "default";
            //open stream
            using (TextReader reader = new StreamReader(filepath.FullName))
            {
                CsvReader csv = new CsvReader(reader);
                while (csv.Read()) //while there are records to read
                {
                    //get the readmode for the current record
                    if (csv[0] == "rooms")
                    {
                        readmode = "rooms";
                        csv.Read();
                    } else if (csv[0] == "courses")
                    {
                        readmode = "courses";
                        csv.Read();
                    } else if (csv[0] == "professors")
                    {
                        readmode = "professors";
                    } else if (csv[0] == "blocks")
                    {
                        readmode = "blocks";
                        csv.Read();
                    }

                    switch(readmode) //depending on readmode
                    {
                        case "rooms":
                            roomSet.Add(csv.GetRecord<Room>()); //read rooms to set
                            break;
                        case "courses": //read courses to set
                            courseSet.Add(csv.GetRecord<Course>());
                            break;
                        case "professors": //read profs to set
                            profSet.Add(csv.GetRecord<Professor>());
                            break;
                        case "blocks": //read blocks to set
                            csvSet.Add(csv.GetRecord<csvRecord>());
                            break;
                        default: //probably throw some kind of exception here
                            break;
                    }
                }
                
                //find the rooms record
                //read the records
                //find the courses record
                //read the records
                //find the profs record
                //read the records
                //find the blocks record
                //read the records
                //close reader
            }

            List<CourseBlock> tempList = new List<CourseBlock>();
            //convert profs/rooms/courses/blocks to courseblock list
            foreach (csvRecord record in csvSet)
            {
                CourseBlock block = new CourseBlock(record.blockID, record.time);
                foreach (Professor prof in profSet) //for each professor in the set
                {
                    if (prof.id == record.professorID) //if the id matches
                    {
                        block.professor = prof; //set professor
                        break; //break the loop since the prof has been found
                    }
                }
                foreach (Room room in roomSet) //for each room in the set
                {
                    if (room.id == record.roomID) //if the id matches
                    {
                        block.room = room; //set room
                        break; //break the loop since the room has been found
                    }
                }
                foreach (Course course in courseSet) //for each course in the set
                {
                    if (course.id == record.courseID) //if the id matches
                    {
                        block.course = course; //set course
                        break; //break the loop since the course has been found
                    }
                }
                tempList.Add(block);
            }
            //this.classlist = courseblock list
            if (isValid(tempList))
            {
                classList = tempList;
            }
        }
        public class csvRecord
        {
            public int blockID;
            public int courseID;
            public int roomID;
            public int professorID;
            public int time;

            public csvRecord(CourseBlock block)
            {
                blockID = block.id;
                courseID = block.course.id;
                roomID = block.room.id;
                professorID = block.professor.id;
                time = block.time;
            }
        }
    }

        
}
