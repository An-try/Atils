#if UNITY_EDITOR

using Atils.Runtime.Attributes;
using Atils.Runtime.Extensions;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;

namespace TerrainToModel.Runtime
{
    [ExecuteInEditMode]
    public class TerrainToModelConverter : MonoBehaviour
    {
        private enum MeshFormat { Triangles, Quads }
		private enum MeshResolution { Full = 0, Half, Quarter, Eighth, Sixteenth }
		private enum TextureResolution
        {
            _128 = 128,
            _256 = 256,
            _512 = 512,
            _1024 = 1024,
            _2048 = 2048,
            _4096 = 4096,
            _8192 = 8192
        }

        private class ModelCalculationData
		{
            public TerrainData TerrainData { get; set; } = default;
            public Camera Camera { get; set; } = default;
            public Light Light { get; set; } = default;
            public Vector3 TerrainPos { get; set; } = default;
            public float DefaultBaseMapDistance { get; set; } = default;
            public int TCount { get; set; } = default;
            public int Counter { get; set; } = default;
            public int TotalCount { get; set; } = default;
            public int ProgressUpdateInterval { get; set; } = 10000;
        }

		[Header("General")]
		[SerializeField] private Terrain _terrain = default;
		[SerializeField, Optional] private TerrainCollider _terrainCollider = default;

		[Header("Mesh settings")]
        [SerializeField] private MeshFormat _meshFormat = MeshFormat.Triangles;
        [SerializeField] private MeshResolution _meshResolution = MeshResolution.Full;

        [Header("Texture settings")]
        [SerializeField] private Camera _cameraPrefab = default;
        [SerializeField] private Light _lightPrefab = default;
        [SerializeField] private TextureResolution _textureResolution = TextureResolution._4096;

        private bool _isExporting = default;
        private ModelCalculationData _modelCalculationData = default;

        private TerrainModelContainer _terrainModelContainer => GetComponentInChildren<TerrainModelContainer>();

        public Terrain Terrain => _terrain;

        private void OnGUI()
		{
			if (!_isExporting && _terrainModelContainer != null)
			{
                if (_terrain.enabled ||
                    (_terrainCollider != null && _terrainCollider.enabled))
                {
                    ClearModelAndHelperObjects();
                }
			}
		}

		public async void ExportToModel()
        {
            Assert.IsNotNull(_terrain);
            Assert.IsNotNull(_terrain.terrainData);
            Assert.IsNotNull(_cameraPrefab);
            Assert.IsNotNull(_lightPrefab);

            string path = EditorUtility.SaveFilePanelInProject(
                "Export .obj file",
                _terrain.terrainData.name,
                "obj",
                "",
                Path.GetDirectoryName(AssetDatabase.GetAssetPath(_terrain.terrainData)));

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            _isExporting = true;

            DisableAllLights(out Tuple<Light, bool>[] lightsData);

            transform.rotation = Quaternion.identity;

            _modelCalculationData = new ModelCalculationData();

            _modelCalculationData.TerrainData = _terrain.terrainData;
            _modelCalculationData.TerrainPos = _terrain.transform.position;

            ClearModelAndHelperObjects();
            CreateHelperObjects(out float cameraFarClipPlane);

            _modelCalculationData.DefaultBaseMapDistance = _terrain.basemapDistance;
            _terrain.basemapDistance = cameraFarClipPlane + Mathf.Max(_modelCalculationData.TerrainData.size.x, _modelCalculationData.TerrainData.size.z);

            await Task.Delay(10);

            string folderPath = Path.GetDirectoryName(path);
            string modelPath = Path.Combine(folderPath, _modelCalculationData.TerrainData.name + ".obj");
            string texturePath = Path.Combine(folderPath, _modelCalculationData.TerrainData.name + ".png");
            string materialPath = Path.Combine(folderPath, _modelCalculationData.TerrainData.name + ".mat");

            ExportModel(modelPath);
            ExportTexture(texturePath);

            AssetDatabase.Refresh();

            UpdateTextureImportSettings(texturePath);

            SetUpModel(modelPath, texturePath, materialPath);
            SetEnabledTerrainComponents(false);

            DestroyImmediate(_modelCalculationData.Light.gameObject);
            DestroyImmediate(_modelCalculationData.Camera.gameObject);

            _terrain.basemapDistance = _modelCalculationData.DefaultBaseMapDistance;

            _modelCalculationData = null;

            RestoreLights(lightsData);

            _isExporting = false;

            EditorSceneManager.MarkSceneDirty(gameObject.scene);

            Debug.Log("The terrain has been successfully exported and created on the scene.");
        }

