namespace LangLang.Domain.RepositoryInterfaces
{
    public interface IEmailRepository
    {
        public string GetContent(string path);
        public void Load();
    }
}
