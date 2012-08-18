using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Embedded;


namespace Team_board
{
    public class Repository
    {
        IDocumentStore store = null;

        public Repository()
        {
            this.store = new EmbeddableDocumentStore { RunInMemory = true };
            this.store.Initialize();
        }

        public void Save<T>(T document)
        {
            using (IDocumentSession session = this.store.OpenSession())
            {
                session.Store(document);
                session.SaveChanges();
            }
        }

        public T Get<T>(string id)
        {
            using (IDocumentSession session = this.store.OpenSession())
            {
                return session.Load<T>(id);
            }
        }
    }

}
