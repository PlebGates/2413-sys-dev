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
    /// Interaction logic for SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        public SchedulePage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            courseBlockListbox1.Items.Add("hello world");
        }

        private void courseBlockListbox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;

            courseBlockListbox1.Items.Add(e.Data.GetData(DataFormats.Text));
        }

        private void courseBlockListbox2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;

            courseBlockListbox2.Items.Add(e.Data.GetData(DataFormats.Text));
        }
    }
}
