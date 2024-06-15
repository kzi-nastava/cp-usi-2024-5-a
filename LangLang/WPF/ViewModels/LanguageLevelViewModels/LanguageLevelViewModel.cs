
using LangLang.Domain.Enums;
using LangLang.Domain.Models;
using System.ComponentModel;

namespace LangLang.WPF.ViewModels.LanguageLevelViewModels
{
    public class LanguageLevelViewModel : INotifyPropertyChanged
    {
        public int Id {  get; set; }
        public string Language {  get; set; }
        public Level Level {  get; set; }

        public LanguageLevelViewModel() { }

        public LanguageLevelViewModel(LanguageLevel languageLevel)
        {
            Id = languageLevel.Id;
            Language = languageLevel.Language;
            Level = languageLevel.Level;
        }

        public LanguageLevel ToLanguageLevel()
        {
            return new LanguageLevel(Id, Language, Level);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
