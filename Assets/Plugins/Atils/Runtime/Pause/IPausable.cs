namespace Atils.Runtime.Pause
{
	public interface IPausable
	{
		bool IsPaused { get; }

		void Pause();
		void Unpause();
	}
}
