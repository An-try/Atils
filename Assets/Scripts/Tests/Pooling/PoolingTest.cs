using Atils.Runtime.Pause;
using Atils.Runtime.Pooling;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Zenject;

public class PoolingTest : MonoBehaviour
{
	[SerializeField] private SphereView sphereView;

	private ScenePool _scenePool = default;

	[Inject]
	private void Construct(ScenePool scenePool)
	{
		_scenePool = scenePool;
	}

	private void Start()
	{
		StartTest();
	}

	private void Update()
	{
		UnityEngine.Debug.Log(_scenePool.GetAllObjectsOfType<SphereView>().Count);

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (_scenePool.IsPaused)
			{
				_scenePool.Unpause();
			}
			else
			{
				_scenePool.Pause();
			}
		}
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
				//SphereView sphereView1 = _scenePool.GetObject<SphereView>();
				//SphereView sphereView2 = _scenePool.GetObject<SphereView>().SetPosition(new Vector3());
				_scenePool.GetObject<SphereView>().SetPosition(new Vector3()).Initialize();

				//float random = 10;

				//float x = Random.Range(-random, random);
				//float y = Random.Range(-random, random);
				//float z = Random.Range(0, random);

				//Vector3 randomScatter = new Vector3(x, y, z);
				//Vector3 direction = (Vector3.forward + randomScatter).normalized;

				//sphereView.Rigidbody.velocity = direction * 10;
			}
			yield return new PausableWaitForSeconds(Time.fixedDeltaTime, () => _scenePool.IsPaused);
			//UnityEngine.Debug.Log(_scenePool.GetActiveObjectsOfType<SphereView>().Count);
		}
	}

	[ContextMenu("StartTest")]
	private async void StartTest()
	{
		StartCoroutine(SpawnBalls());

		return;

#pragma warning disable 0162

		IPoolObject poolObject1 = _scenePool.GetObject<CubeView>()
			.SetPosition(new Vector3(1, 1, -1))
			.SetLocalScale(new Vector3(1, 0.5f, 1))
			.SetParent(null);

		_scenePool.GetObject<CubeView>()
			.SetPosition(new Vector3(1, 1, 1))
			.SetParent(null);

		IPoolObject poolObject2 = _scenePool.GetObject<CylinderView>();


		return;

		//---------------------------------------------------------------------------------

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < 10000; i++)
		{
			_scenePool.GetObject<SphereView>();
		}
		stopwatch.Stop();
		UnityEngine.Debug.Log("Pooling: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------
		await System.Threading.Tasks.Task.Delay(5000);

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_scenePool.ReturnToPoolAllObjectsOfType<SphereView>();
		stopwatch.Stop();
		UnityEngine.Debug.Log("ReturnToPoolObjectsOfType: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------
		await System.Threading.Tasks.Task.Delay(5000);

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < 10000; i++)
		{
			_scenePool.GetObject<SphereView>();
		}
		stopwatch.Stop();
		UnityEngine.Debug.Log("Pooling: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------
		await System.Threading.Tasks.Task.Delay(5000);

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_scenePool.ReturnToPoolAllObjectsOfType<SphereView>();
		stopwatch.Stop();
		UnityEngine.Debug.Log("ReturnToPoolObjectsOfType: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------
		await System.Threading.Tasks.Task.Delay(5000);

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < 10000; i++)
		{
			_scenePool.GetObject<SphereView>();
		}
		stopwatch.Stop();
		UnityEngine.Debug.Log("Pooling: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------
		await System.Threading.Tasks.Task.Delay(5000);

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_scenePool.ReturnToPoolAllObjectsOfType<SphereView>();
		stopwatch.Stop();
		UnityEngine.Debug.Log("ReturnToPoolObjectsOfType: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------



















		return;
		//---------------------------------------------------------------------------------

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_scenePool.GetAllObjects();
		stopwatch.Stop();
		UnityEngine.Debug.Log("GetActiveObjects: " + stopwatch.Elapsed.TotalSeconds);



		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_scenePool.GetActiveObjectsOfType<SphereView>();
		stopwatch.Stop();
		UnityEngine.Debug.Log("GetActiveObjectsOfType: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_scenePool.Pause();
		stopwatch.Stop();
		UnityEngine.Debug.Log("Pause: " + stopwatch.Elapsed.TotalSeconds);



		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_scenePool.Unpause();
		stopwatch.Stop();
		UnityEngine.Debug.Log("Unpause: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------
		_scenePool.ReturnToPoolAllObjects();
		//---------------------------------------------------------------------------------


		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_scenePool.ReturnToPoolAllObjects();
		stopwatch.Stop();
		UnityEngine.Debug.Log("ReturnToPoolObjects: " + stopwatch.Elapsed.TotalSeconds);

		stopwatch = new Stopwatch();
		stopwatch = Stopwatch.StartNew();
		_scenePool.ReturnToPoolAllObjectsOfType<SphereView>();
		stopwatch.Stop();
		UnityEngine.Debug.Log("ReturnToPoolObjectsOfType: " + stopwatch.Elapsed.TotalSeconds);

		//---------------------------------------------------------------------------------

#pragma warning restore 0162
	}
}
