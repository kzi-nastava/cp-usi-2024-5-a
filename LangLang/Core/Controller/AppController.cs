﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Core.Controller
{
    class AppController
    {
        private TutorController _tutorController;
        private CourseController _courseController;
        private StudentController _studentController;
        private EnrollmentRequestController _enrollmentRequestController;
        private ExamSlotController _examSlotController;
        public AppController()
        {
            this._tutorController = new TutorController();
            this._courseController = new CourseController();
            this._studentController = new StudentController();
            this._enrollmentRequestController = new EnrollmentRequestController();
            this._examSlotController = new ExamSlotController();
        }

        public TutorController TutorController
        {
            get { return this._tutorController; }
            set { this._tutorController = value; }
        }

        public CourseController CourseController
        {
            get { return this._courseController; }
            set { this.CourseController = value; }
        }

        public StudentController StudentController
        {
            get { return this._studentController; }
            set { this._studentController = value; }
        }

        public EnrollmentRequestController EnrollmentRequestController
        {
            get { return this._enrollmentRequestController; }
            set { this._enrollmentRequestController = value; }
        }

        public ExamSlotController ExamSlotController
        {
            get { return this._examSlotController; }
            set { this._examSlotController = value; }
        }
    }
}