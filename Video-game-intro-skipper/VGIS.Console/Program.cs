using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.DataAccessLayers;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;
using VGIS.Domain.Repositories;
using VGIS.Domain.Services;
using VGIS.Domain.Tools;

namespace VGIS.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // Init
            var gameSettingsRepo = new GameSettingsRepository($@"{Directory.GetCurrentDirectory()}\GameSettings\", new FileSystemDal());
            var installationDirRepo = new InstallationDirectoriesRepository($@"{Directory.GetCurrentDirectory()}\DefaultInstallFolders.json", new FileSystemDal());
            var fileAndFolderRenamer = new FileAndFolderRenamer();
            var directoryBrowser = new DirectoryBrowser();
            var pathPatternTranslator = new PathPatternTranslator(directoryBrowser);


            var introEditionService = new IntroEditionService(gameSettingsRepo, installationDirRepo, fileAndFolderRenamer, pathPatternTranslator);
            Func<IEnumerable<Game>> introEditionServiceFunc = () => introEditionService.GetAllGames();

            // Load all games
            var allGames = LoadAllGames(introEditionServiceFunc);

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
                    var introState = gameToModify.DetectionResult.IntroductionState;
                    if (introState == IntroductionStateEnum.Disabled)
                    {
                        introEditionService.ReenableIntro(gameToModify);
                        allGames = LoadAllGames(introEditionServiceFunc);
                    }
                    else if (introState == IntroductionStateEnum.Enabled)
                    {
                        introEditionService.DisableIntro(gameToModify);
                        allGames = LoadAllGames(introEditionServiceFunc);
                    }
                    else if(introState == IntroductionStateEnum.Unknown)
                    {
                        introEditionService.DisableIntro(gameToModify);
                        allGames = LoadAllGames(introEditionServiceFunc);
                    }
                }
                else if (inputValue?.ToLower() == "q" || inputValue?.ToLower() == "quit")
                {
                    return;
                }
            }
        }

        private static List<Game> LoadAllGames(Func<IEnumerable<Game>> detectAllGamesStatusFunc)
        {
            var allGames = new List<Game>();
            foreach (var gameStatus in detectAllGamesStatusFunc())
            {
                allGames.Add(gameStatus);

                var introMessage = gameStatus.DetectionResult.IntroductionState == IntroductionStateEnum.Enabled ? "Intro Video Enabled" : "Intro Video Disabled";
                System.Console.WriteLine($"{allGames.Count} - {gameStatus.Settings.Name} : {introMessage} ");
            }
            return allGames;
        }
    }
}
