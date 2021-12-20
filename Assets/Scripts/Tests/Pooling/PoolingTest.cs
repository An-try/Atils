using Atils.Runtime.Pooling;
using UnityEngine;
using Zenject;

public class PoolingTest : MonoBehaviour
{
	SampleSceneObjectsPoolView _sampleSceneObjectsPoolView = default;

	[Inject]
	private void Construct(SampleSceneObjectsPoolView sampleSceneObjectsPoolView)
	{
		_sampleSceneObjectsPoolView = sampleSceneObjectsPoolView;
	}

	private void Start()
	{
		StartTest();
	}

	[ContextMenu("StartTest")]
	private void StartTest()
	{
		IPoolObject poolObject1 = _sampleSceneObjectsPoolView.GetObjectProvider<CubeView>().WithPosition(new Vector3(1, 1, -1)).WithLocalScale(new Vector3(1, 0.5f, 1)).GetObject();
		IPoolObject poolObject2 = _sampleSceneObjectsPoolView.GetObject<CylinderView>();
	}
}
