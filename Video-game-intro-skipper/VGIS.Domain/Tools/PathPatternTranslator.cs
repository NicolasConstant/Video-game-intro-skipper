using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VGIS.Domain.Consts;

namespace VGIS.Domain.Tools
{
    public interface IPathPatternTranslator
    {
        IEnumerable<string> GetPathFromPattern(string pattern);
    }

    public class PathPatternTranslator : IPathPatternTranslator
    {
        private readonly IDirectoryBrowser _directoryBrowser;

        #region Ctor
        public PathPatternTranslator(IDirectoryBrowser directoryBrowser)
        {
            _directoryBrowser = directoryBrowser;
        }
        #endregion

        public IEnumerable<string> GetPathFromPattern(string pattern)
        {
            //If no navigation needed, return the pattern
            if (!pattern.Contains(SpecialChar.AnyDirectoryName.ToString()))
                return new[] { pattern };
            
            //Analyse the pattern and navigate when needed
            var pathSections = pattern.Split('\\');
            var allPaths = new List<string>();
            for (var i = 1; i < pathSections.Length; i++)
            {
                var subPath = pathSections[i];
                //Init
                if (i == 1)
                {
                    allPaths.Add($@"{pathSections[0]}\{pathSections[1]}");
                }
                //Is a subfolder
                else if (!subPath.Contains(SpecialChar.AnyDirectoryName.ToString()))
                {
                    for (var j = 0; j < allPaths.Count; j++)
                    {
                        var completePath = allPaths[j];
                        var completeSubPath = $"{completePath}\\{subPath}";
                        allPaths[j] = completeSubPath;
                    }
                }
                //Is a navigation order
                else if (subPath.Trim() == SpecialChar.AnyDirectoryName.ToString().Trim())
                {
                    var allPathResults = new List<string>();
                    for (var j = 0; j < allPaths.Count; j++)
                    {
                        var completePath = allPaths[j];
                        var allSubPaths = _directoryBrowser.GetSubDirectories(completePath);
                        foreach (var allSubPath in allSubPaths)
                            allPathResults.Add(allSubPath);
                    }
                    allPaths = allPathResults;
                }
                //Is unsupported
                else
                {
                    throw new ArgumentException($"{pattern} is using a unsupported pattern");
                }
            }

            return allPaths;
        }
    }
}