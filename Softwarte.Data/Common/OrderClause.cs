/************************************************
 *	Helper classe to build predicates.			
 *	Programmed by: Rafael Hernández
 *	Revision Date: 4/03/2014
 *	Version: 1.0											
 * **********************************************/

namespace Softwarte.Data.Common
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Xml;
	/// <summary>
	/// Collection of order clauses.
	/// </summary>
	public class OrderCollection
	{
		private List<OrderClause> OrderClauses { get; set; }
	}
	/// <summary>
	/// Order clause to include in linq queries.
	/// </summary>
	public class OrderClause
	{
		public OrderDirectionEnum Direction { get; set; }
		public string FieldName { get; set; }
	}
	/// <summary>
	/// Order direction enum.
	/// </summary>
	public enum OrderDirectionEnum
	{
		Ascending, Descending
	}
}


