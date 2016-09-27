﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace TF2Net.Data
{
	[DebuggerDisplay("SendTable {NetTableName}, {Properties.Count} SendProps")]
	public class SendTable
	{
		public SendTable()
		{
			m_FlattenedProps = new Lazy<ImmutableArray<SendProp>>(
				() => ImmutableArray.Create(SetupFlatPropertyArray().ToArray()));
		}

		/// <summary>
		/// The name matched between client and server.
		/// </summary>
		public string NetTableName { get; set; }

		public IList<SendProp> Properties { get; set; } = new List<SendProp>();

		private Lazy<ImmutableArray<SendProp>> m_FlattenedProps;
		public ImmutableArray<SendProp> FlattenedProps { get { return m_FlattenedProps.Value; } }

		public bool Unknown1 { get; set; }
		
		IEnumerable<FlattenedProp> AllProperties { get { return Flatten(Excludes); } }
		
		IEnumerable<SendProp> Excludes
		{
			get
			{
				foreach (SendProp prop in Properties)
				{
					if (prop.Flags.HasFlag(SendPropFlags.Exclude))
						yield return prop;
					else if (prop.Type == SendPropType.Datatable)
					{
						foreach (SendProp childExclude in prop.Table.Excludes)
							yield return childExclude;
					}
				}
			}
		}

#if DEBUG
		IEnumerable<FlattenedProp> DataTables
		{
			get
			{
				var datatablesFirst = Properties.OrderByDescending(p => p.Type,
				   Comparer<SendPropType>.Create((p1, p2) =>
				   {
					   bool isDT1 = p1 == SendPropType.Datatable;
					   bool isDT2 = p2 == SendPropType.Datatable;

					   if (isDT1 == isDT2)
						   return 0;

					   if (isDT1)
						   return 1;
					   else if (isDT2)
						   return -1;

					   throw new InvalidOperationException();
				   }));

				foreach (SendProp prop in datatablesFirst)
				{
					if (prop.Type == SendPropType.Datatable)
					{
						if (!prop.Table.Properties.Any(p => p.Type == SendPropType.Datatable))
						{
							FlattenedProp flatProp = new FlattenedProp();
							flatProp.Property = prop;
							flatProp.FullName = flatProp.Property.Name.Insert(0, NetTableName + '.');
							yield return flatProp;
						}

						foreach (FlattenedProp childProp in prop.Table.DataTables)
						{
							childProp.FullName = childProp.FullName.Insert(0, NetTableName + '.');
							yield return childProp;
						}
					}
				}
			}
		}
#endif

		IEnumerable<FlattenedProp> Flatten(IEnumerable<SendProp> excludes)
		{
			var datatablesFirst = Properties.OrderByDescending(p => p.Type,
				Comparer<SendPropType>.Create((p1, p2) =>
				{
					bool isDT1 = p1 == SendPropType.Datatable;
					bool isDT2 = p2 == SendPropType.Datatable;

					if (isDT1 == isDT2)
						return 0;

					if (isDT1)
						return 1;
					else if (isDT2)
						return -1;

					throw new InvalidOperationException();
				}));

			foreach (SendProp prop in datatablesFirst)
			{
				if (excludes.Any(e => e.Name == prop.Name && e.ExcludeName == prop.Parent.NetTableName))
					continue;

				if (excludes.Contains(prop))
					continue;

				Debug.Assert(!prop.Flags.HasFlag(SendPropFlags.Exclude));

				if (prop.Type == SendPropType.Datatable)
				{
					foreach (FlattenedProp childProp in prop.Table.Flatten(excludes))
					{
						childProp.FullName = childProp.FullName.Insert(0, NetTableName + '.');
						yield return childProp;
					}
				}
				else
				{
					FlattenedProp flatProp = new FlattenedProp();
					flatProp.Property = prop;
					flatProp.FullName = flatProp.Property.Name.Insert(0, NetTableName + '.');
					yield return flatProp;
				}
			}
		}

		public IEnumerable<FlattenedProp> SortedProperties
		{
			get
			{
				var allProperties = AllProperties.ToList();

				//var allChangesOften = allProperties.Where(p => p.Property.Flags.HasFlag(SendPropFlags.ChangesOften));

				if (allProperties.Count < 2)
					return allProperties;

				int start = 0;
				for (int i = start + 1; i < allProperties.Count; i++)
				{
					if (allProperties[start].Property.Flags.HasFlag(SendPropFlags.ChangesOften))
					{
						start++;
						continue;
					}

					if (allProperties[i].Property.Flags.HasFlag(SendPropFlags.ChangesOften))
					{
						var temp = allProperties[i];
						allProperties[i] = allProperties[start];
						allProperties[start] = temp;

						start++;
						continue;
					}
				}

				return allProperties;
			}
		}

		List<SendProp> SetupFlatPropertyArray()
		{
			var excludes = Excludes;
			
			List<SendProp> props = new List<SendProp>();

			SendTable_BuildHierarchy(excludes, props);

			SendTable_SortByPriority(props);

			return props;
		}

		void SendTable_BuildHierarchy(IEnumerable<SendProp> excludes, List<SendProp> allProperties)
		{
			List<SendProp> localProperties = new List<SendProp>();

			SendTable_BuildHierarchy_IterateProps(excludes, localProperties, allProperties);

			allProperties.AddRange(localProperties);
		}

		IEnumerable<SendProp> TestSortedProps
		{
			get { return SetupFlatPropertyArray(); }
		}

		void SendTable_SortByPriority(List<SendProp> props)
		{
			int start = 0;
			for (int i = start; i < props.Count; i++)
			{
				if (props[i].Flags.HasFlag(SendPropFlags.ChangesOften))
				{
					if (i != start)
					{
						var temp = props[i];
						props[i] = props[start];
						props[start] = temp;
					}

					start++;
					continue;
				}
			}
		}

		void SendTable_BuildHierarchy_IterateProps(IEnumerable<SendProp> excludes, List<SendProp> localProperties, List<SendProp> childDTProperties)
		{
			foreach (var prop in Properties)
			{
				if (prop.Flags.HasFlag(SendPropFlags.Exclude) || excludes.Contains(prop))
				{
					continue;
				}
				
				if (excludes.Any(e => e.Name == prop.Name && e.ExcludeName == prop.Parent.NetTableName))
					continue;

				if (prop.Type == SendPropType.Datatable)
				{
					if (prop.Flags.HasFlag(SendPropFlags.Collapsible))
						prop.Table.SendTable_BuildHierarchy_IterateProps(excludes, localProperties, childDTProperties);
					else
					{
						prop.Table.SendTable_BuildHierarchy(excludes, childDTProperties);
					}
				}
				else
				{
					localProperties.Add(prop);
				}
			}
		}
	}
}
