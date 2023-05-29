using System;

namespace Atils.Runtime.Generics
{
	public static class GenericMethodGenerator
    {
        /// <summary>
        /// Creates a generic method from a class. The search for the required method is performed by the class name in the specified class type.
        /// If the class is not static, the method will be searched in an instance of this class.
        /// For a more detailed search for the required method, use the chain of methods from <see cref="GenericMethod"/>.
        /// </summary>
        /// <param name="sourceClassType">The source class to search for the method</param>
        /// <param name="methodName">Method name to search</param>
        /// <param name="sourceClassInstance">The instance of the class to search for the method. Must be null if it is a static class</param>
        /// <returns>An object representing a genetic method and methods for detailed search and invocation of this generic method</returns>
        public static GenericMethod GetGenericMethod(Type sourceClassType, string methodName, object sourceClassInstance)
		{
            return new GenericMethod(sourceClassType, methodName, sourceClassInstance);
        }
    }
}
