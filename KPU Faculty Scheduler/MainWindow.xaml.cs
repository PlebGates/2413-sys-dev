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
        }

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
        }
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
    }
}
