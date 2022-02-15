using Newtonsoft.Json;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Atils.Runtime.DataManagement
{
	public static class JsonDataWriter
	{
        private const string FILE_EXTENSION = ".json";



        public static void SaveData(object value, JsonWriter writer)
        {
            var obj = value as IEnumerable;

            var tuples = obj.Cast<object>().Select(UnpackUnknownObject);

            object[] dataArray = (from dp in tuples select new[] { dp, }).ToArray();

            var serializer = new JsonSerializer();
            serializer.Serialize(writer, dataArray);
        }

        public static object UnpackUnknownObject(object @object)
        {
            var itemProperty = @object.GetType();

            return itemProperty;
        }




        public static string GetSerializedString(object data)
		{
            return JsonUtility.ToJson(data, true);
        }





        public static void SaveData<T>(T data, string folderPath, string fileName)
        {
            SaveData(data, Path.Combine(folderPath, fileName));
        }

        public static void SaveData<T>(T data, string path)
        {
            if (!path.EndsWith(FILE_EXTENSION))
			{
                path += FILE_EXTENSION;
            }

            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, jsonData);
        }
    }
}