        private void DisableAllLights(out Tuple<Light, bool>[] lightsData)
		{
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            lightsData = new Tuple<Light, bool>[lights.Length];

            for (int i = 0; i < lights.Length; i++)
			{
                lightsData[i] = new Tuple<Light, bool>(lights[i], lights[i].enabled);
                lights[i].enabled = false;
            }
        }

        private void RestoreLights(Tuple<Light, bool>[] lightsData)
		{
            for (int i = 0; i < lightsData.Length; i++)
            {
                lightsData[i].Item1.enabled = lightsData[i].Item2;
            }
        }

        public void ClearModelAndHelperObjects()
		{
            SetEnabledTerrainComponents(true);

            bool logAboutSuccessfulOperation = _terrainModelContainer != null;
            DestroyImmediate(_terrainModelContainer?.gameObject);

            EditorSceneManager.MarkSceneDirty(gameObject.scene);

            if (logAboutSuccessfulOperation)
            {
                Debug.Log("The terrain model has been removed from the scene.");
            }
        }

        private void CreateHelperObjects(out float cameraFarClipPlane)
        {
            float terrainSizeX = _modelCalculationData.TerrainData.size.x;
            float terrainSizeY = _modelCalculationData.TerrainData.size.y;
            float terrainSizeZ = _modelCalculationData.TerrainData.size.z;

            GameObject go = new GameObject(nameof(TerrainModelContainer), typeof(TerrainModelContainer));
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(
                _modelCalculationData.TerrainPos.x,
                -_modelCalculationData.TerrainPos.y,
                -_modelCalculationData.TerrainPos.z);

            Camera camera = Instantiate(_cameraPrefab, _terrainModelContainer.transform);
            camera.transform.localPosition = new Vector3(terrainSizeX / 2, terrainSizeY + 100, terrainSizeZ / 2);
            camera.orthographicSize = Mathf.Max(terrainSizeX / 2, terrainSizeZ / 2);
            camera.farClipPlane = camera.transform.localPosition.y + 100;
            camera.cullingMask = 1 << LayerMask.NameToLayer("Terrain");
            _modelCalculationData.Camera = camera;

            Light light = Instantiate(_lightPrefab, _terrainModelContainer.transform);
            light.cullingMask = 1 << LayerMask.NameToLayer("Terrain");
            _modelCalculationData.Light = light;

            cameraFarClipPlane = camera.farClipPlane;
        }

        private void ExportModel(string finalPath)
		{
            int w = _modelCalculationData.TerrainData.heightmapResolution;
            int h = _modelCalculationData.TerrainData.heightmapResolution;
            Vector3 meshScale = _modelCalculationData.TerrainData.size;
            int tRes = (int)Mathf.Pow(2, (int)_meshResolution);
            meshScale = new Vector3(meshScale.x / (w - 1) * tRes, meshScale.y, meshScale.z / (h - 1) * tRes);
            Vector2 uvScale = new Vector2(1.0f / (w - 1), 1.0f / (h - 1));
            float[,] tData = _modelCalculationData.TerrainData.GetHeights(0, 0, w, h);

            w = (w - 1) / tRes + 1;
            h = (h - 1) / tRes + 1;
            Vector3[] tVertices = new Vector3[w * h];
            Vector2[] tUV = new Vector2[w * h];

            int[] tPolys;

            if (_meshFormat == MeshFormat.Triangles)
            {
                tPolys = new int[(w - 1) * (h - 1) * 6];
            }
            else
            {
                tPolys = new int[(w - 1) * (h - 1) * 4];
            }

            // Build vertices and UVs
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    tVertices[y * w + x] = Vector3.Scale(meshScale, new Vector3(-y, tData[x * tRes, y * tRes], x)) + _modelCalculationData.TerrainPos;
                    tUV[y * w + x] = Vector2.Scale(new Vector2(x * tRes, y * tRes), uvScale);
                }
            }

