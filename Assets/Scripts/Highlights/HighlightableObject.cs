using UnityEngine;

public class HighlightableObject : MonoBehaviour
{
	[SerializeField] private Material _highlightMaterial = default;
	[SerializeField] private MeshRenderer[] _renderers = default;

	private Material[] _highlightMaterials = default;
	private MaterialsContainer[] _defaultMaterialsContainers = default;

	private void Awake()
	{
		CacheHighlightAndDefaultMaterials();
	}

	public void Highlight()
	{
		for (int i = 0; i < _renderers.Length; i++)
		{
			_renderers[i].materials = _highlightMaterials;
		}
	}

	public void UnHighlight()
	{
		for (int i = 0; i < _renderers.Length; i++)
		{
			_renderers[i].materials = _defaultMaterialsContainers[i].Materials;
		}
	}

	private void CacheHighlightAndDefaultMaterials()
	{
		_highlightMaterials = new Material[_renderers.Length];
		_defaultMaterialsContainers = new MaterialsContainer[_renderers.Length];

		for (int i = 0; i < _renderers.Length; i++)
		{
			Material[] defaultMaterials = _renderers[i].materials;

			_highlightMaterials[i] = _highlightMaterial;
			_defaultMaterialsContainers[i] = new MaterialsContainer(defaultMaterials);
		}
	}
}
