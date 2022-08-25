using Atils.Runtime.Pooling;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Zenject;

public class PoolingTest : MonoBehaviour
{
	[SerializeField] private SphereView sphereView;

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

	Stopwatch stopwatch;

	private IEnumerator SpawnBalls()
	{
		yield return new WaitForSeconds(5);

		for (int i = 0; i < 10000; i++)
		{
			for (int j = 0; j < 30; j++)
			{
				//_objectsPoolView.GetObject<SphereView>();
				SphereView sphereView1 = _objectsPoolView.GetObject<SphereView>();
				SphereView sphereView2 = _objectsPoolView.GetObject<SphereView>().SetPosition(new Vector3());
				PoolObject poolObject = _objectsPoolView.GetRandomObject<PoolObject>().SetPosition(new Vector3());

				//float random = 10;

				//float x = Random.Range(-random, random);
				//float y = Random.Range(-random, random);
				//float z = Random.Range(0, random);

				//Vector3 randomScatter = new Vector3(x, y, z);
				//Vector3 direction = (Vector3.forward + randomScatter).normalized;

				//sphereView.Rigidbody.velocity = direction * 10;
			}
			yield return new WaitForFixedUpdate();
		}
	}

	[ContextMenu("StartTest")]
	private async void StartTest()
	{
		StartCoroutine(SpawnBalls());

		return;

		IPoolObject poolObject1 = _objectsPoolView.GetObject<CubeView>()
			.SetPosition(new Vector3(1, 1, -1))
			.SetLocalScale(new Vector3(1, 0.5f, 1))
			.SetParent(null);

		_objectsPoolView.GetObject<CubeView>()
			.SetPosition(new Vector3(1, 1, 1))
			.SetParent(null);

		IPoolObject poolObject2 = _objectsPoolView.GetObject<CylinderView>();


		return;

		//---------------------------------------------------------------------------------

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < 10000; i++)
		{
			_objectsPoolView.GetObject<SphereView>();
		}
		stopwatch.Stop();
		UnityEngine.Debug.Log("Pooling: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------
		await System.Threading.Tasks.Task.Delay(5000);

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_objectsPoolView.ReturnToPoolAllObjectsOfType<SphereView>();
		stopwatch.Stop();
		UnityEngine.Debug.Log("ReturnToPoolObjectsOfType: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------
		await System.Threading.Tasks.Task.Delay(5000);

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < 10000; i++)
		{
			_objectsPoolView.GetObject<SphereView>();
		}
		stopwatch.Stop();
		UnityEngine.Debug.Log("Pooling: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------
		await System.Threading.Tasks.Task.Delay(5000);

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_objectsPoolView.ReturnToPoolAllObjectsOfType<SphereView>();
		stopwatch.Stop();
		UnityEngine.Debug.Log("ReturnToPoolObjectsOfType: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------
		await System.Threading.Tasks.Task.Delay(5000);

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < 10000; i++)
		{
			_objectsPoolView.GetObject<SphereView>();
		}
		stopwatch.Stop();
		UnityEngine.Debug.Log("Pooling: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------
		await System.Threading.Tasks.Task.Delay(5000);

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_objectsPoolView.ReturnToPoolAllObjectsOfType<SphereView>();
		stopwatch.Stop();
		UnityEngine.Debug.Log("ReturnToPoolObjectsOfType: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------



















		return;
		//---------------------------------------------------------------------------------

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_objectsPoolView.GetAllObjects();
		stopwatch.Stop();
		UnityEngine.Debug.Log("GetActiveObjects: " + stopwatch.Elapsed.TotalSeconds);



		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_objectsPoolView.GetActiveObjectsOfType<SphereView>();
		stopwatch.Stop();
		UnityEngine.Debug.Log("GetActiveObjectsOfType: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_objectsPoolView.Pause();
		stopwatch.Stop();
		UnityEngine.Debug.Log("Pause: " + stopwatch.Elapsed.TotalSeconds);



		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_objectsPoolView.Unpause();
		stopwatch.Stop();
		UnityEngine.Debug.Log("Unpause: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------
		_objectsPoolView.ReturnToPoolAllObjects();
		//---------------------------------------------------------------------------------


		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_objectsPoolView.ReturnToPoolAllObjects();
		stopwatch.Stop();
		UnityEngine.Debug.Log("ReturnToPoolObjects: " + stopwatch.Elapsed.TotalSeconds);

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_objectsPoolView.ReturnToPoolAllObjectsOfType<SphereView>();
		stopwatch.Stop();
		UnityEngine.Debug.Log("ReturnToPoolObjectsOfType: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------
	}
}
