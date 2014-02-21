/*
 * ­­© Copyright Hermione 2013.
 * Programmed by Rafa Hernández.
 * 
 * Modified:04/03/2013
 */

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Hermione.Data.Common;

namespace Hermione.Data
{
	public interface IDataLayer
	{
		ICollection<TEntity> GetAll<TEntity>(params string[] eagerLoadEntities) where TEntity : class ,new();

		ICollection<TEntity> GetAll<TEntity>(out long totalCount, params string[] eagerLoadEntities) where TEntity : class ,new();
		//with count.
		ICollection<TEntity> GetAll<TEntity>(int pageIndex,
											int pageSize,
											out long totalCount,
											params string[] eagerLoadEntities) where TEntity : class, new();
		//without count.
		ICollection<TEntity> GetAll<TEntity>(int pageIndex,
											int pageSize,
											params string[] eagerLoadEntities) where TEntity : class, new();
		//with count.
		long GetCountAll<TEntity>() where TEntity : class, new();
		ICollection<TEntity> GetByQuery<TEntity>(Expression<Func<TEntity, bool>> predicate,
																 int pageIndex,
																 int pageSize,
																 OrderClause order,
																 out long totalCount,
																 params string[] eagerLoadEntities) where TEntity : class, new();
		//without count.
		ICollection<TEntity> GetByQuery<TEntity>(Expression<Func<TEntity, bool>> predicate,
																 int pageIndex,
																 int pageSize,
																 OrderClause order,
																 params string[] eagerLoadEntities) where TEntity : class, new();
		//without count.
		ICollection<TEntity> GetByQuery<TEntity>(Expression<Func<TEntity, bool>> predicate,
																 OrderClause order,
																 params string[] eagerLoadEntities) where TEntity : class, new();
		//with count.
		ICollection<TEntity> GetByQuery<TEntity>(Expression<Func<TEntity, bool>> predicate,
																 out long totalCount,
																 OrderClause order,
																 params string[] eagerLoadEntities) where TEntity : class, new();
		long GetCountByQuery<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, new();

		TEntity GetEntityByQuery<TEntity>(Expression<Func<TEntity, bool>> predicate,
													params string[] eagerLoadEntities) where TEntity : class, new();

		bool SaveEntity<TEntity>(TEntity entity) where TEntity : class, new();

		TEntity CreateEntity<TEntity>(TEntity entity) where TEntity : class, new();
	}
}