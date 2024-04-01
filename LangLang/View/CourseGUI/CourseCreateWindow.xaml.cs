using LangLang.Core.Model;
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
using System.Windows.Shapes;

namespace LangLang.View.CourseGUI
{
    /// <summary>
    /// Interaction logic for CourseCreateWindow.xaml
    /// </summary>
    public partial class CourseCreateWindow : Window
    {
        public Tutor tutor { get; set; }
        public CourseCreateWindow(Tutor t)
        {
            tutor = t;
            InitializeComponent();
        }

        private void CourseCreateBtn_Click(object sender, RoutedEventArgs e)
        {
            string language = languageTb.Text;
            string numOfWeeksStr = numOfWeeksTb.Text;
            string maxStudentsStr = maxNumOfStudentsTb.Text;
            string timeStr = classTimeTb.Text;
            string[] timeParts = timeStr.Split(':');
            if (timeParts.Length != 2)
            {
                // add error bad time format
            }
            ComboBoxItem selectedLevel = languageLvlCb.SelectedItem as ComboBoxItem;
            DateTime? date = startDateDp.SelectedDate;
            if(!date.HasValue)
            {
                // add error for no date selected
            }
            List<DayOfWeek> days = new List<DayOfWeek>();
            if (mon.IsChecked == true)
            {
                days.Add(DayOfWeek.Monday);
            }
            if (tue.IsChecked == true)
            {
                days.Add(DayOfWeek.Tuesday);
            }
            if (wed.IsChecked == true)
            {
                days.Add(DayOfWeek.Wednesday);
            }
            if (thu.IsChecked == true)
            {
                days.Add(DayOfWeek.Thursday);
            }
            if (fri.IsChecked == true)
            {
                days.Add(DayOfWeek.Friday);
            }
            else
            {
                // add error for no days selected
            }

            if (selectedLevel != null)
            {
                try
                {
                    LanguageLevel level = (LanguageLevel)Enum.Parse(typeof(LanguageLevel), selectedLevel.Content.ToString());
                    int maxStudents;
                    if (classsroomCb.IsChecked == true)
                    {
                        maxStudents = int.Parse(numOfWeeksStr);
                    }
                    else if (maxStudentsStr == "")
                    {
                        maxStudents = 0;
                    }
                    else
                    {
                        maxStudents = int.Parse(numOfWeeksStr);
                    }
                    int numOfWeeks = int.Parse(numOfWeeksStr);
                    int hour = int.Parse(timeParts[0]);
                    int minute = int.Parse(timeParts[1]);
                    if(hour < 0 || hour > 24 || minute < 0 || minute > 59)
                    {
                        // add error for time
                    }
                    Course course = new Course();
                    course.Language = language;
                    course.Level = level;
                    course.NumberOfWeeks = numOfWeeks;
                    course.Days = days;
                    course.MaxStudents = maxStudents;
                    course.NumberOfStudents = 0;
                    course.Online = !(bool)classsroomCb.IsChecked;
                    course.StartDateTime = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, hour, minute, 0);
                    course.TutorId = tutor.Id;
                    course.CreatedByDirector = false;
                    
                }
                catch (FormatException ex)
                {
                    // int parse
                }
                catch (ArgumentException ex)
                {
                    // enum parse
                }
            }
            else
            {
                // add error for no language level selected
            }
        }

        // Method enables textbox for maxNumOfStudents when the course is to be held in a classroom

        private void ClasssroomCb_Checked(object sender, RoutedEventArgs e)
        {
            maxNumOfStudentsTb.IsEnabled = true;
        }

        private void ClasssroomCb_Unchecked(object sender, RoutedEventArgs e)
        {
            maxNumOfStudentsTb.IsEnabled = false;
        }
    }
}
