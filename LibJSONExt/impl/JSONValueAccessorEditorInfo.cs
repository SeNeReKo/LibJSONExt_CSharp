using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace LibJSONExt.impl
{

	public class JSONValueAccessorEditorInfo: AbstractJAccessor
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private readonly JObject obj;
		private JSONValueHistory history;
		private bool bCanHaveHistory;
		private bool bAutoStoreOldValuesInHistory;
		private IEditorInfo editor;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public JSONValueAccessorEditorInfo(JObject obj, string path, bool bCanHaveHistory, bool bAutoStoreOldValuesInHistory, IEditorInfo editor)
			: base(path)
		{
			this.bAutoStoreOldValuesInHistory = bCanHaveHistory && bAutoStoreOldValuesInHistory;
			this.editor = editor;
			this.obj = obj;
			this.bCanHaveHistory = bCanHaveHistory;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public override JSONValueHistory History
		{
			get {
				if (history == null) {
					if (bCanHaveHistory) {
						history = new JSONValueHistory(obj, path + "/history");
					}
				}
				return history;
			}
		}

		public override string UserID
		{
			get {
				object v = __Get(obj, "userID");
				if (v is string) {
					return (string)v;
				} else {
					return null;
				}
			}
			protected set {
				if (__Set(obj, "userID", value)) IsChanged = true;
			}
		}

		public override string SourceName
		{
			get {
				object v = __Get(obj, "sourceName");
				if (v is string) {
					return (string)v;
				} else {
					return null;
				}
			}
			protected set {
				if (__Set(obj, "sourceName", value)) IsChanged = true;
			}
		}

		public override EnumValueSource Source
		{
			get {
				object v = __Get(obj, "source");
				if (v is string) {
					string s = (string)v;
					EnumValueSource q = (EnumValueSource)(Enum.Parse(typeof(EnumValueSource), s, true));
					return q;
				} else {
					return EnumValueSource.Unknown;
				}
			}
			protected set {
				if (__Set(obj, "source", value.ToString().ToLower())) IsChanged = true;
			}
		}

		public override EnumValueQuality Quality
		{
			get {
				object v = __Get(obj, "quality");
				if (v is int) {
					int n = (int)v;

					foreach (var vv in Enum.GetValues(typeof(EnumValueQuality))) {
						if (n == (int)vv) {
							return (EnumValueQuality)vv;
						}
					}

					return EnumValueQuality.Unknown;
				} else
				if (v is string) {
					string s = (string)v;
					EnumValueQuality q = (EnumValueQuality)(Enum.Parse(typeof(EnumValueQuality), s, true));
					return q;
				} else {
					return EnumValueQuality.Unknown;
				}
			}
			protected set {
				if (__Set(obj, "quality", value.ToString().ToLower())) IsChanged = true;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		protected override object __GetValue()
		{
			return __Get(obj, "value");
		}

		protected override bool __SetValue(object value)
		{
			bool b = __Set(obj, "value", value);
			if (((value != null) || b) && (editor != null)) {
				Quality = editor.Quality;
				Source = editor.Source;
				SourceName = editor.SourceName;
				UserID = editor.UserID;
			}
			return b;
		}

		public override JObject CloneWithoutHistory()
		{
			JPath p = new JPath(path);
			JToken t = p.Get(obj);
			if (t == null) return null;
			if (t is JObject) {
				JObject ret = (JObject)(((JObject)t).DeepClone());
				ret.Remove("history");
				return ret;
			} else {
				return null;
			}
		}

	}

}
