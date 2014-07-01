using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Newtonsoft.Json.Linq;


namespace LibJSONExt
{

	public static class StaticJObjectExt
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private static void __Write(string s, StringBuilder sw)
		{
			sw.Append('\"');
			foreach (char c in s) {
				switch (c) {
					case '"':
						sw.Append("\\\"");
						break;
					case '\\':
						sw.Append("\\\\");
						break;
					case '/':
						sw.Append("\\/");
						break;
					case '\b':
						sw.Append("\\\b");
						break;
					case '\f':
						sw.Append("\\\f");
						break;
					case '\n':
						sw.Append("\\\n");
						break;
					case '\r':
						sw.Append("\\\r");
						break;
					case '\t':
						sw.Append("\\\t");
						break;
					default:
						if (c <= 31) {
							sw.Append("\\u" + ((int)c).ToString("X4"));
						} else {
							sw.Append(c);
						}
						break;
				}
			}
			sw.Append('\"');
		}

		private static void __Write(JObject obj, StringBuilder sw)
		{
			SortedList<string, JProperty> properties = new SortedList<string, JProperty>();
			foreach (JProperty p in obj.Properties()) {
				properties.Add(p.Name, p);
			}

			sw.Append('{');

			int i = 0;
			foreach (string key in properties.Keys) {
				if (i > 0) sw.Append(',');
				JProperty p = properties[key];
				__Write(p.Name, sw);
				sw.Append(':');
				__WriteAny(p.Value, sw);
				i++;
			}

			sw.Append('}');
		}

		private static void __WriteAny(JToken t, StringBuilder sw)
		{
			if (t == null) {
				sw.Append("null");
				return;
			}

			switch (t.Type) {
				case JTokenType.Array:
					__Write((JArray)t, sw);
					break;
				case JTokenType.Object:
					__Write((JObject)t, sw);
					break;
				default:
					sw.Append(t.ToString(Newtonsoft.Json.Formatting.None));
					break;
			}
		}

		private static void __Write(JArray a, StringBuilder sw)
		{
			sw.Append('[');
			for (int i = 0; i < a.Count; i++) {
				if (i > 0) sw.Append(',');
				JToken e = a[i];
				__WriteAny(e, sw);
			}
			sw.Append(']');
		}

		private static void __Write(JValue v, StringBuilder sw)
		{
			sw.Append(v.ToString(Newtonsoft.Json.Formatting.None));
		}

		////////////////////////////////////////////////////////////////

		public static string _ToClearlyDefinedString(this JObject obj)
		{
			StringBuilder sb = new StringBuilder();
			__Write(obj, sb);
			return sb.ToString();
		}

		public static JObject _CreatePropertyObject(this JObject obj, string propertyName)
		{
			obj.Remove(propertyName);
			JObject objNew = new JObject();
			obj.Add(propertyName, objNew);
			return objNew;
		}

		public static JArray _CreatePropertyArray(this JObject obj, string propertyName)
		{
			obj.Remove(propertyName);
			JArray objNew = new JArray();
			obj.Add(propertyName, objNew);
			return objNew;
		}

		public static JObject _GetCreatePropertyObject(this JObject obj, string propertyName)
		{
			JProperty p = obj.Property(propertyName);
			if (p == null) return _CreatePropertyObject(obj, propertyName);
			if (p.Value is JObject) return (JObject)(p.Value);
			throw new Exception("Property value is of type " + p.Value.GetType().Name + " and not of type JObject!");
		}

		public static JObject _GetPropertyObject(this JObject obj, string propertyName)
		{
			JProperty p = obj.Property(propertyName);
			if (p == null) return null;
			if (p.Value is JObject) return (JObject)(p.Value);
			throw new Exception("Property value is of type " + p.Value.GetType().Name + " and not of type JObject!");
		}

		public static JArray _GetPropertyArray(this JObject obj, string propertyName)
		{
			JProperty p = obj.Property(propertyName);
			if (p == null) return null;
			if (p.Value is JArray) return (JArray)(p.Value);
			throw new Exception("Property value is of type " + p.Value.GetType().Name + " and not of type JArray!");
		}

		public static JObject _SetProperty(this JObject obj, string propertyName, JObject childObj)
		{
			obj.Remove(propertyName);
			if (childObj != null) {
				obj.Add(propertyName, childObj);
			}
			return childObj;
		}

