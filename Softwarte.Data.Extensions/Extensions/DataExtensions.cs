/*
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Hermione.Data.Model;


namespace Hermione.Data
{
	public static class Extensions
	{
		#region Document

		public static void Create(this Document entity)
		{
			using(var db = new DataLayer<ModelContext>())
			{
				db.CreateEntity(entity);
			}
		}

		public static void Save(this Document entity)
		{
			using(var db = new DataLayer<ModelContext>())
			{
				db.CreateEntity(entity);
			}
		}

		#endregion

	}
}


