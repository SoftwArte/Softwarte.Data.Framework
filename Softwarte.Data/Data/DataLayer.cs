﻿/************************************************
 *	Generic data layer for EF code first.			
 *	Programmed by: Rafael Hernández
 *	Revision Date: 4/03/2014
 *	Version: 1.0											
 * **********************************************/

namespace Softwarte.Data
{
  using Softwarte.Data.Common;
  using System;
  using System.Collections.Generic;
  using System.Data.Entity;
  using System.Linq;
  using System.Linq.Expressions;
  public class DataLayer<TContext> : IDataLayer, IDisposable where TContext : DbContext, new()
  {
    private readonly TContext _context;

    #region Ctor
    public DataLayer()
    {
      //Database.SetInitializer<StoreModelContext>(new MigrateDatabaseTEntityoLatestVersion<StoreModelContext, Configuration>());
      _context = new TContext();
      /*
       * Lazy loading and dynamic proxies creation are disabled because are not compatible with serialization in asp.net web api.
       * Navigation properties are marked as non virtual to avoid lazy loading and dynamic proxies creation.
       * By requisites, there are a strong control over related entities loading.
       * But since version Beta 2 using DTEntityOs to expose entities in rest api, 
       */

      _context.Configuration.ValidateOnSaveEnabled = false;
      _context.Configuration.AutoDetectChangesEnabled = false;
      _context.Configuration.ProxyCreationEnabled = false;
      _context.Configuration.LazyLoadingEnabled = false;
    }
    #endregion

    #region IDataLayer Members

    /// <summary>
    /// Get all items of an entity without count.It can accept, optionally, the payload associated with the results..
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="eagerLoadEntities"></param>
    /// <returns></returns>
    public ICollection<TEntity> GetAll<TEntity>(params string[] eagerLoadEntities) where TEntity : class, new()
    {
      var query = _context.Set<TEntity>().OfType<TEntity>();
      //Payload.
      query = eagerLoadEntities.Aggregate(query, (current, related) => current.Include(related));
      //Results.
      return query.ToList();
    }

    /// <summary>
    /// Get all items of an entity with count.It can accept, optionally, the payload associated with the results..
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="totalCount"></param>
    /// <param name="eagerLoadEntities"></param>
    /// <returns></returns>
    public ICollection<TEntity> GetAll<TEntity>(out long totalCount, params string[] eagerLoadEntities) where TEntity : class, new()
    {
      var query = _context.Set<TEntity>().OfType<TEntity>();
      //Payload.
      query = eagerLoadEntities.Aggregate(query, (current, related) => current.Include(related));
      //Count.
      totalCount = GetCountAll<TEntity>(); //Get a count.
      //Results.
      return query.ToList();
    }

    /// <summary>
    /// Paginated query of all items of an entity with count.It can accept, optionally, the payload associated with the results..
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="totalCount"></param>
    /// <param name="eagerLoadEntities"></param>
    /// <returns></returns>
    public ICollection<TEntity> GetAll<TEntity>(int pageIndex, int pageSize, out long totalCount, params string[] eagerLoadEntities)
        where TEntity : class, new()
    {
      //Query.
      var query = _context.Set<TEntity>().OfType<TEntity>();
      //Payload.
      query = eagerLoadEntities.Aggregate(query, (current, related) => current.Include(related));
      //Filter
      query = query
               .Skip(pageIndex * pageSize)
               .Take(pageSize);
      //Count.
      totalCount = GetCountAll<TEntity>(); //Get count without pagination.
      //Results.
      return query.ToList();
    }

    /// <summary>
    /// Paginated get of all items of an entity without count.It can accept, optionally, the payload associated with the results..
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="eagerLoadEntities"></param>
    /// <returns></returns>
    public ICollection<TEntity> GetAll<TEntity>(int pageIndex, int pageSize, params string[] eagerLoadEntities)
      where TEntity : class, new()
    {
      //Query.
      var query = _context.Set<TEntity>().OfType<TEntity>();
      //Payload.
      query = eagerLoadEntities.Aggregate(query, (current, related) => current.Include(related));
      //Filter
      query = query
               .Skip(pageIndex * pageSize)
               .Take(pageSize);
      //Results.
      return query.ToList();
    }

    /// <summary>
    /// Get count over all items of an entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public long GetCountAll<TEntity>() where TEntity : class, new()
    {
      //Query.
      var results = _context.Set<TEntity>().OfType<TEntity>();
      //Result.
      return results.Count();
    }

