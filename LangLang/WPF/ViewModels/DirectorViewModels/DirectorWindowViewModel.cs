using LangLang.WPF.Views.DirectorView.Tabs;

namespace LangLang.WPF.ViewModels.DirectorViewModels
{
    public class DirectorWindowViewModel
    {
        public DirectorWindowViewModel() {}

        public void UpdateCourses(CoursesReview coursesTab)
        {
            coursesTab.Update();
        }

        public void UpdateExams(ExamSlotsReview examsTab)
        {
            examsTab.Update();
        }

    }
}
