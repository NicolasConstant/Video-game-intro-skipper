using System;
using System.Threading;
using VGIS.Domain.Domain;

namespace VGIS.Domain.BusinessRules
{
    public class ValidateGameRoot
    {
        private string _potentialGameEmplacementPath;
        private RootValidationRule _rootValidationRule;

        #region Ctor
        public ValidateGameRoot(string potentialGameEmplacementPath, RootValidationRule rootValidationRule)
        {
            _potentialGameEmplacementPath = potentialGameEmplacementPath;
            _rootValidationRule = rootValidationRule;
        }
        #endregion

        public bool Run()
        {
            throw new NotImplementedException();
        }
    }
}