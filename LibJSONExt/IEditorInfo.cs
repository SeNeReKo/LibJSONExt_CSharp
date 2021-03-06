﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibJSONExt
{

	/// <summary>
	/// Information about someone who edits the data
	/// </summary>
	public interface IEditorInfo
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

		string UserID
		{
			get;
		}

		string SourceName
		{
			get;
		}

		EnumValueSource Source
		{
			get;
		}

		EnumValueQuality Quality
		{
			get;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

	}

}
