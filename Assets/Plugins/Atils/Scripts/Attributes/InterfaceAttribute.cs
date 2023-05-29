using System;
using UnityEngine;

namespace Atils.Runtime.Attributes
{
	public class InterfaceAttribute : PropertyAttribute
	{
		public Type InterfaceType;

		public InterfaceAttribute(Type type)
		{
			InterfaceType = type;
		}
	}
}
