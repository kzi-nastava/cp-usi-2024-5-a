using LangLang.BusinessLogic.UseCases;
using LangLang.Domain.Models;
using System;
using System.Windows;

namespace LangLang.WPF.ViewModels.DirectorViewModels
{
    public class ReportsViewModel
    {
        private Director _director;
        public ReportsViewModel(Director director) {
            _director = director;   
        }

        public void SentAveragePoints()
        {
            var senderService = new SenderService();
            senderService.SendAveragePoints(_director);
            ShowSuccess();
        }

        public void SentAveragePenaltyPoints()
        {
            var senderService = new SenderService();
            senderService.SendAveragePenaltyPoints(_director);
            ShowSuccess();
        }
        public void SendAverageGradeByPenaltyCount()
        {
            var senderService = new SenderService();
            senderService.SendAverageGradeByPenaltyCount(_director);
            ShowSuccess();
        }
        public void SendPenaltiesCountLastYear()
        {
            var senderService = new SenderService();
            senderService.SendPenaltiesCountLastYear(_director);
            ShowSuccess();
        }

        private void ShowSuccess()
        {
            MessageBox.Show("Successfully completed!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
