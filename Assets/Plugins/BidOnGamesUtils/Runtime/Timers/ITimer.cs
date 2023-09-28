namespace BidOnGamesUtils.Runtime.Timers
{
	public interface ITimer
	{
		int StartTime { get; }
		float RemainingTime { get; }
		float ElapsedTime { get; }

		void Start(int startSeconds);
		void Stop();
		void Pause();
		void UnPause();
		void DecreaseTime(int addSeconds);
	}
}