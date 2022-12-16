using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CRV.AX.POS365Integration
{
    public static class AxCSVHelper
    {
        public static List<T> Convert<T>(string filePath)
        {
            List<T> result = new List<T>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                result = csv.GetRecords<T>().ToList();
            }

            string fileName = $"{new FileInfo(filePath).Name}";
            result.ForEach(x => x.GetType().GetProperty("FileName")?.SetValue(x, fileName));

            return result;
        }

        public static void SaveToFile<T>(T input, string _filePath)
        {
            bool isExisted = File.Exists(_filePath);

            if (isExisted)
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                };
                using (var stream = File.Open(_filePath, FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecord<T>(input);
                }
            }
            else
            {
                using (var writer = new StreamWriter(_filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecord<T>(input);
                }
            }
        }

        public static void SaveToFile<T>(List<T> inputs, string _filePath)
        {
            bool isExisted = File.Exists(_filePath);

            if (isExisted)
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                };
                using (var stream = File.Open(_filePath, FileMode.Append))
                using (var writer = new StreamWriter(stream))
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords<T>(inputs);
                }
            }
            else
            {
                using (var writer = new StreamWriter(_filePath, true))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords<T>(inputs);
                }
            }
        }
    }
}
