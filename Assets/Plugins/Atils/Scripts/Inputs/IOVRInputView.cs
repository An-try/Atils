using UnityEngine;

namespace Atils.Runtime.Inputs
{
	public interface IOVRInputView
	{
		Transform RayPoint { get; }
		bool IsActive { get; }
	}
}
