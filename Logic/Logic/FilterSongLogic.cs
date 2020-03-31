using System;
using System.Linq;
using System.Text.RegularExpressions;
using Logic.Interfaces;

namespace Logic.Logic
{
    public class FilterSongLogic : IFilterSongLogic
    {
        public bool ShouldInclude(string filename, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }

            var flag = filter.Split(Environment.NewLine)
                .Where(pattern => !string.IsNullOrWhiteSpace(pattern))
                .All(pattern => !Regex.Matches(filename, pattern, RegexOptions.IgnoreCase).Any());

            return flag;
        }
    }
}