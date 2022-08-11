using UnityEngine;

namespace Atils.Runtime.Extensions
{
	public static class Vector3Extensions
    {
        public static Vector3 TransformPoint(this Vector3 positionToTransform, Vector3 originalPosition, Quaternion originalRotation, Vector3 originalScale)
		{
            Matrix4x4 matrix = Matrix4x4.TRS(originalPosition, originalRotation, originalScale);
            return matrix.MultiplyPoint3x4(positionToTransform);
        }

        public static Vector3 InverseTransformPoint(this Vector3 positionToTransform, Vector3 originalPosition, Quaternion originalRotation, Vector3 originalScale)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(originalPosition, originalRotation, originalScale);
            matrix = matrix.inverse;
            return matrix.MultiplyPoint3x4(positionToTransform);
        }
    }
}
