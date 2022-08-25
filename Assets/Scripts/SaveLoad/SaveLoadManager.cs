using Atils.Runtime.DataManagement;
using Atils.Runtime.Generics;
using Atils.Runtime.SaveLoad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
	private void Start()
	{
		Save();
	}

	[ContextMenu("Save")]
	private void Save()
	{
		SaveableLoadableObject[] saveableLoadableObjects = FindObjectsOfType<SaveableLoadableObject>();
		SaveLoadData saveLoadData = new SaveLoadData();
		//saveLoadData.Objects = new object[] { saveableLoadableObjects[0].A };

		List<object> objects = new List<object>();

		for (int i = 0; i < saveableLoadableObjects.Length; i++)
		{
			List<PropertyInfo> properties = saveableLoadableObjects[i].GetType().GetProperties().Where(x => x.IsDefined(typeof(SaveLoadAttribute), false)).ToList();
			
			for (int j = 0; j < properties.Count; j++)
			{
				objects.Add(properties[j].GetValue(saveableLoadableObjects[i]));
			}
		}

		//Debug.Log(objects.Count); // TODO ended here

		saveLoadData.Objects = objects;
		//Debug.Log(saveLoadData.Objects.Length);

		Debug.Log(JsonDataWriter.GetSerializedString(objects)); // TODO ended here as well
		//JsonDataWriter.SaveData(saveLoadData, Application.dataPath, "TestFile");
	}

	[ContextMenu("Load")]
	private void Load()
	{
		JsonDataReader.LoadData<SaveLoadData>(Application.dataPath, "TestFile");
	}
}
