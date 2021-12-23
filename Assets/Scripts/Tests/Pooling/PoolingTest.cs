using Atils.Runtime.Pooling;
using UnityEngine;
using Zenject;

public class PoolingTest : MonoBehaviour
{
	ObjectsPoolView _objectsPoolView = default;

	[Inject]
	private void Construct(ObjectsPoolView sampleSceneObjectsPoolView)
	{
		_objectsPoolView = sampleSceneObjectsPoolView;
	}

	private void Start()
	{
		StartTest();
	}

	[ContextMenu("StartTest")]
	private void StartTest()
	{
		IPoolObject poolObject1 = _objectsPoolView.GetObjectProvider<CubeView>()
			.SetPosition(new Vector3(1, 1, -1))
			.SetLocalScale(new Vector3(1, 0.5f, 1))
			.SetParent(null)
			.GetObject();

		_objectsPoolView.GetObjectProvider<CubeView>()
			.SetPosition(new Vector3(1, 1, 1))
			.SetParent(null)
			.GetObject();

		IPoolObject poolObject2 = _objectsPoolView.GetObject<CylinderView>();
		IPoolObject poolObject3 = _objectsPoolView.GetObject<SphereView>();
	}
}
