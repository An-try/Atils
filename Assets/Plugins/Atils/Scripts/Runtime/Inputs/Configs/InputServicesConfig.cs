using UnityEngine;

namespace Atils.Runtime.Inputs
{
	[CreateAssetMenu(fileName = "InputServicesConfig", menuName = "Atils/Services/Configs/InputServicesConfig")]
	public class InputServicesConfig : ScriptableObject
	{
		[SerializeField] private MouseInputService _mouseInputService = default;
		[SerializeField] private TouchInputService _touchInputService = default;

		public MouseInputService MouseInputService => _mouseInputService;
		public TouchInputService TouchInputService => _touchInputService;
	}
}
