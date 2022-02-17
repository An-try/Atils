using Atils.Runtime.Attributes;
using UnityEngine;

public class ReadOnlyPropertyTest : MonoBehaviour
{
	[ReadOnly] public int q;
	[ReadOnly] protected int w;
	[ReadOnly] private int e;
	[ReadOnly] private protected int r;

	[SerializeField, ReadOnly] public int t;
	[SerializeField, ReadOnly] protected int y;
	[SerializeField, ReadOnly] private int u;
	[SerializeField, ReadOnly] private protected int i;
}
