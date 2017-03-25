using System;
using System.IO;
using System.Threading;
using VGIS.Domain.Domain;
using VGIS.Domain.Enums;

namespace VGIS.Domain.BusinessRules
{
    public class ValidateGameRoot
    {
        private readonly string _potentialGameEmplacementPath;
        private readonly RootValidationRule _rootValidationRule;

        #region Ctor
        public ValidateGameRoot(string potentialGameEmplacementPath, RootValidationRule rootValidationRule)
        {
            _potentialGameEmplacementPath = potentialGameEmplacementPath;
            _rootValidationRule = rootValidationRule;
        }
        #endregion

        public bool Execute()
        {
            var type = _rootValidationRule.Type;
            var witnessName = _rootValidationRule.WitnessName;

            switch (type)
            {
                case RootValidationTypeEnum.FileValidation:
                    return ValidateFile(witnessName);
                case RootValidationTypeEnum.FolderValidation:
                    return ValidateFolder(witnessName);
                default:
                    throw new Exception($"RootValidationTypeEnum {type} not found");

            }
        }

        private bool ValidateFile(string witnessName)
        {
            return File.Exists($"{_potentialGameEmplacementPath}\\{witnessName}");
        }

        private bool ValidateFolder(string witnessName)
        {
            return Directory.Exists($"{_potentialGameEmplacementPath}\\{witnessName}");
        }
    }
}