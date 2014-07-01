using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace LibJSONExt
{

	public class JSONValueConverter
	{

		public enum EnumDataType : int
		{
			Unknown = 0,

			BooleanValue = 1,
			IntegerValue = 2,
			LongValue = 3,
			DoubleValue = 4,
			StringValue = 5,
			Object = 15,

			Array = 32,

			BooleanValueArray = 33,
			IntegerValueArray = 34,
			LongValueArray = 35,
			DoubleValueArray = 36,
			StringValueArray = 37,
			ObjectArray = 47,
		}

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		private JSONValueConverter()
		{
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public static EnumDataType DetectDataTypeFromToken(JToken t)
		{
			if (t == null) return EnumDataType.Unknown;
			if (t is JValue) {
				object v = ((JValue)t).Value;
				if (v is bool) return EnumDataType.BooleanValue;
				if (v is int) return EnumDataType.IntegerValue;
				if (v is long) return EnumDataType.LongValue;
				if (v is double) return EnumDataType.DoubleValue;
				if (v is string) return EnumDataType.StringValue;
				return EnumDataType.Unknown;
			} else
			if (t is JArray) {
				JArray a = (JArray)t;
				if (a.Count == 0) return EnumDataType.Unknown;
				JToken t2 = a[0];
				if (t2 is JValue) {
					object v = ((JValue)t2).Value;
					if (v is bool) return EnumDataType.BooleanValueArray;
					if (v is int) return EnumDataType.IntegerValueArray;
					if (v is long) return EnumDataType.LongValueArray;
					if (v is double) return EnumDataType.DoubleValueArray;
					if (v is string) return EnumDataType.StringValueArray;
					return EnumDataType.Unknown;
				} else
				if (t2 is JObject) {
					return EnumDataType.ObjectArray;
				} else
					return EnumDataType.Unknown;
			} else
			if (t is JObject) {
				return EnumDataType.Object;
			} else {
				return EnumDataType.Unknown;
			}
		}

		public static EnumDataType DetectDataTypeFromValue(object value)
		{
			if (value == null) return EnumDataType.Unknown;

			if (value is string) return EnumDataType.StringValue;
			if (value is string[]) return EnumDataType.StringValueArray;
			if (value is int) return EnumDataType.IntegerValue;
			if (value is int[]) return EnumDataType.IntegerValueArray;
			if (value is long) return EnumDataType.LongValue;
			if (value is long[]) return EnumDataType.LongValueArray;
			if (value is bool) return EnumDataType.BooleanValue;
			if (value is bool[]) return EnumDataType.BooleanValueArray;
			if (value is double) return EnumDataType.DoubleValue;
			if (value is double[]) return EnumDataType.DoubleValueArray;

			return EnumDataType.Unknown;
		}

		////////////////////////////////////////////////////////////////

		public static JArray ToJArray(string[] values)
		{
			if (values.Length == 0) return null;

			JArray a = new JArray();
			foreach (string v in values) {
				a.Add(new JValue(v));
			}
			return a;
		}

		public static JArray ToJArray(int[] values)
		{
			if (values.Length == 0) return null;

			JArray a = new JArray();
			foreach (int v in values) {
				a.Add(new JValue(v));
			}
			return a;
		}

		public static JArray ToJArray(long[] values)
		{
			if (values.Length == 0) return null;

			JArray a = new JArray();
			foreach (long v in values) {
				a.Add(new JValue(v));
			}
			return a;
		}

		public static JArray ToJArray(double[] values)
		{
			if (values.Length == 0) return null;

			JArray a = new JArray();
			foreach (double v in values) {
				a.Add(new JValue(v));
			}
			return a;
		}

		public static JArray ToJArray(bool[] values)
		{
			if (values.Length == 0) return null;

			JArray a = new JArray();
			foreach (bool v in values) {
				a.Add(new JValue(v));
			}
			return a;
		}

		////////////////////////////////////////////////////////////////

		public static T[] ToArrayE<T>(JArray a)
		{
			T[] ret = new T[a.Count];
			for (int i = 0; i < ret.Length; i++) {
				JValue v = (JValue)(a[i]);
				ret[i] = (T)(v.Value);
			}
			return ret;
		}

		////////////////////////////////////////////////////////////////

		public static string[] StrSetToArray(HashSet<string> set)
		{
			string[] array = set.ToArray();
			Array.Sort(array);
			return array;
		}

		////////////////////////////////////////////////////////////////

		/*
		public static bool IsEqual(JToken oldElement, JValue newValue)
		{
			if (oldElement == null) {
				if (newValue == null) {
					return true;
				} else {
					return false;
				}
			} else {
				if (newValue == null) {
					return false;
				} else {
					if (oldElement is JValue) {
						JValue oldValueV = (JValue)oldElement;
						object oldV = oldValueV.Value;
						object newV = newValue.Value;

						if (oldV == null) {
							if (newV == null) {
								return true;
							} else {
								return false;
							}
						} else {
							if (newV == null) {
								return false;
							} else {

								return oldV.Equals(newV);

							}
						}

					} else {
						return false;
					}
				}
			}
		}
		*/

		public static bool IsEqual(JToken oldElement, JToken newElement)
		{
			if (oldElement == null) {
				if (newElement == null) {
					return true;
				} else {
					return false;
				}
			} else {
				if (newElement == null) {
					return false;
				} else {
					return JToken.DeepEquals(oldElement, newElement);
				}
			}
		}

		public static JToken ValueOrValueArrayToJToken(object value)
		{
			EnumDataType dt = DetectDataTypeFromValue(value);
			switch (dt) {
				case JSONValueConverter.EnumDataType.Object:
				case JSONValueConverter.EnumDataType.ObjectArray:
					throw new Exception("Cannot handle objects!");
				case JSONValueConverter.EnumDataType.BooleanValue:
					return new JValue((bool)value);
				case JSONValueConverter.EnumDataType.BooleanValueArray:
					return JSONValueConverter.ToJArray((bool[])value);
				case JSONValueConverter.EnumDataType.DoubleValue:
					return new JValue((double)value);
				case JSONValueConverter.EnumDataType.DoubleValueArray:
					return JSONValueConverter.ToJArray((double[])value);
				case JSONValueConverter.EnumDataType.IntegerValue:
					return new JValue((int)value);
				case JSONValueConverter.EnumDataType.IntegerValueArray:
					return JSONValueConverter.ToJArray((int[])value);
				case JSONValueConverter.EnumDataType.LongValue:
					return new JValue((long)value);
				case JSONValueConverter.EnumDataType.LongValueArray:
					return JSONValueConverter.ToJArray((long[])value);
				case JSONValueConverter.EnumDataType.StringValue:
					return new JValue((string)value);
				case JSONValueConverter.EnumDataType.StringValueArray:
					return JSONValueConverter.ToJArray((string[])value);
				case JSONValueConverter.EnumDataType.Unknown:
					throw new Exception("Cannot handle data types of this kind: " + value.GetType().Name);
				default:
					throw new Exception("Implementation error!");
			}
		}

	}

}
