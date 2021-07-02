using DevExpress.Xpo;

namespace XpoBulkLoad
{
    public class Product : XPObject
    {
        public Product(Session session) : base(session)
        { }




        string name;
        string code;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Code
        {
            get => code;
            set => SetPropertyValue(nameof(Code), ref code, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }
    }
}