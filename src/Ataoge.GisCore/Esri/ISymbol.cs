namespace Ataoge.GisCore
{
    public interface ISymbol
    {
      

    }

    public enum SymbolType
    {
          esriSMS,
          esriSLS,
          esriSFS,
          esriPMS,
          esriPFS,
          esriTS
    }

    public abstract class Symbol : ISymbol
    {
        public virtual SymbolType Type {get; set;}

        //public string Style {get; set;} 

        //public int[] Color {get; set;}
    }
   
}