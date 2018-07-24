using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace KPU_Faculty_Scheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        // Create new pages upon program launch // Application resources
        CoursePage coursesPage = new CoursePage();
        RoomPage roomsPage = new RoomPage();
        ProfessorPage professorsPage = new ProfessorPage();
        ReviewPage reviewPage = new ReviewPage();
        Schedule schedule = new Schedule();
        SchedulePage schedulePage = new SchedulePage();

        DBMethods db = new DBMethods(new System.Data.SQLite.SQLiteConnection("Data Source=:memory:"));

        // Upon clicking "Create", hide original buttons, and toggle visibility of the frame and stack panels
        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            createButton.Visibility = System.Windows.Visibility.Hidden;
            importButton.Visibility = System.Windows.Visibility.Hidden;
            navBarBack.Visibility = System.Windows.Visibility.Visible;
            navBarNext.Visibility = System.Windows.Visibility.Visible;
            mainFrame.Visibility = System.Windows.Visibility.Visible;
            AddButton.Visibility = System.Windows.Visibility.Visible;
            mainFrame.Content = coursesPage;
        }

        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            //import excel
            // 1 - open file dialogue
            // 2 - open selected file
            // 3 - parse and import
        }

        // Upon clicking the next button, figure out which page is currently visible, and decide where to go from there
        private void nextClick(object sender, RoutedEventArgs e)
        {
            if (mainFrame.Content == coursesPage)
            {
                mainFrame.Content = roomsPage;
            }
            else if (mainFrame.Content == roomsPage)
            {
                mainFrame.Content = professorsPage;
                //populate the listbox for teach can teach on professorPage
                foreach (ListBox box in professorsPage.getlistboxes())
                {
                    foreach (Course c in db.getAllCourse())
                    {
                        if (!box.Items.Contains(c.name))
                        {
                            box.Items.Add(c.name);
                        }
                    }
                }
            }
            else if (mainFrame.Content == professorsPage)
            {
                AddButton.Visibility = System.Windows.Visibility.Hidden;
                reviewButtons.Visibility = System.Windows.Visibility.Visible;
                mainFrame.Content = reviewPage;

                List<Course> allCourses = db.getAllCourse();
                List<Professor> allProfessors = db.getAllProfessor();
                List<ListBox> box = professorsPage.getlistboxes();

                // Populates canTeach TABLE
                for(int i = 0; i < 10; i++)
                {
                    Professor professor = new Professor();//to ensure professor isnt broken every new professor object is made
                    foreach(object selected in box[i].SelectedItems) {
                            foreach(Course c in allCourses)
                            {
                                if(c.name == selected.ToString())
                                {
                                db.addCanTeach(allProfessors[i].id, c.id);
                                break; //found a match goto next selected course
                                }
                            }
                        }
                }
            }
            else if (mainFrame.Content == reviewPage)
            {
                schedule.CreateSchedule(db.getAllProfessor(),db.getAllCourse(),db.getAllRoom());
                foreach(CourseBlock block in schedule.classList)
                {
                    db.addCourseBlock(block);
                    
                }

                mainFrame.Content = schedulePage;
                reviewButtons.Visibility = System.Windows.Visibility.Hidden;
                schedulePageGrid.Visibility = System.Windows.Visibility.Visible;
                loadSchedulePage(); //load scheulde page days -- needs another function here to load created scheulde on dataGrid
            }
        }
        //END next-----------------

        //moved here from ReviewPage
        public void reviewProfesser(List<Professor> input)
        {
            reviewPage.Review_Listbox.Items.Clear();
            int count = 0;
            reviewPage.Review_Listbox.Items.Add(count + " |\tProfessor Name\t|  Can Teach The Following");
            foreach (Professor profInput in input) //foreach in list
            {
                List<String> listOfTeachable = db.getCanTeach(profInput.id);
                
                //Add each element to listbox
                count++;
                reviewPage.Review_Listbox.Items.Add(count + " |\t  " + profInput.name + "   \t|             " + string.Join(", ", listOfTeachable));
            }
        }


        // Upon clicking the back button, figure out which page is currently visible, and decide where to go from there
        private void backClick(object sender, RoutedEventArgs e)
        {
            // If returning to the main menu, delete all object content
            if (mainFrame.Content == coursesPage)
            {
                createButton.Visibility = System.Windows.Visibility.Visible;
                importButton.Visibility = System.Windows.Visibility.Visible;
                navBarBack.Visibility = System.Windows.Visibility.Hidden;
                navBarNext.Visibility = System.Windows.Visibility.Hidden;
                mainFrame.Content = " ";
                AddButton.Visibility = System.Windows.Visibility.Hidden;
                mainFrame.Visibility = System.Windows.Visibility.Hidden;
                coursesPage = new CoursePage();
                roomsPage = new RoomPage();
                professorsPage = new ProfessorPage();
                reviewPage = new ReviewPage();
            }
            else if (mainFrame.Content == roomsPage)
            {
                mainFrame.Content = coursesPage;
            }
            else if (mainFrame.Content == professorsPage)
            {
                mainFrame.Content = roomsPage;
            }
            else if (mainFrame.Content == reviewPage)
            {
                AddButton.Visibility = System.Windows.Visibility.Visible;
                reviewButtons.Visibility = System.Windows.Visibility.Hidden;
                mainFrame.Content = professorsPage;
            }
            else if (mainFrame.Content == schedulePage)
            {
                mainFrame.Content = reviewPage;
                reviewButtons.Visibility = System.Windows.Visibility.Visible;
                schedulePageGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        //add elements from x page to DB, only visibile on input pages
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(mainFrame.Content == coursesPage)
            {

                foreach(Course course_ in coursesPage.addInput())
                {
                    db.addCourse(course_);
                }
                /*List<Course> test = db.getAllCourse();
                foreach(Course temp in test)
                {
                    MessageBox.Show("you have entered in course: " + temp.name, "IT WORKED!",MessageBoxButton.OK);
                }*/
            }
            else if (mainFrame.Content == roomsPage)
            {
                foreach (Room room_ in roomsPage.addInput())
                {
                    db.addRoom(room_);
                }
            }
            else if (mainFrame.Content == professorsPage)
            {

                foreach (Professor Prof_ in professorsPage.addInput())
                {
                    db.addProfessor(Prof_);
                    


                }
            }
        }

        private void review_Click_Course(object sender, RoutedEventArgs e)
        {
            reviewPage.reviewCourse(db.getAllCourse());
        }

        private void review_Click_Room(object sender, RoutedEventArgs e)
        {
            reviewPage.reviewRoom(db.getAllRoom());

        }

        private void review_Click_Professor(object sender, RoutedEventArgs e)
        {
            reviewProfesser(db.getAllProfessor());
        }

        private void SwapButton_Click(object sender, RoutedEventArgs e)
        {
            if(course1_ComboBox.SelectedItem != null && course2_ComboBox.SelectedItem != null)
            {
                CourseBlock a = getAllBlocksOnDay(day1_ComboBox.SelectedItem.ToString())[course1_ComboBox.SelectedIndex];
                CourseBlock b = getAllBlocksOnDay(day1_ComboBox.SelectedItem.ToString())[course1_ComboBox.SelectedIndex];
                schedule.swapCourseBlock(a, b);
            }
            
        }

        /* Time blocks
        * M |T |W |T |F |S |S
        * 0 |5 |0 |5 |18|18
        * 1 |6 |10|14|19|23
        * 2 |7 |11|15|20|24
        * 3 |8 |12|16|21|25
        * 4 |9 |13|17|22|26
        */
        //schedulePage stuff
        private static int[] monday = { 0, 1, 2, 3, 4 };
        private static int[] tuseday = { 5, 6, 7, 8, 9 };
        private static int[] wednesday = { 0, 10, 11, 12, 13 };
        private static int[] thursday = { 5, 14, 15, 16, 17 };
        private static int[] friday = { 18, 19, 20, 21, 22 };
        private static int[] saturday = { 18, 23, 24, 25, 26 };
        //creates a Dictionary/Map to connect to the arrays for time
        Dictionary<String, int[]> map = new Dictionary<string, int[]>()
            {
                {"Monday", monday},
                {"Tuseday", tuseday},
                {"Wednesday", wednesday},
                {"Thursday", thursday},
                {"Friday", friday},
                {"Saturday", saturday},
            };
        Dictionary<int, String> timeSchedule = new Dictionary<int, String>()
            {
                {0, "8:30am - 10am"},{5, "8:30am - 10am"},{18, "8:30am - 10am"},
                {1, "10pm - 12:50pm"},{6, "10pm - 12:50pm"},{10, "10pm - 12:50pm"},{14, "10pm - 12:50pm"},{19, "10pm - 12:50pm"},{23, "10pm - 12:50pm"},
                {2, "1pm - 3:50pm"},{7, "1pm - 3:50pm"},{11, "1pm - 3:50pm"},{15, "1pm - 3:50pm"},{20, "1pm - 3:50pm"},{24, "1pm - 3:50pm"},
                {3, "3pm - 6:50pm"},{8, "3pm - 6:50pm"},{12, "3pm - 6:50pm"},{16, "3pm - 6:50pm"},{21, "3pm - 6:50pm"},{25, "3pm - 6:50pm"},
                {4, "7pm - 9:50pm"},{9, "7pm - 9:50pm"},{13, "7pm - 9:50pm"},{17, "7pm - 9:50pm"},{22, "7pm - 9:50pm"},{26, "7pm - 9:50pm"},
            };

        public void loadSchedulePage()
        {
            day1_ComboBox.Items.Clear();
            day2_ComboBox.Items.Clear();
            //populate the DAYS combobox for user to select what day they want to change
            foreach (String day in map.Keys)
            {
                day1_ComboBox.Items.Add(day);
                day2_ComboBox.Items.Add(day);
            }
        }
        //when user clicks on combo box and selects a item it will populate coursescombobox2

        public List<String> getBlocksOnDay(String day)
        {

            List<String> courseDetails = new List<String>();
            List<Room> roomList = db.getAllRoom();
            foreach (int time in map[day])//check if the block.time is on x day ex."monday"
            {
                List<Room> consumedRooms = new List<Room>();//new list of consumed rooms
                CourseBlock blockAtThisTime = new CourseBlock();
                foreach (Room room in roomList)
                {
                    bool roomInUse = false;
                    foreach (CourseBlock block in db.getAllCourseBlockTime(time)) //check courses for that time
                    {
                        Room thisRoom = db.getRoom(block.room.id);
                        Course thisCourse = db.getCourse(block.course.id);
                        Professor thisProf = db.getProfessor(block.professor.id);
                        MessageBox.Show("checking block:" + block.id + "this block time: " + block.time + "for this time: " + time);
                        if (room.id == thisRoom.id && time == block.time) //if block is on this time and room
                        {
                            roomInUse = true;
                            MessageBox.Show("MATCH!");
                        }
                        if (roomInUse)
                        {
                            //[8am - 10pm,  cedar 1045, INFO1213, jendy lee] //has teacher and course
                            //[8am - 10pm, cedar 1045] //no teacher and course
                            if (thisRoom.hasComputers && room.hasComputers)
                            {
                                //[8am - 10pm,  cedar 1045, INFO1213, jendy lee] //has teacher and course
                                courseDetails.Add(timeSchedule[time] + ", (c)" + thisRoom.building + thisRoom.roomNum + ", " + thisCourse.name + " S" + thisCourse.sections + ", " + thisProf.name);
                                break;
                            }
                            else//this room has no computers
                            {
                                //[8am - 10pm, cedar 1045] //no teacher and course
                                courseDetails.Add(timeSchedule[time] + ", " + thisRoom.building + thisRoom.roomNum + ", " + thisCourse.name + " S" + thisCourse.sections + ", " + thisProf.name);
                                break;
                            }
                        }
                        //not a match, keep looking
                        else
                        {
                            MessageBox.Show("no MAtch!");
                            continue;
                        }
                    }
                    if (!roomInUse)
                    { //no course for this room
                        courseDetails.Add(timeSchedule[time] + ", " + room.building + room.roomNum + "no course, no teacher");
                        continue;
                    }
                    
                }
                
            }

            return courseDetails;
        }

        public List<CourseBlock> getAllBlocksOnDay(String day)
        {

            List<CourseBlock> courseDetails = new List<CourseBlock>();
            List<Room> roomList = db.getAllRoom();
            foreach (int time in map[day])//check if the block.time is on x day ex."monday"
            {
                foreach (Room room in roomList)
                {
                    foreach (CourseBlock block in db.getAllCourseBlockTime(time)) //check courses for that time
                    {
                        if (room == block.room && time == block.time) //if block is on this time and room
                        {
                            courseDetails.Add(block);
                            break;
                        }
                    }
                }
            }
            return courseDetails;
        }



        private void day2_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            course2_ComboBox.Items.Clear();
            foreach (String courseDetails in getBlocksOnDay(day2_ComboBox.SelectedItem.ToString()))
            {
                course2_ComboBox.Items.Add(courseDetails);
            }
        }

        private void day1_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            course1_ComboBox.Items.Clear();
            foreach (String courseDetails in getBlocksOnDay(day1_ComboBox.SelectedItem.ToString()))
            {
                course1_ComboBox.Items.Add(courseDetails);
            }
        }
    }
}
