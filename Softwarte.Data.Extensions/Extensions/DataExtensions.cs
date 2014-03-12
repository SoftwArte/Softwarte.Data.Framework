/************************************************
 *	Generic data extensions.			
 *	Programmed by: Rafael Hernández
 *	Revision Date: 4/03/2014
 *	Version: 1.0											
 * **********************************************/

namespace Softwarte.Data
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Data.Entity;
  using System.IO;
  using System.Linq;
  using System.Xml;
  /// <summary>
  /// Entity extensions. Can be used with any context.
  /// </summary>
  public static class Extensions
  {
    /// <summary>
    /// Create an entity. Use the data layer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="entity"></param>
    /// <param name="context"></param>
    public static void Create<T, K>(this T entity, K context)
      where T : class, new()
      where K : DbContext, new()
    {
      using (var db = new DataLayer<K>())
      {
        db.CreateEntity(entity);
      }
    }
    /// <summary>
    /// Save an entity. Use the data layer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="entity"></param>
    /// <param name="context"></param>
    public static void Save<T, K>(this T entity, K context)
      where T : class, new()
      where K : DbContext, new()
    {
      using (var db = new DataLayer<K>())
      {
        db.SaveEntity(entity);
      }
    }

  }
}


