using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using LangLang.WPF.Views.TutorView.Tabs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LangLang.WPF.ViewModels.CourseViewModels
{
    public class CoursesTutorViewModel
    {
        private ObservableCollection<CourseViewModel> pagedCourses;
        private int currentPage;
        private int pageSize = 3;

        private List<Course> courses;

        public CourseViewModel SelectedCourse { get; set; }
        public Tutor LoggedIn { get; set; }
        public ObservableCollection<CourseViewModel> PagedCourses
        {
            get => pagedCourses;
            set
            {
                pagedCourses = value;
                OnPropertyChanged(nameof(PagedCourses));
            }
        }

        public int CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                OnPropertyChanged(nameof(currentPage));
                LoadPageData();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CoursesTutorViewModel(Tutor loggedIn)
        {
            this.LoggedIn = loggedIn;
            pagedCourses = new ObservableCollection<CourseViewModel>();
            courses = new List<Course>();
            currentPage = 1;
            SetDataForReview();
        }
        public void SetDataForReview()
        {
            courses = LoadAllCourses();
            LoadPageData();
        }
        private List<Course> LoadAllCourses()
        {
            List<Course> result = new();
            CourseService courseService = new();
            foreach (Course course in courseService.GetByTutor(LoggedIn.Id))
            {
                result.Add(course);
            }
            return result;
        }
        public void GoToFirstPage()
        {
            CurrentPage = 1;
        }

        public void GoToPreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }

        public void GoToNextPage()
        {
            if (CurrentPage < (courses.Count + pageSize - 1) / pageSize)
            {
                CurrentPage++;
            }
        }

        public void GoToLastPage()
        {
            CurrentPage = (courses.Count + pageSize - 1) / pageSize;

        }
        private void LoadPageData()
        {
            PagedCourses.Clear();
            var pagedData = courses.Skip((currentPage - 1) * pageSize).Take(pageSize);
            foreach (var course in pagedData)
            {
                PagedCourses.Add(new CourseViewModel(course));
            }
        }
    }
}
