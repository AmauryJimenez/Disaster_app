using Contracts.Services;
using Models.Disasters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Commons.Services
{
    public class FileSystemServicePersister : IDisasterInfoPersister
    {
        public void SaveDisasters(IList<Disaster> disasters)
        {
            List<Disaster> floodDisasters = new List<Disaster>();
            List<Disaster> earthquakeDisasters = new List<Disaster>();
            List<Disaster> epidemicDisasters = new List<Disaster>();

            foreach (var disaster in disasters)
            {
                switch (disaster.DisasterType)
                {
                    case DisasterType.Flood:
                        floodDisasters.Add(disaster);
                        break;
                    case DisasterType.Earthquake:
                        earthquakeDisasters.Add(disaster);
                        break;
                    case DisasterType.Epidemic:
                        epidemicDisasters.Add(disaster);
                        break;
                }
            }

            CreateIfNotExist("Saves");
            CreateIfNotExist(Path.Combine("Saves", "Flood"));
            CreateIfNotExist(Path.Combine("Saves", "Earthquake"));
            CreateIfNotExist(Path.Combine("Saves", "Epidemic"));

            SaveDisasterList(Path.Combine("Saves", "Flood", Constants.FloodsFileName), floodDisasters);
            SaveDisasterList(Path.Combine("Saves", "Earthquake", Constants.EarthquakeFileName), earthquakeDisasters);
            SaveDisasterList(Path.Combine("Saves", "Epidemic", Constants.EpidemicFileName), epidemicDisasters);
        }

        private static void CreateIfNotExist(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private static void SaveDisasterList(string filePath, List<Disaster> disastersList)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            Stream saveFileStream = File.Create(filePath);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(saveFileStream, disastersList);
            saveFileStream.Close();
        }
    }
}
