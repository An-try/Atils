using UnityEngine;

namespace Atils.Runtime.ScreenUtils
{
	[CreateAssetMenu(fileName = "ScreenServicesConfig", menuName = "Atils/Services/Configs/ScreenServicesConfig")]
    public class ScreenServicesConfig : ScriptableObject
    {
        [SerializeField] private ScreenService _screenService = default;

        public ScreenService ScreenService => _screenService;
    }
}
