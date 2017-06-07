using System;

namespace Ataoge.Data.Entities
{
    [DbTableAttribute("SequencesRule")]
    public class SequencesRule
    {
        [DbFieldAttribute("PatternName", IsPrimaryKey = true)]
        public string PatternName {get; set;}

        [DbFieldAttribute("MinValue")]
        public int MinValue {get; set;} = 1;

        [DbFieldAttribute("MaxValue")]
        public int MaxValue {get; set;} = int.MaxValue;

        [DbFieldAttribute("NextValue")]
        public int NextValue {get; set;} = 2;

        [DbFieldAttribute("Continuum")]
        public bool Continuum {get; set;} = false;

        [DbFieldAttribute("PreservedCount")]
        public int PreservedCount {get; set;} = 0;

        [DbFieldAttribute("TableField")]
        public string TableField {get; set;}

        [DbFieldAttribute("Step")]
        public int Step {get; set;} = 1;

        [DbFieldAttribute("UpdateTime")]
        public DateTime UpdateTime {get; set;}

    }

}