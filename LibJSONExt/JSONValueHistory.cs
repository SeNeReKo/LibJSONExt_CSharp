using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace LibJSONExt
{

	public class JSONValueHistory
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		JObject obj;
		string path;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public JSONValueHistory(JObject obj, string path)
		{
			this.obj = obj;
			this.path = path;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public void Backup(AbstractJAccessor currentValue)
		{
			if (currentValue == null) return;

			JPath p = new JPath(path);
			JToken t = p.Get(obj);
			JArray a;
			if (t == null) {
				a = new JArray();
				p.Set(obj, a);
			} else {
				if (t is JArray) {
					a = (JArray)t;
				} else {
					throw new Exception("Unexcepted token type for history!");
				}
			}

			JObject clone = currentValue.CloneWithoutHistory();
			if (clone != null) a.Add(clone);
		}

	}

}
