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

        private void reviewCourses_Click(object sender, RoutedEventArgs e)
        {
            //listbox format
            //course name, section, needcomputer
            Review_Listbox.Items.Clear();
            DBMethods db = new DBMethods(new System.Data.SQLite.SQLiteConnection("Data Source=:memory:"));
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
            
        }



    }
}
