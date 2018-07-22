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
    /// Interaction logic for ReviewPage.xaml
    /// </summary>
    public partial class ReviewPage : Page
    {
        public ReviewPage()
        {
            InitializeComponent();
        }

        public void review(List<Course> input)
        {
            Review_Listbox.Items.Clear();
            int count = 0;
            Review_Listbox.Items.Add(count + " |\tCourse Name\t|  Section Number\t|  Need Computers");
            foreach (Course output in input) //foreach in list
            {   //Add each element to listbox
                count++;
                //MessageBox.Show(output.name);
                Review_Listbox.Items.Add(count + " |\t          " + output.name + "   \t|             " + output.sections + "\t|             " + output.needsComputers);
            }
        }
        public void review(List<Room> input)
        {
            Review_Listbox.Items.Clear();
            int count = 0;
            Review_Listbox.Items.Add(count + " |\tBuilding Name\t|  Room Number\t|  Has Computers");
            foreach (Room output in input) //foreach in list
            {   //Add each element to listbox
                count++;
                Review_Listbox.Items.Add(count + " |\t          " + output.building + "   \t|             " + output.roomNum + "\t|             " + output.hasComputers);
            }
        }
        public void review(List<Professor> input)
        {
            Review_Listbox.Items.Clear();
            int count = 0;
            Review_Listbox.Items.Add(count + " |\tCourse Name\t|  Can Teach The Following");
            foreach (Professor output in input) //foreach in list
            {   //Add each element to listbox
                count++;
                Review_Listbox.Items.Add(count + " |\t          " + output.name + "   \t|             " + output.classList );
            }
        }

        /*
        private void reviewCourses_Click(object sender, RoutedEventArgs e)
        {
            //listbox format
            //course name, section, needcomputer
            Review_Listbox.Items.Clear();

            foreach(Course output in db.getAllCourse()) //foreach in list
            {   //Add each element to listbox
                Review_Listbox.Items.Add(output.name + ", " + output.sections + ", " + output.needsComputers);
            }
        }

        private void reviewRoom_Click(object sender, RoutedEventArgs e)
        {
            //listbox format
            //building, room, hascomputer
            Review_Listbox.Items.Clear();
            DBMethods db = new DBMethods(new System.Data.SQLite.SQLiteConnection("Data Source=:memory:"));
            foreach (Room output in db.getAllRoom()) //foreach in list
            {   //Add each element to listbox
                Review_Listbox.Items.Add(output.building + ", " + output.roomNum + ", " + output.hasComputers);
            }
        }

        private void reviewProf_Click(object sender, RoutedEventArgs e)
        {
            //listbox format
            //Last, first, [canteach]
            Review_Listbox.Items.Clear();
            DBMethods db = new DBMethods(new System.Data.SQLite.SQLiteConnection("Data Source=:memory:"));
            foreach (Professor output in db.getAllProfessor()) //foreach in list
            {   //Add each element to listbox
                Review_Listbox.Items.Add(output.name + ", [" + String.Join(",", db.getCanTeach(output.id).classList.ToArray()) + "]");
            }
            
        }*/



    }
}
