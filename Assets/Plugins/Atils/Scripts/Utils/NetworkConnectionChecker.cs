using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Atils.Runtime.Utils
{
	public static class NetworkConnectionChecker
	{
		private static string _checkHost = "https://google.com";

		public static async Task<bool> CheckConnection(CancellationToken cancellationToken, int timeOut = 5)
		{
			UnityWebRequest request = UnityWebRequest.Get(_checkHost);
			request.timeout = timeOut;
			request.SendWebRequest();

			while (!request.isDone)
			{
				await Task.Yield();
				if (cancellationToken.IsCancellationRequested)
				{
					return false;
				}
			}

			return request.result == UnityWebRequest.Result.Success;
		}
	}
}
