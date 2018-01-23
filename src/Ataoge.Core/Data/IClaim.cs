using System.Collections.Generic;

namespace Ataoge.Data
{
    public interface IClaimable
    {
        IEnumerable<IClaim> Claims {get;}
    }

    public interface IClaim
    {
        string Type {get; set;}

        string Value {get; set;}
    }
}