using UnityEngine;

namespace Atils.Runtime.Inputs
{
	public interface IOVRRaycaster
    {
        RaycastHit RaycastHit { get; }

        void Initialize(IOVRCameraRig ovrCameraRig);
        void ShowLine();
        void HideLine();
        void Pause();
        void Unpause();
    }
}
