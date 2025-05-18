using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhoneFileTransfer.Utilities.Path
{
    public class PathUtils : IPathUtils
    {
        public string CombineSafe(params string[] segments)
        {
            if (segments == null || segments.Length == 0)
                return string.Empty;

            char preferredSeparator = DetectPreferredSeparator(segments);

            List<string> cleaned = new List<string>();

            foreach (var segment in segments)
            {
                if (string.IsNullOrWhiteSpace(segment))
                    continue;

                var s = segment.Trim();

                if (cleaned.Count == 0)
                    cleaned.Add(s.TrimEnd('/', '\\'));
                else
                    cleaned.Add(s.TrimStart('/', '\\'));
            }

            // Unione con separator coerente
            var combined = string.Join(preferredSeparator, cleaned);
            return combined;
        }

        private char DetectPreferredSeparator(string[] segments)
        {
            foreach (var s in segments)
            {
                if (string.IsNullOrWhiteSpace(s))
                    continue;

                if (s.Contains('/')) return '/';
                if (s.Contains('\\')) return '\\';
            }
            // Default al separatore del sistema operativo
            return System.IO.Path.DirectorySeparatorChar;
        }
    }
}
