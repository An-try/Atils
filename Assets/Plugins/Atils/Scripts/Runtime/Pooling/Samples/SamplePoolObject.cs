using Zenject;

namespace Atils.Runtime.Pooling
{
	public class SamplePoolObject : PoolObject
	{
		/// <summary>
		/// This class must be in every MonoBehavior class that will be a pool object.
		/// </summary>
		public class Factory : PlaceholderFactory<IPoolObject>
		{ }

		public int SomeValue = 0;

		protected override void ResetObject()
		{
			SomeValue = 0;
		}
	}
}
