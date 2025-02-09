
using Telegram.Bot.Types;
using UtilityBot.Configuration;


namespace UtilityBot.Services
{

    public class FileMemoryStorage : IStorage
    {
        private AppSettings _appSettings;

        private string _logFilePath;


        public FileMemoryStorage(AppSettings appSettings)
        {
            _appSettings = appSettings;
            _logFilePath = Path.Combine(_appSettings.logFilePath, $"{_appSettings.logFileName}.{_appSettings.logFileFormat}");

        }

        public void Save(string message, long chatID, string sessionType = default)
        {
            try
            {
                string logEntry = $"{DateTime.Now}, {message}, ChatID: {chatID}, type: {sessionType}";
                using (StreamWriter writer = new StreamWriter(_logFilePath, append: true))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка записи в лог: {ex.Message}");
            }
        }

        public IEnumerable<string> Load()
        {
            try
            {
                if (File.Exists(_logFilePath))
                {
                    return File.ReadAllLines(_logFilePath);
                }
                return Enumerable.Empty<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения из лога: {ex.Message}");
                return Enumerable.Empty<string>();
            }
        }

        public string GetLastSessionType(long chatID)
        {
            try
            {


                // Ищем строку с данным chatId
                foreach (var line in Load().Reverse())  // Начинаем с конца для поиска последней записи
                {
                    if (line.Contains($"ChatID: {chatID}"))
                    {
                        return line.Split(' ')[^1];
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения из лога: {ex.Message}");
            }

            return null;  // Если не найдена запись для этого пользователя
        }

    }

}