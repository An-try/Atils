using System;
using Zenject;

namespace Atils.Runtime.Pooling
{
	public class UsageExample
	{
		[Inject] private ScenePool pool;

		public void Main()
		{
			SamplePoolObject samplePoolObject = pool.GetObject<SamplePoolObject>();
			samplePoolObject.Initialize();
		}
	}

	public class SamplePoolObject : PoolObject
	{
		/// <summary>
		/// This class must be in every MonoBehavior class that will be a pool object.
		/// </summary>
		public class Factory : PlaceholderFactory<IPoolObject>
		{ }

		public override Action<IPoolObject> OnInitializedEvent { get; set; }

		public int SomeValue1 = 0;
		public int SomeValue2 = 0;

		public void Initialize()
		{
			OnInitializedEvent?.Invoke(this);
		}

		public override void UpdateObject(float timeStep)
		{
			if (IsPaused)
			{
				return;
			}

			SomeValue1++;
			SomeValue2++;
		}

		protected override void OnPreReturnedToPool()
		{
			SomeValue1 = 0;
		}

		protected override void OnAfterReturnedToPool()
		{
			SomeValue2 = 0;
		}

		protected override void OnPaused()
		{
			// Do anything you want when the game pauses
		}

		protected override void OnUnpaused()
		{
			// Do anything you want when the game unpauses
		}
	}
}
