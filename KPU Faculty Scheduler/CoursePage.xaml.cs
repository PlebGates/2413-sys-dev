﻿using System;
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
    public partial class CoursePage : Page
    {
        public CoursePage()
        {
            InitializeComponent();
        }
        public CoursePage(int i)
        {

        }
        public List<Course> addInput()
        {

            // block of OBJECTS to simplify code
            List<TextBox> inputboxes1 = new List<TextBox> { courseNum1, courseNum2, courseNum3, courseNum4, courseNum5, courseNum6, courseNum7, courseNum8, courseNum9, courseNum10};
            List<TextBox> inputboxes2 = new List<TextBox> { sectionNum1, sectionNum2, sectionNum3, sectionNum4, sectionNum5, sectionNum6, sectionNum7, sectionNum8, sectionNum9, sectionNum10 };
            List<CheckBox> inputboxes3 = new List<CheckBox> { coursesCheckBox1, coursesCheckBox2, coursesCheckBox3, coursesCheckBox4, coursesCheckBox5, coursesCheckBox6, coursesCheckBox7, coursesCheckBox8, coursesCheckBox9, coursesCheckBox10 };
            
            List<Course> validInput = new List<Course> { };
            for (int i = 0; i < 10; i++) {
                Course course = new Course();
                if (inputboxes1[i].Text != "" && inputboxes2[i].Text != "")
                {
                    Course course = new Course();
                    try
                    {
                        
                        course.name = inputboxes1[i].Text;
                        course.sections = Convert.ToInt32(inputboxes2[i].Text);
                        course.needsComputers = (bool)inputboxes3[i].IsChecked;
                        validInput.Add(course);
                    }
                    catch (Exception e) {/*maybe grab a list of empty or incomplete textboxes*/  }
                }
            }
            return validInput;
        }


    }
}
