using System.Threading.Tasks;
using UnityEngine;

namespace Atils.Runtime.Extensions
{
	public static class TaskExtensions
	{
		public static void Except(this Task task)
		{
			task.ContinueWith(t =>
			{
				if (t.Exception != null)
				{
					Debug.LogException(t.Exception);
				}
			}, TaskContinuationOptions.NotOnCanceled);
		}
	}
}
