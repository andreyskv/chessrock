using Raven.Client.Embedded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Common
{
    public interface IRavenRepository
    {
        //void SaveEntity(object entity);
        //T LoadEntity<T>(string path);
        //IQueryable<T> Query<T>() where T : class;
        EmbeddableDocumentStore Store {get;}
    }
}