﻿using System;
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
    /// Interaction logic for Professors.xaml
    /// </summary>
    public partial class ProfessorPage : Page
    {
        public ProfessorPage()
        {
            InitializeComponent();
        }
        public List<ListBox> getlistboxes()
        {
            List<ListBox> inputboxes3 = new List<ListBox> { teachListbox1, teachListbox2, teachListbox3, teachListbox4, teachListbox5, teachListbox6, teachListbox7, teachListbox8, teachListbox9, teachListbox10 };
            return inputboxes3;
        }

        public List<Professor> addInput()
        {

            // block of OBJECTS to simplify code
            List<TextBox> inputboxes1 = new List<TextBox> { lastName1, lastName2, lastName3, lastName4, lastName5, lastName6, lastName7, lastName8, lastName9, lastName10 };
            List<TextBox> inputboxes2 = new List<TextBox> { firstName1, firstName2, firstName3, firstName4, firstName5, firstName6, firstName7, firstName8, firstName9, firstName10 };
            // List<TextBox> inputboxes3 = new List<TextBox> { roomNum1, roomNum2, roomNum3, roomNum4, roomNum5, roomNum6, roomNum7, roomNum8, roomNum9, roomNum10 };
            
            List<Professor> validInput = new List<Professor> { };
            int countValid = 0;
            int incomplete = 0;
            for (int i = 0; i < 10; i++)
            {
                if (inputboxes1[i].Text != "" && inputboxes2[i].Text != "")
                {
                    Professor Professor = new Professor();
                    try
                    {
                        Professor.name = inputboxes1[i].Text + " " + inputboxes2[i].Text;
                        //Professor.classList = Convert.ToInt32(inputboxes2[i].Text);
                        validInput.Add(Professor);
                        countValid++;
                        //reset boxes for every line
                        inputboxes1[i].Text = "";
                        inputboxes2[i].Text = "";
                    }
                    catch (Exception e) {/*maybe grab a list of empty or incomplete textboxes*/ incomplete++; }
                }
                else if (inputboxes1[i].Text != "" || inputboxes2[i].Text != "")
                {
                    incomplete++;
                }
                //reset boxes for every line
                inputboxes1[i].Text = "";
                inputboxes2[i].Text = "";
            }
            statusLabel.Text = (incomplete == 0) ? countValid + " valid entries" : countValid + " valid entries, " + incomplete + " incomplete entries";
            return validInput;
        }
    }
}
