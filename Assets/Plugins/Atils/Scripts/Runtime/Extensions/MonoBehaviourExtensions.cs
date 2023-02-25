using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Atils.Runtime.Extensions
{
	public static class MonoBehaviourExtensions
	{
		public static void KillCoroutine(this MonoBehaviour monoBehaviour, ref Coroutine coroutine)
		{
			if (coroutine != null)
			{
				monoBehaviour.StopCoroutine(coroutine);
				coroutine = null;
			}
		}

#if UNITY_EDITOR

		public static void MarkSceneDirty(this MonoBehaviour monoBehaviour)
		{
			Scene scene = SceneManager.GetActiveScene();
			EditorSceneManager.MarkSceneDirty(scene);
		}

		public static void MarkPrefabDirty(this MonoBehaviour monoBehaviour)
		{
			PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
			if (prefabStage != null)
			{
				EditorSceneManager.MarkSceneDirty(prefabStage.scene);
			}
		}

#endif
	}
}
