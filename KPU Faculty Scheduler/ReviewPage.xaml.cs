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
            //DBMethods db = new DBMethods();
            List<Course> temp = new List<Course>;
            temp = db.getAllRoom();
            //Review_Listbox.ItemsSource = temp;
            foreach(Course output in temp)
            {
                Review_Listbox.Items.Add(output.name+", "+output.sections+", "+output.needsComputers)
            }
        }

        private void reviewClass_Click(object sender, RoutedEventArgs e)
        {
            //listbox format
            //building, room, hascomputer
        }

        private void reviewProf_Click(object sender, RoutedEventArgs e)
        {
            //listbox format
            //Last, first, [canteach]
        }
    }
}
