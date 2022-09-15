using System.IO;
using UnityEngine;

namespace Atils.Runtime.DataManagement
{
	public static class JsonDataReader
	{
        public static T LoadData<T>(string folderPath, string fileName)
        {
            return LoadData<T>(Path.Combine(folderPath, fileName));
        }

        public static T LoadData<T>(string path)
        {
            if (!IsFileExists(path))
            {
                Debug.LogError("Path does not exists: " + path);
                return default;
            }

            string jsonData = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(jsonData);
        }

        public static bool IsFileExists(string folderPath, string fileName)
        {
            return IsFileExists(Path.Combine(folderPath, fileName));
        }

        public static bool IsFileExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
