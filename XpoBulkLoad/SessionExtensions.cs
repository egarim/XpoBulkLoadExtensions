using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.Xpo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace XpoBulkLoad
{
    public static class SessionExtensions
    {
        public static XPCollection<T> GetCollection<T>(this Session session, Expression<Func<T, bool>> Filter)
        {
            CriteriaOperator condition = CriteriaOperator.FromLambda<T>(Filter);
            //HACK if you pass the criteria in the constructor the collection is loaded from the database immediately
            //return new XPCollection<T>(PersistentCriteriaEvaluationBehavior.InTransaction, session, condition,);
            XPCollection<T> ts = new XPCollection<T>(session);
            ts.Filter = condition;
            return ts;
        }
        public static IXPBulkLoadableCollection[] BulkLoadCollections<A, B>(this Session session, Expression<Func<A, bool>> FilterA, Expression<Func<B, bool>> FilterB)
        {
            List<IXPBulkLoadableCollection> collections = new List<IXPBulkLoadableCollection>();
            collections.Add(session.GetCollection(FilterA));
            collections.Add(session.GetCollection(FilterB));
            session.BulkLoad(collections.ToArray());
            return collections.ToArray();
        }

        public static IXPBulkLoadableCollection[] GetCollections<A,B>(this Session session, Expression<Func<A, bool>> FilterA, Expression<Func<B, bool>> FilterB)
        {
            List<IXPBulkLoadableCollection> collections = new List<IXPBulkLoadableCollection>();
            collections.Add(session.GetCollection(FilterA));
            collections.Add(session.GetCollection(FilterB));
            return collections.ToArray();
        }
          public static IXPBulkLoadableCollection[] GetCollections<A,B,C>(this Session session, Expression<Func<A, bool>> FilterA, Expression<Func<B, bool>> FilterB,Expression<Func<B, bool>> FilterC)
        {
            List<IXPBulkLoadableCollection> collections = new List<IXPBulkLoadableCollection>();
            collections.Add(session.GetCollection(FilterA));
            collections.Add(session.GetCollection(FilterB));
            collections.Add(session.GetCollection(FilterC));
            return collections.ToArray();
        }
    }
}
