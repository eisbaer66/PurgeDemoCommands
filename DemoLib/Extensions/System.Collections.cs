﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class DemoLib_Extensions
	{
		public static uint? FindNextSetBit(this BitArray source, uint startIndex = 0)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			for (uint i = startIndex; i < source.Length; i++)
			{
				if (source[(int)i])
					return i;
			}

			return null;
		}
	}
}
