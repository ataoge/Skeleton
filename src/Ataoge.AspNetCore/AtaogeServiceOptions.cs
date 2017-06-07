using System;

namespace Ataoge.AspNetCore
{
    public class AtaogeServiceOptions
    {
        

        //public PlugInSourceList PlugInSources { get; }

        
        public AtaogeServiceOptions()
        {
            //PlugInSources = new PlugInSourceList();
        }

        public Type DefaultKeyType {get; set;} = typeof(int);

        public Type UserKeyType {get; set;}

    }
}