using Atils.Runtime.Attributes;
using Atils.Runtime.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ListExtensionsTest : MonoBehaviour
{
	public List<int> list;

	private void Start()
	{
		Test();
	}

	[ContextMenu("Test")]
	private async void Test()
	{
		await Task.Delay(1);

		//list = new List<int>();

		//list.Remove(1);

		//list.AddIfNotExist(2);
		//list.AddIfNotExist(2);
		//list.AddIfNotExist(3);
		//list.AddIfNotExist(3);

		//return;

		Debug.Log("----- GetRandom() -----");
		for (int i = 0; i < 5; i++)
		{
			int number = list.GetRandom();
			Debug.Log(number);
			await Task.Delay(10);
		}

		Debug.Log("----- GetRandom(out int index) -----");
		for (int i = 0; i < 5; i++)
		{
			int number = list.GetRandom(out int index);
			Debug.Log(number + " " + index);
			await Task.Delay(10);
		}

		Debug.Log("----- GetRandom(x => x == 3) -----");
		for (int i = 0; i < 5; i++)
		{
			int number = list.GetRandom(x => x == 3);
			Debug.Log(number);
			await Task.Delay(10);
		}

		Debug.Log("----- GetRandom(x => x == 3, out int index) -----");
		for (int i = 0; i < 5; i++)
		{
			int number = list.GetRandom(x => x == 3, out int index);
			Debug.Log(number + " " + index);
			await Task.Delay(10);
		}

		Debug.Log("----- FindAllWithIndexes(x => x == 3) -----");
		List<Tuple<int, int>> listNumbers = list.FindAllWithIndexes(x => x == 3);
		for (int i = 0; i < listNumbers.Count; i++)
		{
			Debug.Log(listNumbers[i].Item1 + " " + listNumbers[i].Item2);
			await Task.Delay(10);
		}
	}
}
