﻿using System;
using System.Runtime.CompilerServices;

namespace BitSet
{
	public static partial class BitReader
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte ReadUInt3(byte[] buffer, ref ulong startBit)
		{
			var retVal = ReadUInt3(buffer, startBit);
			startBit += 3;
			return retVal;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte ReadUInt3(byte[] buffer, ulong startBit)
		{
			return ReadUInt3(buffer, (int)(startBit / 8), (byte)(startBit % 8));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte ReadUInt3(byte[] buffer, int startByte = 0)
		{
			throw new NotImplementedException();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte ReadUInt3(byte[] buffer, int startByte, byte bitOffset)
		{
			throw new NotImplementedException();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static byte ReadUInt3(byte* buffer, int startByte = 0)
		{
			throw new NotImplementedException();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static byte ReadUInt3(byte* buffer, int startByte, byte bitOffset)
		{
			throw new NotImplementedException();
		}
	}
	public static partial class BitWriter
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteUInt3(byte value, byte[] buffer, int startByte = 0)
		{
			throw new NotImplementedException();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void WriteUInt3(byte value, byte[] buffer, int startByte, byte bitOffset)
		{
			throw new NotImplementedException();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteUInt3(byte value, byte* buffer, int startByte = 0)
		{
			throw new NotImplementedException();
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static void WriteUInt3(byte value, byte* buffer, int startByte, byte bitOffset)
		{
			throw new NotImplementedException();
		}
	}
}
