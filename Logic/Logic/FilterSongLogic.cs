using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Logic.Extensions;
using Logic.Interfaces;
using Microsoft.Extensions.Logging;
using NAudio.Wave;
using NLayer.NAudioSupport;

namespace Logic.Logic
{
    public class FilterSongLogic : IFilterSongLogic
    {
        private readonly ILogger<FilterSongLogic> _logger;

        public FilterSongLogic(ILogger<FilterSongLogic> logger)
        {
            _logger = logger;
        }
        
        public bool ShouldInclude(MemoryStream stream, string track, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }

            var flag = filter.Split(Environment.NewLine)
                .Where(pattern => !string.IsNullOrWhiteSpace(pattern))
                .Select(x => x.Trim())
                .All(pattern => !Regex.Matches(track, pattern, RegexOptions.IgnoreCase).Any());

            try
            {
                var builder = new Mp3FileReader.FrameDecompressorBuilder(wf => new Mp3FrameDecompressor(wf));

                var reader = new Mp3FileReader(stream.Reset(), builder);

                flag &= reader.TotalTime.TotalSeconds >= 30;
            }
            catch(Exception e)
            {
                _logger.LogError($"Stream with name '{track}' is not a valid mp3 file", e);

                flag = false;
            }
            
            return flag;
        }
    }
}