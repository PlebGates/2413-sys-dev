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
            DBMethods db = new DBMethods(new System.Data.SQLite.SQLiteConnection("Data Source=:memory:"));
        }

        // Create new pages upon program launch
        CoursePage coursesPage = new CoursePage();
        RoomPage roomsPage = new RoomPage();
        ProfessorPage professorsPage = new ProfessorPage();
        

        // Upon clicking "Create", hide original buttons, and toggle visibility of the frame and stack panels
        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            createButton.Visibility = System.Windows.Visibility.Hidden;
            importButton.Visibility = System.Windows.Visibility.Hidden;
            navBarBack.Visibility = System.Windows.Visibility.Visible;
            navBarNext.Visibility = System.Windows.Visibility.Visible;
            mainFrame.Visibility = System.Windows.Visibility.Visible;
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
                mainFrame.Visibility = System.Windows.Visibility.Hidden;
                coursesPage = new CoursePage();
                roomsPage = new RoomPage();
                professorsPage = new ProfessorPage();
            }
            else if (mainFrame.Content == roomsPage)
            {
                mainFrame.Content = coursesPage;
            }
            else if (mainFrame.Content == professorsPage)
            {
                mainFrame.Content = roomsPage;
            }
        }
    }
}
