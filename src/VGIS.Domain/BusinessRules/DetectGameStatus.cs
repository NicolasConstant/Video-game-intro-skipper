using System;
using System.Collections.Generic;
using System.IO;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;

namespace VGIS.Domain.BusinessRules
{
    internal class DetectGameStatus
    {
        private readonly GameSetting _gameSetting;
        private readonly IEnumerable<DirectoryInfo> _installationDirectories;

        #region Ctor
        public DetectGameStatus(GameSetting gameSetting, IEnumerable<DirectoryInfo> installationDirectories)
        {
            _gameSetting = gameSetting;
            _installationDirectories = installationDirectories;
        }
        #endregion

        public GameDetectionResult Execute()
        {
            var detectionResult = new GameDetectionResult(_gameSetting.Name);
            string completeInstallationDirectory = null;
            var isDetected = false;

            foreach (var installationDirectory in _installationDirectories)
            {
                foreach (var rootDirectory in _gameSetting.PotentialRootFolderNames)
                {
                    // Validate if game emplacement exists
                    var potentialGameEmplacementPath = $"{installationDirectory}\\{rootDirectory}\\";
                    if (!Directory.Exists(potentialGameEmplacementPath)) continue;

                    // Validate that the folder is the targeted one 
                    var isValidated = true;
                    foreach (var rootValidationRule in _gameSetting.ValidationRules)
                    {
                        var rootValidation = new ValidateGameRoot(potentialGameEmplacementPath, rootValidationRule);
                        if (!rootValidation.Execute())
                            isValidated = false;
                    }

                    // If validated store value and break
                    if (isValidated)
                    {
                        completeInstallationDirectory = potentialGameEmplacementPath;
                        isDetected = true;
                        break;
                    }
                }
            }

            // Assign detection value
            detectionResult.Detected = isDetected;

            // Assign Installation Path if detected
            if (detectionResult.Detected && !string.IsNullOrEmpty(completeInstallationDirectory))
                detectionResult.InstallationPath = new DirectoryInfo(completeInstallationDirectory);

            // Determine current introduction state 
            if (detectionResult.Detected)
            {
                var introStateDetection = new DetectIntroductionState(detectionResult.InstallationPath,
                    _gameSetting.DisablingIntroductionActions);
                detectionResult.IntroductionState = introStateDetection.Execute();
            }

            // Return result
            return detectionResult;
        }
    }
}