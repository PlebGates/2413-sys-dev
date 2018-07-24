using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace KPU_Faculty_Scheduler
{
    public class DBMethods
    {
        SQLiteConnection connection;
        public DBMethods(SQLiteConnection connection_)
        {
            connection = connection_; //"Data Source=:memory:"
            connection.Open();
            createTables();
        }

        public void createTables()
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText =
            @"CREATE TABLE professors(
	id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	name VARCHAR(50) NOT NULL
	);
	
CREATE TABLE rooms(
	id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	name VARCHAR(50) NOT NULL,
    roomNumber int NOT NULL,
	hasComputers INTEGER NOT NULL
	);

CREATE TABLE courses(
	id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	name VARCHAR(50) NOT NULL,
	sections INTEGER NOT NULL,
	needsComputers INTEGER NOT NULL
	);
	
CREATE TABLE schedule(
	id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	professorID INTEGER,
    courseID INTEGER,
	roomID INTEGER,
	time INTEGER
	);

CREATE TABLE professorscourses(
	professorID INTEGER NOT NULL,
	courseID INTEGER NOT NULL,
	PRIMARY KEY(professorID,courseID)
	);";
            cmd.ExecuteNonQuery();
        }

        public Professor getProfessor(int id)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from professors where id = " + id;
            using (SQLiteDataReader data = cmd.ExecuteReader())
            {
                
                while(data.Read())
                {
                    Professor prof = new Professor();
                    prof.id = data.GetInt32(0);
                    prof.name = data.GetString(1);
                    return prof;
                }
                
            }
            return null;
            //return new Prof(data.GetInt32(0), data.GetString(1));
        }
        public List<Professor> getAllProfessor()
        {
            SQLiteCommand cmd = connection.CreateCommand(); //new sql command
            cmd.CommandText = "select * from professors"; //select all profs
            List<Professor> profList = new List<Professor>(); //create the list
            using (SQLiteDataReader data = cmd.ExecuteReader()) //using the datareader
            {
                while (data.Read())
                {
                    Professor prof = new Professor();
                    prof.id = data.GetInt32(0);
                    prof.name = data.GetString(1);
                    prof.classList = getClassList(prof.id);
                    profList.Add(prof);
                }
            }
            return profList; //return the list
        }
        public bool canTeach(int teachID, int classID)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from professorscourses where professorID = " + teachID + " and classID = " + classID;
            //select the row where the teacher id and class id are shared
            using (SQLiteDataReader data = cmd.ExecuteReader())
            {
                while(data.Read())
                {
                    return data.HasRows; //if that row exists, then the class is taught by the teacher - so you return true
                }
                
            }
            return false;
        }
        public List<String> getCanTeach(int teachID)
        {   //populates Professor List<String> classList
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from professorscourses where professorID = " + teachID;

            List<String> classList = new List<string> { };
            using (SQLiteDataReader data = cmd.ExecuteReader())
            {
                Professor professor = getProfessor(teachID);
                while (data.Read())
                {
                    classList.Add( getCourse(data.GetInt32(1)).name);
                }
                return classList;
            }
        }
        public List<String> getClassList(int id)
        {
            List<String> classList = new List<string>();

            SQLiteCommand cmd = connection.CreateCommand(); //new sql command
            cmd.CommandText = "select * from professorscourses where professorID = " + id; //select all courses
            using (SQLiteDataReader data = cmd.ExecuteReader()) //using the datareader
            {
                while (data.Read())
                {
                    classList.Add(getCourse(data.GetInt32(1)).name); //get the course name and add to list
                }
            }
            return classList; //return the list
        }
        public Room getRoom(int id)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from rooms where id = " + id;
            using (SQLiteDataReader data = cmd.ExecuteReader())
            {
                while (data.Read())
                {
                    Room room = new Room();
                    room.id = data.GetInt32(0);
                    room.building = data.GetString(1);
                    room.roomNum = data.GetInt32(2);
                    room.hasComputers = (data.GetInt32(3) == 1);
                    return room;
                }
                return null;
            }
                
            //return new Room(data.GetInt32(0), data.GetString(1), data.GetBoolean(3));
        }

        public List<Room> getAllRoom()
        {
            SQLiteCommand cmd = connection.CreateCommand(); //new sql command
            cmd.CommandText = "select * from rooms"; //select all rooms
            List<Room> roomList = new List<Room>(); //create the roomlist
            using (SQLiteDataReader data = cmd.ExecuteReader()) //using the datareader
            {
                while (data.Read()) //while there are rows to read
                {
                    Room room = new Room(); //create a new room
                    
                    room.id = data.GetInt32(0); //get the roomid
                    room.building = data.GetString(1);
                    room.roomNum = data.GetInt32(2);
                    room.hasComputers = (data.GetInt32(3) == 1); //get the bool value of having computers
                    roomList.Add(room); //add the room to the list
                }
            }

                return roomList; //return the list
        }
        public Course getCourse(int id)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from courses where id = " + id;
            
            using (SQLiteDataReader data = cmd.ExecuteReader())
            {
                while (data.Read())
                {
                    Course course = new Course();
                    course.id = data.GetInt32(0);
                    course.name = data.GetString(1);
                    course.sections = data.GetInt32(2);
                    course.needsComputers = (data.GetInt32(3) == 1);
                    return course;
                }
            }
            return null;
                
            //return new Room(data.GetInt32(0), data.GetString(1), data.GetBoolean(3));
        }
        public List<Course> getAllCourse()
        {
            SQLiteCommand cmd = connection.CreateCommand(); //new sql command
            cmd.CommandText = "select * from courses"; //select all courses
            List<Course> courseList = new List<Course>(); //create the list
            using (SQLiteDataReader data = cmd.ExecuteReader()) //using the datareader
            {
               
                while (data.Read()) //while there are rows to read
                {
                    Course course = new Course(); //create a new course
                    course.id = data.GetInt32(0); //get the course id
                    course.name = data.GetString(1); //get thecourse name
                    course.sections = data.GetInt32(2); //get the section #
                    course.needsComputers = (data.GetInt32(3) == 1); //get the bool value of having computers
                    courseList.Add(course); //add the room to the list
                }
            }

            return courseList; //return the list
        }
        public CourseBlock getBlock(int id)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from schedule where id = " + id;
            
            using (SQLiteDataReader data = cmd.ExecuteReader())
            {
                while (data.Read())
                {
                    CourseBlock block = new CourseBlock();
                    block.id = id;
                    block.professor = getProfessor(data.GetInt32(1));
                    block.course = getCourse(data.GetInt32(2));
                    block.room = getRoom(data.GetInt32(3));
                    block.time = data.GetInt32(4);
                    return block;
                }
            }
            return null;
        }

        public List<CourseBlock> getAllCourseBlockTime(int time)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from schedule where time = " + time;

            List<CourseBlock> blockList = new List<CourseBlock>(); //create the list
            using (SQLiteDataReader data = cmd.ExecuteReader()) //using the reader
            {
                while (data.Read()) //while there are records
                {
                    CourseBlock block = new CourseBlock(); //create a block
                    block.id = data.GetInt32(0); //get the id
                    block.professor = getProfessor(data.GetInt32(1)); //get the prof object
                    block.course = getCourse(data.GetInt32(2)); //get the course
                    block.room = getRoom(data.GetInt32(3)); //get the room
                    block.time = data.GetInt32(4); //get the time
                    blockList.Add(block); //add the block
                }
            }

            return blockList; //return the list
        }

        public List<CourseBlock> getAllCourseBlock()
        {
            SQLiteCommand cmd = connection.CreateCommand(); //new sql command
            cmd.CommandText = "select * from courses"; //select all courses
            List<CourseBlock> blockList = new List<CourseBlock>(); //create the list
            using (SQLiteDataReader data = cmd.ExecuteReader()) //using the reader
            {
                while (data.Read()) //while there are records
                {
                    CourseBlock block = new CourseBlock(); //create a block
                    block.id = data.GetInt32(0); //get the id
                    block.professor = getProfessor(data.GetInt32(1)); //get the prof object
                    block.course = getCourse(data.GetInt32(2)); //get the course
                    block.room = getRoom(data.GetInt32(3)); //get the room
                    block.time = data.GetInt32(4); //get the time
                    blockList.Add(block); //add the block
                }
            }

                return blockList; //return the list
        }

        public void addProfessor(Professor prof)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO professors (name) values ('" + prof.name + "');";
            cmd.ExecuteNonQuery();
        }
        public void addRoom(Room room)
        {
            int computers = room.hasComputers ? 1 : 0; //room has computers? yes = 1, no = 0
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO rooms (name,roomNumber,hasComputers) values ('" + room.building + "'," + room.roomNum + "," + computers + ");";
            cmd.ExecuteNonQuery();
        }
        public void addCourse(Course course)
        {
            int computers = course.needsComputers ? 1 : 0; //room has computers? yes = 1, no = 0
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO courses (name,sections,needsComputers) values ('" + course.name + "'," + course.sections + "," + computers + ");";
            cmd.ExecuteNonQuery();
        }
        public void addCanTeach(int profid, int courseid)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO professorscourses (professorID,courseID) values (" + profid + "," + courseid + ");";
            cmd.ExecuteNonQuery();
        }
        public void addCourseBlock(CourseBlock block)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO schedule (professorid,courseid,roomid,time) values ("+ block.professor.id +
                "," + block.course.id + "," + block.room.id + "," + block.time + ");";
            cmd.ExecuteNonQuery();
        }
    }
}
