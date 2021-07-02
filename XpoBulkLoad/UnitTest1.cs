using DevExpress.Xpo;
using DevExpress.Xpo.Helpers;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace XpoBulkLoad
{
    public class Tests
    {
        IDataLayer dl;
        DataStoreWrapper dataStoreWrapper;
        [SetUp]
        public void Setup()
        {
            //https://docs.devexpress.com/XPO/DevExpress.Xpo.Session._methods
            string conn = DevExpress.Xpo.DB.AccessConnectionProvider.GetConnectionString("TestDb.mdb");

            var AccessDataStore = XpoDefault.GetConnectionProvider(conn, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
            dataStoreWrapper = new DataStoreWrapper(AccessDataStore);
            dl = new SimpleDataLayer(dataStoreWrapper);
            //dl = XpoDefault.GetDataLayer(conn, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
            using (Session session = new Session(dl))
            {
                System.Reflection.Assembly[] assemblies =
                        new System.Reflection.Assembly[] {
                                   typeof(Customer).Assembly
                   };
                session.UpdateSchema(assemblies);
                session.CreateObjectTypeRecords(assemblies);
            }
            UnitOfWork unitOfWork = new UnitOfWork(dl);
            if (unitOfWork.FindObject<Customer>(null) == null)
            {
                var Javier = new Customer(unitOfWork) { Code = "001", Name = "Javier" };
                var Hector = new Customer(unitOfWork) { Code = "002", Name = "Hector" };
                var Oniel = new Customer(unitOfWork) { Code = "003", Name = "Oniel" };
                var Hismel = new Customer(unitOfWork) { Code = "004", Name = "Hismel" };
                var Joche = new Customer(unitOfWork) { Code = "005", Name = "Joche" };


                var Computer = new Product(unitOfWork) { Code = "001", Name = "Computer" };
                var Cellphone = new Product(unitOfWork) { Code = "002", Name = "Cellphone" };
                var Laptop = new Product(unitOfWork) { Code = "003", Name = "Laptop" };
                unitOfWork.CommitChanges();
            }

        }
        [Test]
        public void WithoutBulkLoad()
        {
            UnitOfWork unitOfWork = new UnitOfWork(dl);
            XPCollection<Customer> Customers = new XPCollection<Customer>(unitOfWork);
            XPCollection<Product> Products = new XPCollection<Product>(unitOfWork);

            dataStoreWrapper.ResetDatabaseTripCounter();

            foreach (Customer customer in Customers)
            {
                Debug.WriteLine($"{customer.Code} {customer.Name}");
            }
            foreach (Product product in Products)
            {
                Debug.WriteLine($"{product.Code} {product.Name}");
            }
            var Trips = dataStoreWrapper.GetTotalDatabaseTrips();
            Assert.AreEqual(2, Trips);
        }
        [Test]
        public void WithBulkLoad()
        {
            UnitOfWork unitOfWork = new UnitOfWork(dl);
            XPCollection<Customer> Customers = new XPCollection<Customer>(unitOfWork);
            XPCollection<Product> Products = new XPCollection<Product>(unitOfWork);
            dataStoreWrapper.ResetDatabaseTripCounter();
            unitOfWork.BulkLoad(Customers, Products);

            foreach (Customer customer in Customers)
            {
                Debug.WriteLine($"{customer.Code} {customer.Name}");
            }
            foreach (Product product in Products)
            {
                Debug.WriteLine($"{product.Code} {product.Name}");
            }
            var Trips = dataStoreWrapper.GetTotalDatabaseTrips();
            Assert.AreEqual(1, Trips);
        }
        [Test]
        public void CollectionFromLambda()
        {
            UnitOfWork unitOfWork = new UnitOfWork(dl);

            //Old Way
            //XPCollection<Customer> Customers = new XPCollection<Customer>(unitOfWork);
            //XPCollection<Product> Products = new XPCollection<Product>(unitOfWork);



            XPCollection<Customer> Customers = unitOfWork.GetCollection<Customer>(c => c.Name != "");
            XPCollection<Product> Products = unitOfWork.GetCollection<Product>(c => c.Name != "");
            dataStoreWrapper.ResetDatabaseTripCounter();

            unitOfWork.BulkLoad(Customers, Products);

            foreach (Customer customer in Customers)
            {
                Debug.WriteLine($"{customer.Code} {customer.Name}");
            }
            foreach (Product product in Products)
            {
                Debug.WriteLine($"{product.Code} {product.Name}");
            }
            var Trips = dataStoreWrapper.GetTotalDatabaseTrips();
            Assert.AreEqual(1, Trips);
        }
        [Test]
        public void LoadMultipleCollections()
        {
            UnitOfWork unitOfWork = new UnitOfWork(dl);

            dataStoreWrapper.ResetDatabaseTripCounter();

            var collections = unitOfWork.GetCollections<Customer, Product>( c => c.Name != "",p => p.Name != "");


            unitOfWork.BulkLoad(collections);

            foreach (Customer customer in collections[0])
            {
                Debug.WriteLine($"{customer.Code} {customer.Name}");
            }
            foreach (Product product in collections[1])
            {
                Debug.WriteLine($"{product.Code} {product.Name}");
            }
            var Trips = dataStoreWrapper.GetTotalDatabaseTrips();
            Assert.AreEqual(1, Trips);
        }
        [Test]
        public void BulkLoadMultipleCollections()
        {
            UnitOfWork unitOfWork = new UnitOfWork(dl);

            dataStoreWrapper.ResetDatabaseTripCounter();

            var collections = unitOfWork.BulkLoadCollections<Customer, Product>
                 (
                     c => c.Name != "",
                     p => p.Name != ""
                  );

            foreach (Customer customer in collections[0])
            {
                Debug.WriteLine($"{customer.Code} {customer.Name}");
            }
            foreach (Product product in collections[1])
            {
                Debug.WriteLine($"{product.Code} {product.Name}");
            }
            var Trips = dataStoreWrapper.GetTotalDatabaseTrips();
            Assert.AreEqual(1, Trips);
        }
    }
}