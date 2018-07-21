using System;
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
                Professor prof = new Professor();
                while(data.Read())
                {
                    prof.id = data.GetInt32(0);
                    prof.name = data.GetString(1);
                    return prof;
                }
                
            }
            return null;
            //return new Prof(data.GetInt32(0), data.GetString(1));
        }
        public bool canTeach(int teachID, int classID)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from professorsclass where teachID = " + teachID + " and classID = " + classID;
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
        public Room getRoom(int id)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from rooms where id = " + id;
            Room room = new Room();
            using (SQLiteDataReader data = cmd.ExecuteReader())
            {
                while (data.Read())
                {
                    room.id = data.GetInt32(0);
                    room.roomNum = data.GetInt32(1);
                    room.hasComputers = (data.GetInt32(3) == 1);
                    return room;
                }
                return null;
            }
                
            //return new Room(data.GetInt32(0), data.GetString(1), data.GetBoolean(3));
        }
        public Course getCourse(int id)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from rooms where id = " + id;
            Course course = new Course();
            using (SQLiteDataReader data = cmd.ExecuteReader())
            {
                while (data.Read())
                {
                    course.id = data.GetInt32(0);

                    // NOT SURE IF THIS IS CORRECT VINCENT, ALSO SEE LINE 156
                    course.name = data.GetString(1);
                    course.needsComputers = (data.GetInt32(3) == 1);
                    return course;
                }
            }
            return null;
                
            //return new Room(data.GetInt32(0), data.GetString(1), data.GetBoolean(3));
        }
        public CourseBlock getBlock(int id)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select * from schedule where id = " + id;
            CourseBlock block = new CourseBlock();
            using (SQLiteDataReader data = cmd.ExecuteReader())
            {
                while (data.Read())
                {
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

        public void addProfessor(Professor prof)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO professors (id,name) values (" + prof.id + ",'" + prof.name + "');";
            cmd.ExecuteNonQuery();
        }
        public void addRoom(Room room)
        {
            int computers = room.hasComputers ? 1 : 0; //room has computers? yes = 1, no = 0
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO rooms (id,name,hasComputers) values (" + room.id + ",'" + room.roomNum + "'," + computers + ");";
            cmd.ExecuteNonQuery();
        }
        public void addCourse(Course course)
        {
            int computers = course.needsComputers ? 1 : 0; //room has computers? yes = 1, no = 0
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO courses (id,name,sections,needsComputers) values (" + course.id + ",'" + course.name + "'," + course.sections + "," + computers + ");";
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
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO schedule (id,professorid,courseid,roomid,time) values (" + block.id + "," + block.professor.id +
                "," + block.course.id + "," + block.room.id + "," + block.time + ");";
            cmd.ExecuteNonQuery();
        }
    }
}
