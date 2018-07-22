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
    /// Interaction logic for Rooms.xaml
    /// </summary>
    public partial class RoomPage : Page
    {
        public RoomPage()
        {
            InitializeComponent();
        }
        public List<Room> addInput()
        {

            // block of OBJECTS to simplify code
            List<TextBox> inputboxes1 = new List<TextBox> { buildingName1, buildingName2, buildingName3, buildingName4, buildingName5, buildingName6, buildingName7, buildingName8, buildingName9, buildingName10 };
            List<TextBox> inputboxes2 = new List<TextBox> { roomNum1, roomNum2, roomNum3, roomNum4, roomNum5, roomNum6, roomNum7, roomNum8, roomNum9, roomNum10 };
            List<CheckBox> inputboxes3 = new List<CheckBox> { roomCheckBox1, roomCheckBox2, roomCheckBox3, roomCheckBox4, roomCheckBox5, roomCheckBox6, roomCheckBox7, roomCheckBox8, roomCheckBox9, roomCheckBox10 };
            Room room = new Room();
            List<Room> validInput = new List<Room> { };
            for (int i = 0; i < 10; i++)
            {
                if (inputboxes1[i].Text != "" && inputboxes2[i].Text != "")
                {
                    try
                    {
                        room.building = inputboxes1[i].Text;
                        room.roomNum = Convert.ToInt32(inputboxes2[i].Text);
                        room.hasComputers = (bool)inputboxes3[i].IsChecked;
                        validInput.Add(room);
                    }
                    catch (Exception e) { /*maybe grab a list of empty or incomplete textboxes*/ }
                }
            }
            return validInput;
        }
    }
}
