using System;
using System.Diagnostics;
using System.IO;
using LangLang.Aplication.UseCases;
using LangLang.Core;
using LangLang.Core.Controller;
using LangLang.Core.Model;
using LangLang.Domain.Models;

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
            int pointsToRemove = ((DateTime.Now.Year - LastReduced.Year) * 12) + DateTime.Now.Month - LastReduced.Month;
            LastReduced = currentMonth;

            var studentService = new StudentService();
            foreach (var student in studentService.GetAll())
            {
                Remove(pointsToRemove, student, appController);
            }

            Write();
        }
    }
    public void Remove(int toRemove, Student student, AppController appController)
    {
        for(int i = 0; i<toRemove; i++)
        {
            appController.PenaltyPointController.RemovePenaltyPoint(student);
        }
    }
}
