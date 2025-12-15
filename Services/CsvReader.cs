using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CatiaHoleAutomation.Models;

namespace CatiaHoleAutomation.Services
{
    public class CsvReader
    {
        public static List<HoleData> ReadHoleData(string csvPath)
        {
            var holes = new List<HoleData>();

            if (string.IsNullOrWhiteSpace(csvPath) || !File.Exists(csvPath))
            {
                throw new FileNotFoundException("CSV file not found.", csvPath);
            }

            var lines = File.ReadAllLines(csvPath);
            var slNo = 1;
            var isFirstDataRow = true;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var parts = line.Split(',');
                if (parts.Length < 5)
                {
                    continue;
                }

                var xText = parts[0].Trim();
                var yText = parts[1].Trim();

                // Skip header row if present
                if (isFirstDataRow && !TryParseDouble(xText, out _))
                {
                    isFirstDataRow = false;
                    continue;
                }
                isFirstDataRow = false;

                if (!TryParseDouble(xText, out var x))
                {
                    throw new FormatException($"Invalid X coordinate at SlNo {slNo}.");
                }

                if (!TryParseDouble(yText, out var y))
                {
                    throw new FormatException($"Invalid Y coordinate at SlNo {slNo}.");
                }

                var radiusText = parts[2].Trim();
                if (!TryParseDouble(radiusText, out var radius) || radius <= 0)
                {
                    throw new FormatException($"Invalid radius at SlNo {slNo}.");
                }

                var depthText = parts[3].Trim();
                if (!TryParseDouble(depthText, out var depth) || depth <= 0)
                {
                    throw new FormatException($"Invalid depth at SlNo {slNo}.");
                }

                var holeType = parts[4].Trim();

                holes.Add(new HoleData
                {
                    SlNo = slNo,
                    X = x,
                    Y = y,
                    Radius = radius,
                    Depth = depth,
                    HoleType = holeType
                });

                slNo++;
            }

            return holes;
        }

        private static bool TryParseDouble(string text, out double result)
        {
            return double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out result)
                   || double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out result);
        }
    }
}
