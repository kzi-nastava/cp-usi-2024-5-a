
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
            //availableExamsTab.ExamSlots.Clear(); // TOOD: move this logic to AvailableExamsViewModel
            //foreach (var exam in availableExamsTab.ExamsForReview)
                //availableExamsTab.ExamSlots.Add(new ExamSlotDTO(exam));
        }

        public void UpdateExamApplications(ExamApplications examApplicationsTab)
        {
            //examApplicationsTab.Applications.Clear(); // TOOD: move this logic to ExamAplicationsViewModel
            //foreach (var app in examApplicationsTab.ApplicationsForReview)
            //    examApplicationsTab.Applications.Add(new ExamApplicationDTO(app));
        }

        public void UpdateEnrollmentRequests(EnrollmentRequests enrollmentRequestsTab)
        {
            enrollmentRequestsTab.Update();
        }
    }
}
