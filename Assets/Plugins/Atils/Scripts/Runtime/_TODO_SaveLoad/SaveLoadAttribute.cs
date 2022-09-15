using System;

namespace Atils.Runtime.SaveLoad
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class SaveLoadAttribute : Attribute
	{ }
}