            int index = 0;
            if (_meshFormat == MeshFormat.Triangles)
            {
                // Build triangle indices: 3 indices into vertex array for each triangle
                for (int y = 0; y < h - 1; y++)
                {
                    for (int x = 0; x < w - 1; x++)
                    {
                        // For each grid cell output two triangles
                        tPolys[index++] = (y * w) + x;
                        tPolys[index++] = ((y + 1) * w) + x;
                        tPolys[index++] = (y * w) + x + 1;

                        tPolys[index++] = ((y + 1) * w) + x;
                        tPolys[index++] = ((y + 1) * w) + x + 1;
                        tPolys[index++] = (y * w) + x + 1;
                    }
                }
            }
            else
            {
                // Build quad indices: 4 indices into vertex array for each quad
                for (int y = 0; y < h - 1; y++)
                {
                    for (int x = 0; x < w - 1; x++)
                    {
                        // For each grid cell output one quad
                        tPolys[index++] = (y * w) + x;
                        tPolys[index++] = ((y + 1) * w) + x;
                        tPolys[index++] = ((y + 1) * w) + x + 1;
                        tPolys[index++] = (y * w) + x + 1;
                    }
                }
            }

            // Export to .obj
            StreamWriter sw = new StreamWriter(finalPath);
            try
            {

                sw.WriteLine("# Unity terrain OBJ File");

                // Write vertices
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                _modelCalculationData.Counter = _modelCalculationData.TCount = 0;
                _modelCalculationData.TotalCount =
                    (tVertices.Length * 2 + (_meshFormat == MeshFormat.Triangles ? tPolys.Length / 3 : tPolys.Length / 4)) / _modelCalculationData.ProgressUpdateInterval;

                for (int i = 0; i < tVertices.Length; i++)
                {
                    UpdateProgress();
                    StringBuilder sb = new StringBuilder("v ", 20);
                    // StringBuilder stuff is done this way because it's faster than using the "{0} {1} {2}"etc. format
                    // Which is important when you're exporting huge terrains.
                    sb.Append(tVertices[i].x.ToString()).Append(" ").
                       Append(tVertices[i].y.ToString()).Append(" ").
                       Append(tVertices[i].z.ToString());
                    sw.WriteLine(sb);
                }
                // Write UVs
                for (int i = 0; i < tUV.Length; i++)
                {
                    UpdateProgress();
                    StringBuilder sb = new StringBuilder("vt ", 22);
                    sb.Append(tUV[i].x.ToString()).Append(" ").
                       Append(tUV[i].y.ToString());
                    sw.WriteLine(sb);
                }
                if (_meshFormat == MeshFormat.Triangles)
                {
                    // Write triangles
                    for (int i = 0; i < tPolys.Length; i += 3)
                    {
                        UpdateProgress();
                        StringBuilder sb = new StringBuilder("f ", 43);
                        sb.Append(tPolys[i] + 1).Append("/").Append(tPolys[i] + 1).Append(" ").
                           Append(tPolys[i + 1] + 1).Append("/").Append(tPolys[i + 1] + 1).Append(" ").
                           Append(tPolys[i + 2] + 1).Append("/").Append(tPolys[i + 2] + 1);
                        sw.WriteLine(sb);
                    }
                }
                else
                {
                    // Write quads
                    for (int i = 0; i < tPolys.Length; i += 4)
                    {
                        UpdateProgress();
                        StringBuilder sb = new StringBuilder("f ", 57);
                        sb.Append(tPolys[i] + 1).Append("/").Append(tPolys[i] + 1).Append(" ").
                           Append(tPolys[i + 1] + 1).Append("/").Append(tPolys[i + 1] + 1).Append(" ").
                           Append(tPolys[i + 2] + 1).Append("/").Append(tPolys[i + 2] + 1).Append(" ").
                           Append(tPolys[i + 3] + 1).Append("/").Append(tPolys[i + 3] + 1);
                        sw.WriteLine(sb);
                    }
                }
            }
            catch (Exception err)
            {
                Debug.Log("Error saving file: " + err.Message);
            }
            sw.Close();

