using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace Windwaker_coop
{
    class Program
    {
        public static bool programSyncing = false;

        internal static User currUser;

        internal static IGame[] games;
        public static IGame currGame;
        public static Config config;
        public static SyncSettings syncSettings;

        /// <summary>
        /// Loads config and games. Called once at app startup.
        /// </summary>
        public static bool Initialize()
        {
            config = readConfigFile();
            if (!config.isValidConfig())
            {
                Output.error("Invalid configuration file - Fix the errors or delete the config.json file");
                return false;
            }

            games = loadGames();
            return true;
        }

        /// <summary>
        /// Sets the current game by id.
        /// </summary>
        public static bool SelectGame(int gameId)
        {
            if (games == null || gameId < 0 || gameId >= games.Length)
            {
                Output.error("Invalid game id");
                return false;
            }
            currGame = games[gameId];
            config.gameId = gameId;
            return true;
        }

        /// <summary>
        /// Creates a Server instance at the given IP.
        /// </summary>
        public static void StartAsServer(string ip)
        {
            currUser = new Server(ip);
        }

        /// <summary>
        /// Creates a Client instance connecting to the given IP.
        /// </summary>
        public static void StartAsClient(string ip, string playerName)
        {
            currUser = new Client(ip, playerName);
        }

        /// <summary>
        /// Processes a command string and returns the response.
        /// </summary>
        public static string ProcessCommand(string input)
        {
            if (currUser == null)
                return "Not connected.";

            string command = input.Trim();
            string[] words = command.Split(' ');
            if (words.Length < 1 || string.IsNullOrEmpty(words[0]))
                return "Enter a command.";

            string[] args = new string[words.Length - 1];
            string debugOutput = $"Processing command: '{words[0]}'";
            for (int i = 1; i < words.Length; i++)
            {
                args[i - 1] = words[i];
                debugOutput += " '" + words[i] + "'";
            }
            Output.debug(debugOutput, 1);

            if (words[0] == "stop")
            {
                Shutdown();
                return "Stopping...";
            }

            return currUser.processCommand(words[0], args);
        }

        /// <summary>
        /// Gracefully shuts down syncing and disconnects.
        /// </summary>
        public static void Shutdown()
        {
            programSyncing = false;
            try { currUser?.End(); } catch { }
        }

        /// <summary>
        /// Fatal error handler - called by networking code on unrecoverable errors.
        /// </summary>
        public static void EndProgram()
        {
            Output.text("\nApplication terminated.", ConsoleColor.Gray);
            programSyncing = false;
            try { currUser?.End(); } catch { }
            Environment.Exit(0);
        }

        /// <summary>
        /// Returns list of local IPv4 addresses for display in the UI.
        /// </summary>
        public static List<string> GetLocalIpAddresses()
        {
            List<string> possibleIps = new List<string>();
            try
            {
                string strHostName = Dns.GetHostName();
                Output.debug("Local Machine's Host Name: " + strHostName, 2);
                IPAddress[] addr = Dns.GetHostEntry(strHostName).AddressList;
                foreach (IPAddress ipAd in addr)
                {
                    Output.debug(ipAd.ToString(), 2);
                    if (ipAd.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        possibleIps.Add(ipAd.ToString());
                }
            }
            catch { }
            return possibleIps;
        }

        //~Reads from config.json and returns the config object
        private static Config readConfigFile()
        {
            string path = Environment.CurrentDirectory + "/config.json";
            Config c;

            if (File.Exists(path))
            {
                string configString = File.ReadAllText(path);
                c = JsonConvert.DeserializeObject<Config>(configString);
            }
            else
            {
                c = Config.getDefaultConfig();
                File.WriteAllText(path, JsonConvert.SerializeObject(c, Formatting.Indented));
            }
            return c;
        }

        //Reads the syncSettings from json file
        public static SyncSettings GetSyncSettingsFromFile()
        {
            string path = Environment.CurrentDirectory + $"/syncSettings-{currGame.gameId}.json";
            SyncSettings s;

            if (File.Exists(path))
            {
                string syncString = File.ReadAllText(path);
                s = JsonConvert.DeserializeObject<SyncSettings>(syncString);
            }
            else
            {
                s = currGame.getDefaultSyncSettings();
                File.WriteAllText(path, JsonConvert.SerializeObject(s, Formatting.Indented));
            }
            return s;
        }

        public static string toJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static T fromJson<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        private static IGame[] loadGames()
        {
            IGame[] games = new IGame[]
            {
                new Windwaker(),
                new OcarinaOfTime(),
                new Zelda1(),
                new OracleOfSeasons(),
                new OracleOfAges(),
                new Zelda2()
            };
            Output.debug("Loading " + games.Length + " games", 1);
            return games;
        }
    }
}
