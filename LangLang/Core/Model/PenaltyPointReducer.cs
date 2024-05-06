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
        Load();
    }

    public PenaltyPointReducer(DateTime lastReduced)
    {
        LastReduced = lastReduced;
    }

    public void Load()
    {
        if (!File.Exists(fileName))
        {
            return;
        }
        DateTime dateTime = DateTime.MinValue;
        if (File.Exists(fileName))
        {
            string content = File.ReadAllText(fileName);
            DateTime.TryParse(content, out dateTime);
            LastReduced = dateTime;
        }
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
            LastReduced = currentMonth;

            foreach (var student in appController.StudentController.GetAll())
            {
                appController.PenaltyPointController.RemovePenaltyPoint(student);
            }

            Write();
        }
    }
}
