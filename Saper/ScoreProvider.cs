using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace Saper
{
    public static class ScoreProvider
    {
        public static List<ScoreItem> LoadScores(Difficulty difficulty)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(List<ScoreItem>));
                var reader = new StreamReader(PersistentStorageFileName(difficulty));
                return (List<ScoreItem>)xmlSerializer.Deserialize(reader);
            }
            catch (FileNotFoundException)
            {
            }
            catch (DirectoryNotFoundException)
            {
            }
            return new List<ScoreItem>();
        }

        public static void SaveScores(List<ScoreItem> scores, Difficulty difficulty)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<ScoreItem>));
            var filename = PersistentStorageFileName(difficulty);

            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            var writer = new StreamWriter(filename);
            xmlSerializer.Serialize(writer, scores);
        }

        private static string PersistentStorageFileName(Difficulty difficulty)
        {
            var appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appDataDir, string.Format("NarangiSoft\\Saper\\Scores-{0}.xml", difficulty));
        }
    }
}
