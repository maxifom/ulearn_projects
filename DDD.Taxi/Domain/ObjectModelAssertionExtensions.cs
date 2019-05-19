using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Ddd.Taxi.Domain
{
    public static class ObjectModelAssertionExtensions
    {
        public static Type AssertHasPropertyOrField(this Type type, string propName, string propTypeName)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
            var field = type.GetField(propName, bindingFlags);
            if (field != null)
            {
                Assert.AreEqual(propTypeName, field.FieldType.Name, type + " has wrong type of field " + propTypeName);
                return field.FieldType;
            }

            var prop = type.GetProperty(propName, bindingFlags);
            Assert.IsNotNull(prop, type + " should have field or property " + propName + " with type " + propTypeName);
            Assert.AreEqual(propTypeName, prop.PropertyType.Name, type + " has wrong type of property " + propTypeName);
            return prop.PropertyType;
        }

        public static MethodInfo AssertHasMethod(this Type type, string methodName)
        {
            var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
            Assert.IsNotNull(method, type.Name + " should have public method " + methodName);
            return method;
        }

        public static void AssertNotAnemic(this Type type)
        {
            var publicProps = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var publicFields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            Assert.IsEmpty(publicProps.Where(p => !(p.SetMethod?.IsPrivate ?? true)),
                type.Name + " should not have writable properties");
            Assert.IsEmpty(publicFields, type.Name + " should not have any public fields");
        }
    }
}