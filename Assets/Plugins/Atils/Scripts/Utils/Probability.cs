using UnityEngine;

namespace Atils.Runtime.Utils
{
	public static class Probability
	{
		public static bool Check(int persentage)
		{
			persentage = Mathf.Clamp(persentage, 0, 100);
			int randomNumber = Random.Range(0, 100);
			return randomNumber < persentage;
		}
	}
}