		public static void _SetPropertyValue(this JObject obj, string propertyName, object value)
		{
			obj.Remove(propertyName);
			if (value != null) {
				JValue objNew = new JValue(value);
				obj.Add(propertyName, objNew);
			}
		}

		public static void _SetPropertyValueArray(this JObject obj, string propertyName, object[] arrayData)
		{
			obj.Remove(propertyName);
			if (arrayData != null) {
				JArray objNew = new JArray();
				foreach (object value in arrayData) {
					objNew.Add(new JValue(value));
				}
				obj.Add(propertyName, objNew);
			}
		}

		public static void _SetPropertyArray(this JObject obj, string propertyName, JToken[] arrayData)
		{
			obj.Remove(propertyName);
			if (arrayData != null) {
				JArray objNew = new JArray();
				foreach (object value in arrayData) {
					objNew.Add(value);
				}
				obj.Add(propertyName, objNew);
			}
		}

		public static object _GetPropertyValue(this JObject obj, string propertyName)
		{
			JProperty p = obj.Property(propertyName);
			if (p == null) return null;
			if (p.Value is JValue) return ((JValue)(p.Value)).Value;
			throw new Exception("Property value is of type " + p.Value.GetType().Name + " and not of type JObject!");
		}

		public static object _GetPropertyValueNotNull(this JObject obj, string propertyName)
		{
			object v = _GetPropertyValue(obj, propertyName);
			if (v == null) throw new Exception("No value for property '" + propertyName + "'");
			return v;
		}

		public static string GetPropertyValueAsNormalizedString(this JObject obj, string propertyName)
		{
			if (propertyName == null) throw new Exception("propertyName == null!");

			JProperty p = obj.Property(propertyName);
			if (p == null) return null;
			if (p.Value == null) return null;
			string s = p.Value.ToString();
			s = s.Trim();
			if (s.Length == 0) return null;
			return s;
		}

		public static JArray GetPropertyValueAsArray(this JObject obj, string propertyName)
		{
			if (propertyName == null) throw new Exception("propertyName == null!");

			JProperty p = obj.Property(propertyName);
			if (p == null) return null;
			if (p.Value == null) return null;
			if (!(p.Value is JArray)) return null;
			JArray a = (JArray)(p.Value);
			return a;
		}

		public static JObject GetPropertyValueAsObject(this JObject obj, string propertyName)
		{
			if (propertyName == null) throw new Exception("propertyName == null!");

			JProperty p = obj.Property(propertyName);
			if (p == null) return null;
			if (p.Value == null) return null;
			if (!(p.Value is JObject)) return null;
			JObject o = (JObject)(p.Value);
			return o;
		}

		public static void SetStringValueTrim(this JObject obj, string propertyName, string value)
		{
			if (propertyName == null) throw new Exception("propertyName == null!");

			obj.Remove(propertyName);

			if (value == null) return;

			value = value.Trim();
			if (value.Length == 0) return;

			obj.Add(propertyName, new JValue(value));
		}

		public static void SetStringArray(this JObject obj, string propertyName, string[] values)
		{
			if (propertyName == null) throw new Exception("propertyName == null!");

			obj.Remove(propertyName);

			if (values == null) return;

			obj.Add(propertyName, new JArray(values));
		}

		public static void SetValue(this JObject obj, string propertyName, object value)
		{
			if (propertyName == null) throw new Exception("propertyName == null!");

			obj.Remove(propertyName);

			if (value == null) return;

			if (value is JObject) {
				obj.Add(propertyName, (JObject)value);
			} else
			if (value is JArray) {
				obj.Add(propertyName, (JArray)value);
			} else
			if (value is JValue) {
				obj.Add(propertyName, (JValue)value);
			} else
			if (value is string) {
				obj.Add(propertyName, new JValue((string)value));
			} else
			if (value is int) {
				obj.Add(propertyName, new JValue((int)value));
			} else
			if (value is long) {
				obj.Add(propertyName, new JValue((long)value));
			} else
			if (value is double) {
				obj.Add(propertyName, new JValue((double)value));
			} else
			if (value is bool) {
				obj.Add(propertyName, new JValue((bool)value));
			} else
				throw new Exception("Unexpected data type: " + value.GetType().Name);
		}

	}

}
