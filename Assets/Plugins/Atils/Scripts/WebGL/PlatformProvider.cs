#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

namespace Atils.Runtime.WebGL
{
	public static class PlatformProvider
	{
#if !UNITY_EDITOR && UNITY_WEBGL
		[DllImport("__Internal")]
		private static extern bool isMobile();

		[DllImport("__Internal")]
		private static extern bool isAndroid();

		[DllImport("__Internal")]
		private static extern bool isIOS();

		[DllImport("__Internal")]
		private static extern bool isIPhone();

		[DllImport("__Internal")]
		private static extern bool isIPad();

		[DllImport("__Internal")]
		private static extern bool isIPod();
#endif

		public static bool IsMobile()
		{
#if !UNITY_EDITOR && UNITY_WEBGL
			return isMobile();
#else
			return false;
#endif
		}

		public static bool IsAndroid()
		{
#if !UNITY_EDITOR && UNITY_WEBGL
			return isMobile();
#else
			return false;
#endif
		}

		public static bool IsIOS()
		{
#if !UNITY_EDITOR && UNITY_WEBGL
			return isIOS();
#else
			return false;
#endif
		}

		public static bool IsIPhone()
		{
#if !UNITY_EDITOR && UNITY_WEBGL
			return isIPhone();
#else
			return false;
#endif
		}

		public static bool IsIPad()
		{
#if !UNITY_EDITOR && UNITY_WEBGL
			return isIPhone();
#else
			return false;
#endif
		}

		public static bool IsIPod()
		{
#if !UNITY_EDITOR && UNITY_WEBGL
			return isIPhone();
#else
			return false;
#endif
		}
	}
}
