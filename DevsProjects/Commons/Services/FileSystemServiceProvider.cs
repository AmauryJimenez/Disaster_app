using Contracts.Services;
using Models.Disasters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Commons.Services
{
    public class FileSystemServiceProvider : IDisasterInfoProvider
    {
        public IList<Disaster> RequestDisasters(DisasterType[] requestDisasterTypes)
        {
            return LoadDisasterRecursive("Saves");
        }

        private static List<Disaster> LoadDisasterRecursive(string path)
        {
            List<Disaster> result = new List<Disaster>();

            foreach (string file in Directory.GetFiles(path))
            {
                if(file.Contains("Flood") || file.Contains("Earth") || file.Contains("Epidemic"))
                    result.AddRange((List<Disaster>)LoadDisasterList(file));
            }

            foreach(string directory in Directory.GetDirectories(path))
                result.AddRange(LoadDisasterRecursive(directory));

            return result;
        }

        private static IList LoadDisasterList(string filePath)
        {
            IList result;

            if (!File.Exists(filePath))
                throw new InvalidOperationException($"Cannot find the file {filePath}");

            Stream openFileStream = File.OpenRead(filePath);
            BinaryFormatter deserializer = new BinaryFormatter();
            result = (List<Disaster>)deserializer.Deserialize(openFileStream);
            openFileStream.Close();

            return result;
        }

        public void Dispose()
        {
        }
    }
}
