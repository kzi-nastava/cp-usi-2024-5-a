using System;
using System.Diagnostics;
using System.IO;
using LangLang.Core;
using LangLang.Core.Controller;

public class PenaltyPointReducer
{
    private string fileName;
    public DateTime LastReduced { get; set; }

    public PenaltyPointReducer()
    {
        fileName = Constants.REDUCER_FILE_NAME;
    }

    public PenaltyPointReducer(DateTime lastReduced)
    {
        LastReduced = lastReduced;
    }

    public PenaltyPointReducer Load()
    {
        if (!File.Exists(fileName))
        {
            return new PenaltyPointReducer();
        }
        DateTime dateTime = DateTime.MinValue;
        if (File.Exists(fileName))
        {
            string content = File.ReadAllText(fileName);
            DateTime.TryParse(content, out dateTime);
        }
        return new PenaltyPointReducer(dateTime);
    }

    public void Write()
    {
        File.WriteAllText(fileName, LastReduced.ToString());
    }

    public void UpdatePenaltyPoints(AppController appController)
    {
        DateTime currentMonth = DateTime.Today;
        if (currentMonth.Month != LastReduced.Month)
        {
            // Update last reduced to current month
            LastReduced = currentMonth;

            // Call RemovePenaltyPoint for each student
            foreach (var student in appController.StudentController.GetAllStudents())
            {
                appController.PenaltyPointController.RemovePenaltyPoint(student);
            }

            Write();
        }
        Trace.WriteLine("reducer pozvan");
    }
}
