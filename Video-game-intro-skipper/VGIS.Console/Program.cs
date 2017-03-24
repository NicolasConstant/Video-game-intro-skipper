using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Domain;
using VGIS.Domain.Repositories;

namespace VGIS.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // Init
            var gameSettingsRepo = new GameSettingsRepository();
            var installationDirRepo = new InstallationDirectoriesRepository();

            var detectAllGamesStatus = new DetectAllGamesStatus(gameSettingsRepo, installationDirRepo);

            // Load all games
            var allGames = new List<Tuple<GameSetting, GameDetectionResult>>();
            foreach (var gameStatus in detectAllGamesStatus.Run())
            {
                allGames.Add(gameStatus);

                var introMessage = gameStatus.Item2.Detected ?  "Intro Video Enabled" : "Intro Video Disabled";
                System.Console.WriteLine($"{allGames.Count} - {gameStatus.Item1.Name} : {introMessage} ");
            }

            System.Console.WriteLine();
            System.Console.WriteLine("Please select the game for enabling/disabling the intro video");

            for (;;)
            {
                var inputValue = System.Console.ReadLine();
                int indexValue;

                if (int.TryParse(inputValue, out indexValue))
                {
                    
                }
                else if (inputValue?.ToLower() == "q" || inputValue?.ToLower() == "quit")
                {
                    return;
                }
            }
        }
    }
}
