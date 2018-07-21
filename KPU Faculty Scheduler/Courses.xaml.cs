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

        // List to hold all course objects
        List<Course> courseList = new List<Course>();
        int courseCounter = -1;

        // Add button to create course objects and add to list
        private void coursesAddButton_Click(object sender, RoutedEventArgs e)
        {
            // If both textboxes in a row are not empty
            if (courseNum1.Text != "" && sectionNum1.Text != "")
            {
                // And that rows checkbox is not checked
                if (coursesBox1.IsChecked != true)
                {
                    // Create and store a course object in the courseList list with false
                    courseList.Add(new Course(courseNum1.Text, Int32.Parse(sectionNum1.Text), false));
                    courseCounter++;
                }
                // If the checkbox is checked, create and store with true
                else
                {
                    courseList.Add(new Course(courseNum1.Text, Int32.Parse(sectionNum1.Text), true));
                    courseCounter++;
                }
            }
            // If one box is empty, popup stating so
            else
            {
                 MessageBox.Show("A text field on line 1 was left empty");
            }


            // Reapeat the above
            if (courseNum2.Text != "" && sectionNum2.Text != "")
            {
                if (coursesBox2.IsChecked != true)
                {
                    courseList.Add(new Course(courseNum2.Text, Int32.Parse(sectionNum2.Text), false));
                    courseCounter++;
                }
                else
                {
                    courseList.Add(new Course(courseNum2.Text, Int32.Parse(sectionNum2.Text), true));
                    courseNum7.Text = courseList[1].getName();
                    sectionNum7.Text = courseList[1].getSection().ToString();
                    coursesBox7.IsChecked = courseList[1].getComp();
                    courseCounter++;
                }
            }
            // Remove the last object added and allow them to fix their error, decrement the counter
            else
            {
                MessageBox.Show("A text field on line 2 was left empty");
                courseList.RemoveAt(courseCounter);
                courseCounter--;
            }
        }
    }
}
