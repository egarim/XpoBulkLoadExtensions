using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace XpoBulkLoad
{
    public class Customer : DevExpress.Xpo.XPObject
    {
        protected Customer()
        {

        }

        public Customer(Session session) : base(session)
        {

        }

        public Customer(Session session, XPClassInfo classInfo) : base(session, classInfo)
        {

        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }

        string code;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }


    }
}