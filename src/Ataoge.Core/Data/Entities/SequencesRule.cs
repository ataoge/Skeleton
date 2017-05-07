using System;

namespace Ataoge.Data.Entities
{
    [DbTableAttribute("SEQUENCESRULE")]
    public class SequencesRule
    {
        [DbFieldAttribute("PATTERNAME", IsPrimaryKey = true)]
        public string PatternName {get; set;}

        [DbFieldAttribute("MINVALUE")]
        public int MinValue {get; set;} = 1;

        [DbFieldAttribute("MAXVALUE")]
        public int MaxValue {get; set;} = int.MaxValue;

        [DbFieldAttribute("NEXTVALUE")]
        public int NextValue {get; set;} = 2;

        [DbFieldAttribute("CONTINUUM")]
        public bool Continuum {get; set;} = false;

        [DbFieldAttribute("PRESERVEDCOUNT")]
        public int PreservedCount {get; set;} = 0;

        [DbFieldAttribute("TABLEFIELD")]
        public string TableField {get; set;}

        [DbFieldAttribute("STEP")]
        public int Step {get; set;} = 1;

        [DbFieldAttribute("UPDATETIME")]
        public DateTime UpdateTime {get; set;}

    }

}