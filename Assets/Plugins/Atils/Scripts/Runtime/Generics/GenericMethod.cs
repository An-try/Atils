using System;
using System.Reflection;

namespace Atils.Runtime.Generics
{
	public class GenericMethod
	{
		// https://referencesource.microsoft.com/#mscorlib/system/type.cs,747b1a12f39d3a0d
		private const BindingFlags DEFAULT_LOOKUP = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

		#region Required data

		private Type _sourceClassType = null;
		private string _methodName = string.Empty;
		private object _sourceClassInstance = null;

		#endregion

		#region Additional data

		private BindingFlags _bindingAttr = DEFAULT_LOOKUP;
		private Binder _binder = null;
		private CallingConventions _callConvention = CallingConventions.Any;
		private Type[] _types = Array.Empty<Type>();
		private ParameterModifier[] _modifiers = null;

		#endregion

		#region Additional parameters

		private Type[] _typeArguments = null;
		private object[] _parameters = null;

		#endregion

		public GenericMethod(Type sourceClassType, string methodName, object sourceClassInstance)
		{
			_sourceClassType = sourceClassType;
			_methodName = methodName;
			_sourceClassInstance = sourceClassInstance;
		}

		public T Invoke<T>()
		{
			return (T)Invoke();
		}

		public object Invoke()
		{
			MethodInfo methodInfo = _sourceClassType.GetMethod(_methodName, _bindingAttr, _binder, _callConvention, _types, _modifiers);
			MethodInfo genericMethod = methodInfo.MakeGenericMethod(_typeArguments);
			return genericMethod.Invoke(_sourceClassInstance, _parameters);
		}

		#region Chain methods

		public GenericMethod WithBindingFlags(BindingFlags bindingAttr)
		{
			_bindingAttr = bindingAttr;
			return this;
		}

		public GenericMethod WithBinder(Binder binder)
		{
			_binder = binder;
			return this;
		}

		public GenericMethod WithCallingConventions(CallingConventions callConvention)
		{
			_callConvention = callConvention;
			return this;
		}

		public GenericMethod WithTypes(params Type[] types)
		{
			_types = types;
			return this;
		}

		public GenericMethod WithParameterModifiers(params ParameterModifier[] modifiers)
		{
			_modifiers = modifiers;
			return this;
		}

		public GenericMethod WithTypeArguments(params Type[] typeArguments)
		{
			_typeArguments = typeArguments;
			return this;
		}

		public GenericMethod WithParameters(params object[] parameters)
		{
			_parameters = parameters;
			return this;
		}

		#endregion
	}
}
