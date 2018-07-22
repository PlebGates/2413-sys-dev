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

        public void reviewCourse(List<Course> input)
        {
            Review_Listbox.Items.Clear();
            int count = 0;
            Review_Listbox.Items.Add(count + " |\tCourse Name\t|  Section Number\t|  Need Computers");
            foreach (Course courseInput in input) //foreach in list
            {   //Add each element to listbox
                count++;
                //MessageBox.Show(output.name);
                Review_Listbox.Items.Add(count + " |\t          " + courseInput.name + "   \t|             " + courseInput.sections + "\t|             " + courseInput.needsComputers);
            }
        }
        public void reviewRoom(List<Room> input)
        {
            Review_Listbox.Items.Clear();
            int count = 0;
            Review_Listbox.Items.Add(count + " |\tBuilding Name\t|  Room Number\t|  Has Computers");
            foreach (Room roomInput in input) //foreach in list
            {   //Add each element to listbox
                count++;
                Review_Listbox.Items.Add(count + " |\t          " + roomInput.building + "   \t|             " + roomInput.roomNum + "\t|             " + roomInput.hasComputers);
            }
        }
        public void reviewProfesser(List<Professor> input)
        {
            Review_Listbox.Items.Clear();
            int count = 0;
            Review_Listbox.Items.Add(count + " |\tProfessor Name\t|  Can Teach The Following");
            foreach (Professor profInput in input) //foreach in list
            {   //Add each element to listbox
                count++;
                Review_Listbox.Items.Add(count + " |\t          " + profInput.name + "   \t|             " + profInput.classList );
            }
        }
    }
}
