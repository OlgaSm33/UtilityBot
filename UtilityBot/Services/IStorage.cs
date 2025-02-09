using UtilityBot.Models;

namespace UtilityBot.Services
{
    public interface IStorage
    {
        void Save(string message, long chatID, string sessionType = default);
        IEnumerable<string> Load();
        string GetLastSessionType(long chatID);


    }
}
