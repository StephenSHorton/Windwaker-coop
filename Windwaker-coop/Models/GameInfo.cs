namespace Windwaker_coop.Models
{
    public class GameInfo
    {
        public int GameId { get; }
        public string GameName { get; }
        public string EmulatorName { get; }

        public GameInfo(int gameId, string gameName, string emulatorName)
        {
            GameId = gameId;
            GameName = gameName;
            EmulatorName = emulatorName;
        }
    }
}
