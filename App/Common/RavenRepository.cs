using Raven.Client.Embedded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Common
{
 

    public class RavenRepository : IRavenRepository
    {
        private EmbeddableDocumentStore _documentStore;

        public RavenRepository()
        {
            _documentStore = new EmbeddableDocumentStore() { ConnectionStringName = "RavenDB" };
            _documentStore.Initialize();
        }

        public EmbeddableDocumentStore Store 
        { 
            get { return _documentStore; } 
        }


        //public IQueryable<T> Query<T>() where T : class
        //{
        //    IQueryable<T> result = null;
        //    using (var session = _documentStore.OpenSession())
        //    {
        //        result = session.Query<T>();
        //    }
        //    return result;
        //}

        //public void SaveNewEntity(object entity)
        //{
        //    if (entity != null)
        //    {
        //        using (var session = _documentStore.OpenSession())
        //        {
        //            session.Store(entity);
        //            session.SaveChanges();
        //        }
        //    }            
        //}

  
        //public T LoadEntity<T>(string id) // "entities/{Id}"
        //{
        //    T entity = default(T);
        //    using (var session = _documentStore.OpenSession())
        //    {
        //        entity = session.Load<T>(id);
        //    }
        //    return entity;
        //}

    }
}