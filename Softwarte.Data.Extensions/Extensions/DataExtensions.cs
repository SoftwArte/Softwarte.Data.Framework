/************************************************
 *	Generic data extensions.			
 *	Programmed by: Rafael Hernández
 *	Revision Date: 4/03/2014
 *	Version: 1.0											
 * **********************************************/

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

	}
}


