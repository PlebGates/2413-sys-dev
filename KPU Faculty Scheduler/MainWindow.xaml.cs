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

        // Create new pages upon program launch
        Courses page1 = new Courses();
        Rooms page2 = new Rooms();
        Professors page3 = new Professors();

        // Upon clicking "Create", hide original buttons, and toggle visibility of the frame and stack panels
        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            createButton.Visibility = System.Windows.Visibility.Hidden;
            importButton.Visibility = System.Windows.Visibility.Hidden;
            navBarBack.Visibility = System.Windows.Visibility.Visible;
            navBarNext.Visibility = System.Windows.Visibility.Visible;
            mainFrame.Visibility = System.Windows.Visibility.Visible;
            mainFrame.Content = page1;
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
            if (mainFrame.Content == page1)
            {
                mainFrame.Content = page2;
            }
            else if (mainFrame.Content == page2)
            {
                mainFrame.Content = page3;
            }
        }

        // Upon clicking the back button, figure out which page is currently visible, and decide where to go from there
        private void backClick(object sender, RoutedEventArgs e)
        {
            // If returning to the main menu, delete all object content
            if (mainFrame.Content == page1)
            {
                createButton.Visibility = System.Windows.Visibility.Visible;
                importButton.Visibility = System.Windows.Visibility.Visible;
                navBarBack.Visibility = System.Windows.Visibility.Hidden;
                navBarNext.Visibility = System.Windows.Visibility.Hidden;
                mainFrame.Content = " ";
                mainFrame.Visibility = System.Windows.Visibility.Hidden;
                page1 = new Courses();
                page2 = new Rooms();
                page3 = new Professors();
            }
            else if (mainFrame.Content == page2)
            {
                mainFrame.Content = page1;
            }
            else if (mainFrame.Content == page3)
            {
                mainFrame.Content = page2;
            }
        }
    }
}
