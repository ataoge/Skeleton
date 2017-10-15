using System.Collections.Generic;

namespace Ataoge.Data
{
    public interface IFilterInfo
    {
        int RecordFiltered {get; set;}

        IList<string> Filters {get; set;}
    }

}