using TerrainToModel.Runtime;
using UnityEditor;
using UnityEngine;

namespace TerrainToModel.Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(TerrainToModelConverter))]
	public class TerrainToModelConverterEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			TerrainToModelConverter terrainToModelConverter = (TerrainToModelConverter)target;

			if (terrainToModelConverter.Terrain != null &&
					terrainToModelConverter.Terrain.terrainData != null &&
					terrainToModelConverter.Terrain.terrainData.size.x != terrainToModelConverter.Terrain.terrainData.size.z)
			{
				EditorGUILayout.HelpBox(
					$"Since the terrain is not square, the texture will be worse in quality" +
					$"\nCurrent width = {terrainToModelConverter.Terrain.terrainData.size.x}, length = {terrainToModelConverter.Terrain.terrainData.size.z}",
					MessageType.Warning);
			}

			if (LayerMask.LayerToName(terrainToModelConverter.gameObject.layer) != "Terrain")
			{
				EditorGUILayout.HelpBox("This gameobject must have a \"Terrain\" layer!", MessageType.Error);
			}
			else if (Selection.gameObjects != null && Selection.gameObjects.Length <= 1)
			{
				if (GUILayout.Button("Export to model"))
				{
					terrainToModelConverter.ExportToModel();
				}

				if (GUILayout.Button("Clear model from scene"))
				{
					terrainToModelConverter.ClearModelAndHelperObjects();
				}
			}
			GUILayout.Space(10);

			DrawDefaultInspector();
		}
	}
}
