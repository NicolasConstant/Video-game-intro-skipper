using System.Collections.Generic;
using VGIS.Domain.BusinessRules;
using VGIS.Domain.Domain;
using VGIS.Domain.Repositories;
using VGIS.Domain.Tools;

namespace VGIS.Domain.Services
{
    public class IntroEditionService
    {
        private readonly FileAndFolderRenamer _fileAndFolderRenamer;
        private readonly PathPatternTranslator _pathPatternTranslator;

        #region Ctor
        public IntroEditionService(FileAndFolderRenamer fileAndFolderRenamer, PathPatternTranslator pathPatternTranslator)
        {
            _fileAndFolderRenamer = fileAndFolderRenamer;
            _pathPatternTranslator = pathPatternTranslator;
        }
        #endregion

        public void DisableIntro(Game targetedGame)
        {
            var disableIntro = new ApplyDisableIntroAction(targetedGame.Settings, targetedGame.DetectionResult, _fileAndFolderRenamer, _pathPatternTranslator);
            disableIntro.Execute();
        }

        public void ReenableIntro(Game targetedGame)
        {
            var reenableIntro = new ApplyReenableIntroAction(targetedGame.Settings, targetedGame.DetectionResult, _fileAndFolderRenamer, _pathPatternTranslator);
            reenableIntro.Execute();
        }
    }
}