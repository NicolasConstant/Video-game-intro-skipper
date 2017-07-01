using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;
using VGIS.Domain.Repositories;
using VGIS.Domain.Tools;

namespace VGIS.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // Init
            var gameSettingsRepo = new GameSettingsRepository();
            var installationDirRepo = new InstallationDirectoriesRepository();
            var fileAndFolderRenamer = new FileAndFolderRenamer();
            var directoryBrowser = new DirectoryBrowser();

            var detectAllGamesStatus = new DetectAllGamesStatus(gameSettingsRepo, installationDirRepo);

            // Load all games
            var allGames = LoadAllGames(detectAllGamesStatus);

            for (;;)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("Please select the game for enabling/disabling the intro video");

                var inputValue = System.Console.ReadLine();
                int indexValue;

                if (int.TryParse(inputValue, out indexValue))
                {
                    if(indexValue > allGames.Count || indexValue < 1) continue;

                    var gameToModify = allGames[indexValue - 1];
                    var introState = gameToModify.Item2.IntroductionState;
                    if (introState == IntroductionStateEnum.Disabled)
                    {
                        var reenableIntro = new ApplyReenableIntroAction(gameToModify.Item1, gameToModify.Item2);
                        reenableIntro.Execute();
                        allGames = LoadAllGames(detectAllGamesStatus);
                    }
                    else if (introState == IntroductionStateEnum.Enabled)
                    {
                        var reenableIntro = new ApplyDisableIntroAction(gameToModify.Item1, gameToModify.Item2, fileAndFolderRenamer, directoryBrowser);
                        reenableIntro.Execute();
                        allGames = LoadAllGames(detectAllGamesStatus);
                    }
                    else if(introState == IntroductionStateEnum.Unknown)
                    {
                        var reenableIntro = new ApplyDisableIntroAction(gameToModify.Item1, gameToModify.Item2, fileAndFolderRenamer, directoryBrowser);
                        reenableIntro.Execute();
                        allGames = LoadAllGames(detectAllGamesStatus);
                    }
                }
                else if (inputValue?.ToLower() == "q" || inputValue?.ToLower() == "quit")
                {
                    return;
                }
            }
        }

        private static List<Tuple<GameSetting, GameDetectionResult>> LoadAllGames(DetectAllGamesStatus detectAllGamesStatus)
        {
            var allGames = new List<Tuple<GameSetting, GameDetectionResult>>();
            foreach (var gameStatus in detectAllGamesStatus.Execute())
            {
                allGames.Add(gameStatus);

                var introMessage = gameStatus.Item2.IntroductionState == IntroductionStateEnum.Enabled ? "Intro Video Enabled" : "Intro Video Disabled";
                System.Console.WriteLine($"{allGames.Count} - {gameStatus.Item1.Name} : {introMessage} ");
            }
            return allGames;
        }
    }
}