    /// <summary>
    /// Paginated predicate based query of items of an entity with count and order. It can accept, optionally, the payload associated with the results..
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="predicate"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="order"></param>
    /// <param name="totalCount"></param>
    /// <param name="eagerLoadEntities"></param>
    /// <returns></returns>
    public ICollection<TEntity> GetByQuery<TEntity>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, OrderClause order, out long totalCount, params string[] eagerLoadEntities)
      where TEntity : class, new()
    {
      //Count.
      totalCount = GetCountByQuery<TEntity>(predicate); //Get count by predicate.
      //Query.
      var query = _context.Set<TEntity>().OfType<TEntity>();
      //Payload.
      query = eagerLoadEntities.Aggregate(query, (current, related) => current.Include(related));
      //Filter.
      query = query.Where(predicate);
      //Order.
      if (order.Direction == OrderDirectionEnum.Ascending)
      {
        query = query.OrderBy(c => order.FieldName);
      }
      else
      {
        query = query.OrderByDescending(c => order.FieldName);
      }
      //Pagination.
      query = query
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
      //Results.
      return query.ToList();
    }

    /// <summary>
    /// Paginated and predicate based query of items of an entity without count but with order. It can accept, optionally, the payload associated with the results..
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="predicate"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="order"></param>
    /// <param name="eagerLoadEntities"></param>
    /// <returns></returns>
    public ICollection<TEntity> GetByQuery<TEntity>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, OrderClause order, params string[] eagerLoadEntities)
      where TEntity : class, new()
    {
      //Query.
      var query = _context.Set<TEntity>().OfType<TEntity>();
      //Payload.
      query = eagerLoadEntities.Aggregate(query, (current, related) => current.Include(related));
      //Filter.
      query = query.Where(predicate);
      //Order.
      if (order.Direction == OrderDirectionEnum.Ascending)
      {
        query = query.OrderBy(c => order.FieldName);
      }
      else
      {
        query = query.OrderByDescending(c => order.FieldName);
      }
      //Pagination.
      query = query
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
      //Results.
      return query.ToList();
    }
    /// <summary>
    /// Predicate based query of all items of an entity with order. It can accept, optionally, the payload associated with the results..
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="predicate"></param>
    /// <param name="order"></param>
    /// <param name="eagerLoadEntities"></param>
    /// <returns></returns>
    public ICollection<TEntity> GetByQuery<TEntity>(Expression<Func<TEntity, bool>> predicate, OrderClause order, params string[] eagerLoadEntities) where TEntity : class, new()
    {
      throw new NotImplementedException();
    }
    /// <summary>
    /// Predicate based query with order and count. It can accept, optionally, the payload associated with the results.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="predicate"></param>
    /// <param name="totalCount"></param>
    /// <param name="order"></param>
    /// <param name="eagerLoadEntities"></param>
    /// <returns></returns>
    public ICollection<TEntity> GetByQuery<TEntity>(Expression<Func<TEntity, bool>> predicate, out long totalCount, OrderClause order, params string[] eagerLoadEntities) where TEntity : class, new()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Get a items count of an entity with a predicate based query.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public long GetCountByQuery<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class, new()
    {
      //Query.
      var results = _context.Set<TEntity>().OfType<TEntity>();
      //Filter.
      results = results.Where(predicate);
      //Result.
      return results.Count();
    }

    /// <summary>
    /// Return an entity using a predicate based query.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="predicate"></param>
    /// <param name="eagerLoadEntities"></param>
    /// <returns></returns>
    public TEntity GetEntityByQuery<TEntity>(Expression<Func<TEntity, bool>> predicate,
                                params string[] eagerLoadEntities) where TEntity : class, new()
    {
      //Query.
      var query = _context.Set<TEntity>().OfType<TEntity>();
      //Payload.
      query = eagerLoadEntities.Aggregate(query, (current, related) => current.Include(related));
      //Filter
      query = query.Where(predicate);
      //Result, devuelve nulo si no existe. Otra opción es generar exception mediante Single o First.
      return query.FirstOrDefault();
    }

    /// <summary>
    /// Generic method to save an entity. Entity is saved although hasn't changes.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool SaveEntity<TEntity>(TEntity entity)
      where TEntity : class, new()
    {
      _context.Entry<TEntity>(entity).State = EntityState.Modified;
      var result = _context.SaveChanges(); //If exits change result should be greater than 0.
      return true;
    }

    /// <summary>
    /// Generic method to create an entity with persistence.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public TEntity CreateEntity<TEntity>(TEntity entity)
      where TEntity : class, new()
    {
      _context.Set<TEntity>().Add(entity);
      _context.SaveChanges();
      return entity;
    }
    #endregion

    #region IDisposable Members
    /// <summary>
    /// Dispose resources and context.
    /// </summary>
    public void Dispose()
    {
      _context.Dispose();
    }

    #endregion
  }
}


