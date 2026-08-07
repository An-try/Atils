using UnityEngine;

namespace Atils.Runtime.Extensions
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject == null)
            {
                Debug.LogError("GameObject is null. Cannot get or add component.");
                return null;
            }

            if (gameObject.TryGetComponent(out T component))
            {
                return component;
            }
            
            return gameObject.AddComponent<T>();
        }
    }
}
