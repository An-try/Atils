using UnityEngine;

namespace Atils.Runtime.Extensions
{
	public static class Vector3Extensions
    {
		public static Vector3 GetSetX(this Vector3 source, float value)
		{
			return new Vector3(value, source.y, source.z);
		}

		public static Vector3 GetSetY(this Vector3 source, float value)
		{
			return new Vector3(source.x, value, source.z);
		}

		public static Vector3 GetSetZ(this Vector3 source, float value)
		{
			return new Vector3(source.x, source.y, value);
		}

		public static Vector3 Randomize(this Vector3 source, float minInclusive, float maxInclusive)
		{
			source.x = Random.Range(minInclusive, maxInclusive);
			source.y = Random.Range(minInclusive, maxInclusive);
			source.z = Random.Range(minInclusive, maxInclusive);
			return source;
		}

		public static Vector3 Randomize(this Vector3 source, Vector3 minInclusive, Vector3 maxInclusive)
		{
			source.x = Random.Range(minInclusive.x, maxInclusive.x);
			source.y = Random.Range(minInclusive.y, maxInclusive.y);
			source.z = Random.Range(minInclusive.z, maxInclusive.z);
			return source;
		}

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