            EditorUtility.DisplayProgressBar("Saving file to disc.", "This might take a while...", 1f);
            EditorUtility.ClearProgressBar();
        }

        private void UpdateProgress()
        {
            if (_modelCalculationData.Counter++ == _modelCalculationData.ProgressUpdateInterval)
            {
                _modelCalculationData.Counter = 0;
                EditorUtility.DisplayProgressBar("Saving...", "", Mathf.InverseLerp(0, _modelCalculationData.TotalCount, ++_modelCalculationData.TCount));
            }
        }
        
        private void ExportTexture(string finalPath)
		{
            int width = (int)_textureResolution;
            int height = (int)_textureResolution;

			Rect rect = new Rect(0, 0, width, height);
			RenderTexture renderTexture = new RenderTexture(width, height, 24);
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);

			_modelCalculationData.Camera.targetTexture = renderTexture;
            _modelCalculationData.Camera.Render();

			RenderTexture.active = renderTexture;

			texture2D.ReadPixels(rect, 0, 0);
			texture2D.Apply();

            _modelCalculationData.Camera.targetTexture = null;
			RenderTexture.active = null;

            FlipTexture(ref texture2D);
            SaveTexture(texture2D, finalPath);
        }

        public static void FlipTexture(ref Texture2D texture)
        {
            int textureWidth = texture.width;
            int textureHeight = texture.height;

            Color32[] pixels = texture.GetPixels32();

            for (int y = 0; y < textureHeight; y++)
            {
                int yo = y * textureWidth;
                for (int il = yo, ir = yo + textureWidth - 1; il < ir; il++, ir--)
                {
                    Color32 col = pixels[il];
                    pixels[il] = pixels[ir];
                    pixels[ir] = col;
                }
            }
            texture.SetPixels32(pixels);
            texture.Apply();
        }

        private void SaveTexture(Texture2D texture2D, string finalPath)
        {
            byte[] bytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(finalPath, bytes);
        }

        private void UpdateTextureImportSettings(string finalPath)
		{
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(finalPath);

            TextureImporterSettings importerSettings = new TextureImporterSettings();
            importer.ReadTextureSettings(importerSettings);
            importerSettings.mipmapEnabled = false;
            importer.SetTextureSettings(importerSettings);

            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        }

        private void SetUpModel(string modelPath, string texturePath, string materialPath)
		{
            GameObject modelAsset = AssetDatabase.LoadAssetAtPath(modelPath, typeof(GameObject)) as GameObject;
            Texture textureAsset = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture)) as Texture;

			Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            AssetDatabase.CreateAsset(material, materialPath);

            GameObject model = Instantiate(modelAsset, _terrainModelContainer.transform);
            model.tag = "Terrain";
            model.GetComponentsInChildren<Transform>().ForEach(x => x.tag = "Terrain");

            MeshRenderer[] meshRenderers = model.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
			{
				meshRenderer.sharedMaterial = material;

				meshRenderer.sharedMaterial.mainTexture = textureAsset;
				meshRenderer.sharedMaterial.color = new Color32(204, 204, 204, 255);
				meshRenderer.sharedMaterial.SetFloat("_Smoothness", 0);

				float terrainWidth = _modelCalculationData.TerrainData.size.x;
				float terrainLength = _modelCalculationData.TerrainData.size.z;
				// when terrain is not square
				if (terrainWidth > terrainLength)
				{
					float tilingX = terrainLength / terrainWidth;
					float offsetX = (1 - tilingX) / 2;
					meshRenderer.sharedMaterial.mainTextureScale = new Vector2(tilingX, meshRenderer.sharedMaterial.mainTextureScale.y);
					meshRenderer.sharedMaterial.mainTextureOffset = new Vector2(offsetX, meshRenderer.sharedMaterial.mainTextureOffset.y);
				}
				else if (terrainWidth < terrainLength)
				{
					float tilingY = terrainWidth / terrainLength;
					float offsetY = (1 - tilingY) / 2;
					meshRenderer.sharedMaterial.mainTextureScale = new Vector2(meshRenderer.sharedMaterial.mainTextureScale.x, tilingY);
					meshRenderer.sharedMaterial.mainTextureOffset = new Vector2(meshRenderer.sharedMaterial.mainTextureOffset.x, offsetY);
				}

                if (meshRenderer.GetComponent<MeshCollider>() == null)
                {
                    meshRenderer.gameObject.AddComponent(typeof(MeshCollider));
                }
            }

			EditorUtility.SetDirty(model);
        }

        private void SetEnabledTerrainComponents(bool enabled)
		{
            _terrain.enabled = enabled;

            if (_terrainCollider != null)
            {
                _terrainCollider.enabled = enabled;
            }
        }
    }
}

#endif
