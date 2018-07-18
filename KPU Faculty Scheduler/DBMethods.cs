using System;
using System.Data.SQLite;

namespace KPU_Faculty_Scheduler
{
    public class DBMethods
    {
        SQLiteConnection connection;
        public DBMethods(SQLiteConnection connection_)
        {
            connection = connection_;
        }

        public void createTables()
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText =
            @"CREATE TABLE professors(
	id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	name VARCHAR(50) NOT NULL
	)
	
CREATE TABLE rooms(
	id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	name VARCHAR(50) NOT NULL,
	hasComputers INTEGER NOT NULL
	)

CREATE TABLE courses(
	id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	name VARCHAR(50) NOT NULL,
	sections INTEGER NOT NULL,
	needsComputers INTEGER NOT NULL
	)
	
CREATE TABLE schedule(
	id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
	profID INTEGER,
    courseID INTEGER,
	roomID INTEGER,
	time INTEGER
	)
	
CREATE TABLE professorscourses(
	professorID INTEGER NOT NULL,
	courseID INTEGER NOT NULL,
	PRIMARY KEY(profID,courseID)
	)"; //test/
            cmd.ExecuteNonQuery();
        }

        public Professor getProfessor(int id)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from professors where id = " + id;
            SQLiteDataReader data = cmd.ExecuteReader();
            Professor prof = new Professor();
            prof.id = data.GetInt32(0);
            prof.name = data.GetString(1);
            return prof;
            //return new Prof(data.GetInt32(0), data.GetString(1));
        }
        public bool canTeach(int teachID, int classID)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from professorsclass where teachID = " + teachID + " and classID = " + classID;
            //select the row where the teacher id and class id are shared
            SQLiteDataReader data = cmd.ExecuteReader();
            return data.HasRows; //if that row exists, then the class is taught by the teacher - so you return true
        }
        public Room getRoom(int id)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from rooms where id = " + id;
            SQLiteDataReader data = cmd.ExecuteReader();
            Room room = new Room();
            room.id = data.GetInt32(0);
            room.name = data.GetString(1);
            room.hasComputers = (data.GetInt32(3) == 1);
            return room;
            //return new Room(data.GetInt32(0), data.GetString(1), data.GetBoolean(3));
        }
        public Course getCourse(int id)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from rooms where id = " + id;
            SQLiteDataReader data = cmd.ExecuteReader();
            Course course = new Course();
            course.id = data.GetInt32(0);
            course.name = data.GetString(1);
            course.needsComputers = (data.GetInt32(3) == 1);
            return course;
            //return new Room(data.GetInt32(0), data.GetString(1), data.GetBoolean(3));
        }
        public CourseBlock getBlock(int id)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from schedule where id = " + id;
            SQLiteDataReader data = cmd.ExecuteReader();
            CourseBlock block = new CourseBlock();
            block.id = id;
            block.professor = getProfessor(data.GetInt32(1));
            block.course = getCourse(data.GetInt32(2));
            block.room = getRoom(data.GetInt32(3));
            block.time = data.GetInt32(4);
            return block;
        }

        public void addProfessor(Professor prof)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO professors (id,name) values (" + prof.id + "," + prof.name + ");";
            cmd.ExecuteNonQuery();
        }
        public void addRoom(Room room)
        {
            int computers = room.hasComputers ? 1 : 0; //room has computers? yes = 1, no = 0
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO rooms (id,name,hasComputers) values (" + room.id + "," + room.name + "," + computers + ");";
            cmd.ExecuteNonQuery();
        }
        public void addCourse(Course course)
        {
            int computers = course.needsComputers ? 1 : 0; //room has computers? yes = 1, no = 0
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO courses (id,name,sections,needsComputers) values (" + course.id + "," + course.name + "," + course.sections + "," + computers + ");";
            cmd.ExecuteNonQuery();
        }
        public void addCanTeach(int profid, int courseid)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO professorscourses (professorid,courseid) values (" + profid + "," + courseid + ");";
            cmd.ExecuteNonQuery();
        }
        public void addCourseBlock(CourseBlock block)
        {

        }
    }
}
