/*
*
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Xml;


namespace Hermione.Data
{
	public static class Extensions
	{
		#region Document

		public static void Create<T,K>(this T entity, K context)
            where T: class, new()
            where K: DbContext, new()
		{
			using(var db = new DataLayer<K>())
			{
				db.CreateEntity(entity);
			}
		}

		public static void Save<T,K>(this T entity, K context)
            where T: class, new()
            where K: DbContext, new()
		{
			using(var db = new DataLayer<K>())
			{
				db.SaveEntity(entity);
			}
		}

		#endregion

	}
}


