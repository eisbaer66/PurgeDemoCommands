﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF2Net.Data
{
	[DebuggerDisplay("{ToString(),nq}")]
	public class Vector : ICloneable
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }

		public Vector() { }
		public Vector(double x, double y, double z = 0)
		{
			X = x;
			Y = y;
			Z = z;
		}
		public Vector(double[] xyz)
		{
			if (xyz == null)
				throw new ArgumentNullException(nameof(xyz));
			if (xyz.Length != 3)
				throw new ArgumentException("Array is not of length 3", nameof(xyz));

			X = xyz[0];
			Y = xyz[1];
			Z = xyz[2];
		}

		public double this[int i]
		{
			get
			{
				switch (i)
				{
					case 0:	return X;
					case 1:	return Y;
					case 2:	return Z;
				}

				throw new ArgumentOutOfRangeException(nameof(i));
			}
			set
			{
				switch (i)
				{
					case 0:	X = value; return;
					case 1: Y = value; return;
					case 2: Z = value; return;
				}

				throw new ArgumentOutOfRangeException(nameof(i));
			}
		}

		public override string ToString()
		{
			return string.Format("{0}: ({1} {2} {3})", nameof(Vector), X, Y, Z);
		}

		public Vector Clone()
		{
			return (Vector)MemberwiseClone();
		}
		object ICloneable.Clone() { return Clone(); }
	}
}
