
using LangLang.View.StudentGUI.Tabs;
using LangLang.WPF.Views.StudentView.Tabs;

namespace LangLang.WPF.ViewModels.StudentViewModels
{
    public class StudentWindowViewModel
    {

        public StudentWindowViewModel()
        {
            
        }

        public void UpdateAvailableCourses(AvailableCourses availableCoursesTab)
        {
            availableCoursesTab.Update();    
        }

        public void UpdateAvailableExams(AvailableExams availableExamsTab)
        {
            availableExamsTab.Update();
       
        }

        public void UpdateExamApplications(ExamApplications examApplicationsTab)
        {
            examApplicationsTab.Update();
            
        }

        public void UpdateEnrollmentRequests(EnrollmentRequests enrollmentRequestsTab)
        {
            enrollmentRequestsTab.Update();
        }
    }
}
