using System;
using System.Collections;
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
    /// Interaction logic for Courses.xaml
    /// </summary>
    public partial class Courses : Page
    {
        public Courses()
        {
            InitializeComponent();
        }

        ArrayList courses = new ArrayList();

        private void coursesAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (roomNum1.Text != "" && sectionNum1.Text != "")
            {
                if (coursesBox1.IsChecked == true)
                {
                    courses.Add(new Course())
                }
            }
        }
    }
}
