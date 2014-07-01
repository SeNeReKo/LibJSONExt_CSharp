using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace LibJSONExt
{

	/// <summary>
	/// This class represents a path within a JSON object hierarchy.
	/// </summary>
	public class JPath
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private string[] __path;

		private bool bCanHaveHistory;
		private bool bAutoStoreOldValuesInHistory;
		private IEditorInfo editor;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="path">A valid path.</param>
		public JPath(string path)
			: this(path, false, false, null)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="path">A valid path.</param>
		public JPath(string path, bool bCanHaveHistory, bool bAutoStoreOldValuesInHistory, IEditorInfo editor)
		{
			if (!path.StartsWith("/")) throw new Exception("Invalid path!");
			if ((path.Length > 1) &&  path.EndsWith("/")) throw new Exception("Invalid path!");

			this.bCanHaveHistory = bCanHaveHistory;
			this.bAutoStoreOldValuesInHistory = bAutoStoreOldValuesInHistory;
			this.editor = editor;
			this.Path = path;

			if (path.Equals("/")) {
				__path = new string[0];
			} else {
				path = path.Substring(1);

				string[] pathElements = path.Split('/');
				__path = new string[pathElements.Length];
				for (int i = 0; i < pathElements.Length; i++) {
					string s = pathElements[i];
					if ((s == null) || (s.Length == 0) || (s.Trim().Length != s.Length)) {
						throw new Exception("Invalid path!");
					}
					__path[i] = s;
				}
			}
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		/// <summary>
		/// The path as specified during construction.
		/// </summary>
		public string Path
		{
			get;
			private set;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		/*
		public AbstractJAccessor Access(JObject obj,
			bool bCanHaveHistory, bool bAutoStoreOldValuesInHistory, IEditorInfo editor)
		{
			if (editor == null) return new impl.JSONValueAccessorSimple(obj, Path);
			else return new impl.JSONValueAccessorEditorInfo(obj, Path, bCanHaveHistory, bAutoStoreOldValuesInHistory, editor);
		}
		*/

		public static AbstractJAccessor CreateAccessorSimple(JObject obj, string path)
		{
			if (!JPath.IsValid(path)) throw new Exception("Path is not valid: " + path);
			return new impl.JSONValueAccessorSimple(obj, path);
		}

		public static AbstractJAccessor CreateAccessorWithEditInfo(JObject obj, string path,
			bool bCanHaveHistory, bool bAutoStoreOldValuesInHistory, IEditorInfo editor)
		{
			if (!JPath.IsValid(path)) throw new Exception("Path is not valid: " + path);
			return new impl.JSONValueAccessorEditorInfo(obj, path, bCanHaveHistory, bAutoStoreOldValuesInHistory, editor);
		}

		/// <summary>
		/// Get a JSON element from the specified object.
		/// </summary>
		/// <param name="obj">The object the data should be retrieved from</param>
		/// <returns></returns>
		public JToken Get(JObject obj)
		{
			for (int i = 0; i < __path.Length; i++) {
				JProperty p = obj.Property(__path[i]);
				if (p == null) return null;
				if (p.Value == null) return null;
				if (i == __path.Length - 1) return p.Value;
				if (p.Value is JObject) {
					obj = (JObject)(p.Value);
				} else {
					return null;
				}
			}
			return obj;
		}

		/// <summary>
		/// Set a JSON element at the specified object. The existing object is replaced
		/// in this process.
		/// </summary>
		/// <param name="obj">The object the changes should be applied to</param>
		/// <param name="value">A non-null value</param>
		public void Set(JObject obj, JToken value)
		{
			if (value == null) throw new Exception("value == null");

			for (int i = 0; i < __path.Length - 1; i++) {
				JProperty p = obj.Property(__path[i]);
				if (p == null) {
					JObject newObj = new JObject();
					obj.Add(__path[i], newObj);
					obj = newObj;
				} else
				if (p.Value == null) {
					JObject newObj = new JObject();
					p.Value = newObj;
					obj = newObj;
				} else
				if (p.Value is JObject) {
					obj = (JObject)(p.Value);
				} else {
					throw new Exception("A value other than JObject is set within the path!");
				}
			}

			{
				JProperty p = obj.Property(__path[__path.Length - 1]);
				if (p == null) {
					obj.Add(__path[__path.Length - 1], value);
				} else {
					p.Value = value;
				}
			}
		}


		/// <summary>
		/// Remove data from the specified object that is stored at the path denoted by this JPath object.
		/// </summary>
		/// <param name="obj">The object the changes should be applied to</param>
		public void Remove(JObject obj)
		{
			for (int i = 0; i < __path.Length - 1; i++) {
				JProperty p = obj.Property(__path[i]);
				if (p == null) {
					return;
				} else
				if (p.Value == null) {
					return;
				} else
				if (p.Value is JObject) {
					obj = (JObject)(p.Value);
				} else {
					throw new Exception("A value other than JObject is set within the path!");
				}
			}

			obj.Remove(__path[__path.Length - 1]);
		}

		public override string ToString()
		{
			return Path;
		}

		/// <summary>
		/// Verify that the specified path is valid. A path is considered to be valid if
		/// it starts with a slash ("/"), does NOT end with a slash, and contains words
		/// between slashes that do not start or end with spaces.
		/// </summary>
		/// <param name="path">The path to check</param>
		/// <returns>Returns <code>true</code> or <code>false</code>.</returns>
		public static bool IsValid(string path)
		{
			if (!path.StartsWith("/")) return false;
			if ((path.Length > 1) && path.EndsWith("/")) return false;

			if (path.Equals("/")) return true;

			string[] pathElements = path.Split('/');
			for (int i = 1; i < pathElements.Length; i++) {
				string s = pathElements[i];
				if ((s == null) || (s.Length == 0) || (s.Trim().Length != s.Length)) {
					return false;
				}
			}

			return true;
		}

	}

}
