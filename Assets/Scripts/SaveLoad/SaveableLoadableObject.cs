using Atils.Runtime.SaveLoad;
using UnityEngine;

public class SaveableLoadableObject : MonoBehaviour
{
	[SaveLoad]
	public string TypeName { get; set; }

	[SaveLoad]
	public Vector3 Position
	{
		get => transform.position;
		set => transform.position = value;
	}

	[SaveLoad]
	public Quaternion Rotation
	{
		get => transform.rotation;
		set => transform.rotation = value;
	}

	[SaveLoad]
	public Vector3 LocalScale
	{
		get => transform.localScale;
		set => transform.localScale = value;
	}
}
