using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpoBulkLoad
{
    public class DataStoreWrapper : IDataStore
    {
        IDataStore RealDataStore;

        public DataStoreWrapper(IDataStore realDataStore)
        {
            RealDataStore = realDataStore;
            DatabaseTrips = 0;
        }
        public AutoCreateOption AutoCreateOption
        {
            get
            {
               return RealDataStore.AutoCreateOption; 
            }
        }
        int DatabaseTrips;
        public void ResetDatabaseTripCounter()
        {
            Debug.WriteLine($"Count is back to zero");
            DatabaseTrips = 0;
        }
        public int GetTotalDatabaseTrips()
        {
            Debug.WriteLine($"total database trips:{DatabaseTrips}");
            return DatabaseTrips;
        }
        public ModificationResult ModifyData(params ModificationStatement[] dmlStatements)
        {
            return RealDataStore.ModifyData(dmlStatements);
        }

        public SelectedData SelectData(params SelectStatement[] selects)
        {
            SelectedData selectedData = RealDataStore.SelectData(selects);
            DatabaseTrips ++;
            Debug.WriteLine($"total database trips:{DatabaseTrips}");
            return selectedData;
        }

        public UpdateSchemaResult UpdateSchema(bool doNotCreateIfFirstTableNotExist, params DBTable[] tables)
        {
            return RealDataStore.UpdateSchema(doNotCreateIfFirstTableNotExist, tables);
        }
    }
}
