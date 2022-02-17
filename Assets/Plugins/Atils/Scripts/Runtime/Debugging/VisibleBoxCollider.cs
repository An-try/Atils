using UnityEngine;

namespace Atils.Runtime.Debugging
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(BoxCollider))]
	public class VisibleBoxCollider : MonoBehaviour
	{
#if UNITY_EDITOR

		[SerializeField] private bool _showColliders = true;

		[Header("Normal color settings")]
		[SerializeField] private Color _boxGizmoColor = new Color32(39, 144, 183, 255);
		[SerializeField] private Color _boxFillColor = new Color32(66, 174, 202, 92);

		[Header("Selected color settings")]
		[SerializeField] private Color _boxSelectedColor = new Color32(101, 69, 255, 255);
		[SerializeField] private Color _boxSelectedFillColor = new Color32(53, 38, 245, 116);

		private BoxCollider _boxCollider = default;

		private void Awake()
		{
			_boxCollider = GetComponent<BoxCollider>();
		}

		private void OnDrawGizmos()
		{
			if (!_showColliders || !_boxCollider)
			{
				return;
			}

			Gizmos.matrix = transform.localToWorldMatrix;

			Gizmos.color = _boxGizmoColor;
			Gizmos.DrawWireCube(_boxCollider.center, _boxCollider.size);

			Gizmos.color = _boxFillColor;
			Gizmos.DrawCube(_boxCollider.center, _boxCollider.size);
		}

		private void OnDrawGizmosSelected()
		{
			if (!_showColliders || !_boxCollider)
			{
				return;
			}

			Gizmos.matrix = transform.localToWorldMatrix;

			Gizmos.color = _boxSelectedColor;
			Gizmos.DrawWireCube(_boxCollider.center, _boxCollider.size);

			Gizmos.color = _boxSelectedFillColor;
			Gizmos.DrawCube(_boxCollider.center, _boxCollider.size);
		}

#endif
	}
}
