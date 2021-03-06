﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using BitSet;
using TF2Net.Data;

namespace TF2Net.NetMessages
{
	[DebuggerDisplay("{Description, nq}")]
	public class NetStringCmdMessage : INetMessage
	{
		public string Command { get; set; }

		public string Description
		{
			get
			{
				return string.Format("net_StringCmd: \"{0}\"", Command);
			}
		}

		public void ReadMsg(BitStream stream)
		{
			Command = stream.ReadCString();
		}

		public void ApplyWorldState(WorldState ws)
		{
			ws.Listeners.ServerConCommand.Invoke(ws, Command);
		}
	}
}
