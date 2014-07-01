using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace LibJSONExt
{

	public abstract class AbstractJAccessor
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		protected readonly string path;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public AbstractJAccessor(string path)
		{
			if (!JPath.IsValid(path)) throw new Exception("Path is invalid: " + path);
			this.path = path;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public bool IsChanged
		{
			get;
			set;
		}

		public string Path
		{
			get {
				return path;
			}
		}

		/// <summary>
		/// Get or set a value. A value must either be a primitive value, an array of primitive values
		/// or a set of string set.
		/// Please be aware that on write a string set will always get sorted.
		/// </summary>
		public object Value
		{
			get {
				return __GetValue();
			}
			set {
				if (__SetValue(value)) IsChanged = true;
			}
		}

		public string ValueAsStr
		{
			get {
				object v = Value;
				if (v is string) {
					return (string)v;
				} else {
					return null;
				}
			}
			set {
				if (__SetValue(value)) IsChanged = true;
			}
		}

		public string[] ValueAsStrArray
		{
			get {
				object v = Value;
				if (v is string[]) {
					return (string[])v;
				} else {
					return null;
				}
			}
			set {
				if (__SetValue(value)) IsChanged = true;
			}
		}

		public HashSet<string> ValueAsStrSet
		{
			get {
				object v = Value;
				if (v is string[]) {
					HashSet<string> set = new HashSet<string>((string[])v);
					return set;
				} else {
					return null;
				}
			}
			set {
				if (value == null) {
					if (__SetValue(null)) IsChanged = true;
				} else {
					string[] array = value.ToArray();
					Array.Sort(array);
					if (__SetValue(array)) IsChanged = true;
				}
			}
		}

		public int ValueAsInt
		{
			get {
				object v = Value;
				if (v is int) {
					return (int)v;
				} else {
					return -1;
				}
			}
			set {
				if (__SetValue(value)) IsChanged = true;
			}
		}

		public int[] ValueAsIntArray
		{
			get {
				object v = Value;
				if (v is int[]) {
					return (int[])v;
				} else {
					return null;
				}
			}
			set {
				if (__SetValue(value)) IsChanged = true;
			}
		}

		public double ValueAsDouble
		{
			get {
				object v = Value;
				if (v is double) {
					return (double)v;
				} else {
					return -1;
				}
			}
			set {
				if (__SetValue(value)) IsChanged = true;
			}
		}

		public double[] ValueAsDoubleArray
		{
			get {
				object v = Value;
				if (v is double[]) {
					return (double[])v;
				} else {
					return null;
				}
			}
			set {
				if (__SetValue(value)) IsChanged = true;
			}
		}

		public long ValueAsLong
		{
			get {
				object v = Value;
				if (v is long) {
					return (long)v;
				} else {
					return -1;
				}
			}
			set {
				if (__SetValue(value)) IsChanged = true;
			}
		}

		public long[] ValueAsLongArray
		{
			get {
				object v = Value;
				if (v is long[]) {
					return (long[])v;
				} else {
					return null;
				}
			}
			set {
				if (__SetValue(value)) IsChanged = true;
			}
		}

		public bool ValueAsBoolean
		{
			get {
				object v = Value;
				if (v is bool) {
					return (bool)v;
				} else {
					return false;
				}
			}
			set {
				if (__SetValue(value)) IsChanged = true;
			}
		}

		public bool[] ValueAsBooleanArray
		{
			get {
				object v = Value;
				if (v is bool[]) {
					return (bool[])v;
				} else {
					return null;
				}
			}
			set {
				if (__SetValue(value)) IsChanged = true;
			}
		}

		public bool HasValue
		{
			get {
				return Value != null;
			}
		}

		public abstract JSONValueHistory History
		{
			get;
		}

		public abstract string UserID
		{
			get;
			protected set;
		}

		public abstract string SourceName
		{
			get;
			protected set;
		}

		public abstract EnumValueSource Source
		{
			get;
			protected set;
		}

		public abstract EnumValueQuality Quality
		{
			get;
			protected set;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		/// <summary>
		/// If the value stored is a string, put the string into a set and return ist. If the value stored
		/// is a string array, convert the string array to a set and return ist. If the value is nether a string
		/// or a string list or is <code>null</code>, return <code>null</code>
		/// </summary>
		public HashSet<string> GetValueConvertedToStrSet()
		{
			object v = Value;
			if (v is string) {
				HashSet<string> ret = new HashSet<string>();
				ret.Add((string)v);
				return ret;
			} else
			if (v is string[]) {
				HashSet<string> ret = new HashSet<string>((string[])v);
				return ret;
			} else {
				return null;
			}
		}

		protected abstract bool __SetValue(object value);

		protected abstract object __GetValue();

		public void SetValueConvertedFromStrSet(HashSet<string> set)
		{
			if ((set == null) || (set.Count == 0)) {
				if (__SetValue(null)) IsChanged = true;
			} else
			if (set.Count == 1) {
				if (__SetValue(set.ToArray()[0])) IsChanged = true;
			} else {
				string[] array = set.ToArray();
				Array.Sort(array);
				if (__SetValue(array)) IsChanged = true;
			}
		}

		protected bool __Set(JObject obj, string key, object value)
		{
			JPath p;
			if (key == null) {
				p = new JPath(path);
			} else {
				p = new JPath(path + "/" + key);
			}

			JToken tOld = p.Get(obj);
			if (value == null) {
				if (tOld != null) {
					tOld.Remove();
					return true;
				} else {
					return false;
				}
			} else {
				JToken tNew = JSONValueConverter.ValueOrValueArrayToJToken(value);
				if (JSONValueConverter.IsEqual(tOld, tNew)) return false;
				p.Set(obj, tNew);
				return true;
			}
		}

		protected object __Get(JObject obj, string key)
		{
			JPath p;
			if (key == null) {
				p = new JPath(path);
			} else {
				p = new JPath(path + "/" + key);
			}

			JToken t = p.Get(obj);
			if (t == null) return null;
			if (t is JValue) {
				JValue v = (JValue)t;
				return v.Value;
			} else
			if (t is JArray) {
				JArray a = (JArray)t;
				switch (JSONValueConverter.DetectDataTypeFromToken(a)) {
					case JSONValueConverter.EnumDataType.BooleanValueArray:
						return JSONValueConverter.ToArrayE<bool>(a);
					case JSONValueConverter.EnumDataType.DoubleValueArray:
						return JSONValueConverter.ToArrayE<double>(a);
					case JSONValueConverter.EnumDataType.IntegerValueArray:
						return JSONValueConverter.ToArrayE<int>(a);
					case JSONValueConverter.EnumDataType.LongValueArray:
						return JSONValueConverter.ToArrayE<long>(a);
					case JSONValueConverter.EnumDataType.StringValueArray:
						return JSONValueConverter.ToArrayE<string>(a);
					default:
						return null;
				}
			} else {
				return null;
			}
		}

		protected object __GetE(JObject obj, string key)
		{
			JPath p;
			if (key == null) {
				p = new JPath(path);
			} else {
				p = new JPath(path + "/" + key);
			}

			JToken t = p.Get(obj);
			if (t == null) return null;
			if (t is JValue) {
				JValue v = (JValue)t;
				return v.Value;
			} else
			if (t is JArray) {
				JArray a = (JArray)t;
				JSONValueConverter.EnumDataType dt = JSONValueConverter.DetectDataTypeFromToken(a);
				switch (dt) {
					case JSONValueConverter.EnumDataType.BooleanValueArray:
						return JSONValueConverter.ToArrayE<bool>(a);
					case JSONValueConverter.EnumDataType.DoubleValueArray:
						return JSONValueConverter.ToArrayE<double>(a);
					case JSONValueConverter.EnumDataType.IntegerValueArray:
						return JSONValueConverter.ToArrayE<int>(a);
					case JSONValueConverter.EnumDataType.LongValueArray:
						return JSONValueConverter.ToArrayE<long>(a);
					case JSONValueConverter.EnumDataType.StringValueArray:
						return JSONValueConverter.ToArrayE<string>(a);
					default:
						throw new Exception("Unsuitable type at node " + p.Path + " : " + dt);
				}
			} else {
				throw new Exception("Unsuitable type at node " + p.Path + " : " + t.GetType().Name);
			}
		}

		public abstract JObject CloneWithoutHistory();

	}

}
