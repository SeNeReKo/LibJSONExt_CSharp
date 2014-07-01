using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace LibJSONExt.impl
{

	internal class JSONValueAccessorSimple : AbstractJAccessor
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private readonly JObject obj;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public JSONValueAccessorSimple(JObject obj, string path)
			: base(path)
		{
			this.obj = obj;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public override JSONValueHistory History
		{
			get {
				return null;
			}
		}

		public override EnumValueQuality Quality
		{
			get {
				return EnumValueQuality.Unknown;
			}
			protected set {
			}
		}

		public override EnumValueSource Source
		{
			get {
				return EnumValueSource.Unknown;
			}
			protected set {
			}
		}

		public override string SourceName
		{
			get {
				return null;
			}
			protected set {
			}
		}

		public override string UserID
		{
			get {
				return null;
			}
			protected set {
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		protected override object __GetValue()
		{
			return __Get(obj, null);
		}

		protected override bool __SetValue(object value)
		{
			return __Set(obj, null, value);
		}

		public override JObject CloneWithoutHistory()
		{
			return null;
		}

	}

}
