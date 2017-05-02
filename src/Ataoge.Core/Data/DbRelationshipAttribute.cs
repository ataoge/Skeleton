using System;

namespace Ataoge.Data
{

    public enum RelationshipFlags
    {
        Unknown = 0,
        OneToOne = 1,
        OneToMany = 2,
        ManyToOne = 4,
        ManyToMany = 8
    }

    public class DbRelationshipAttribute : Attribute
    {
        public string Name 
        {
            get;
            set;
        }

        public string TableName {get; set;}

        public string FieldName {get; set;}

        ///<summary>
        ///关系类型 0 未知， 1 OneToOne， 2：OneToMany 3: ManyToOne
        ///</summary>
        public RelationshipFlags Type {get; set;}

        public string ClassKey {get; set;}

        public string SecondaryClass {get; set;}

        public bool LasyLoaded { get; set;} = true;
    }
}