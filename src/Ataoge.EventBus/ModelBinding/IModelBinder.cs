using System.Threading.Tasks;

namespace Ataoge.EventBus.ModelBinding
{
    /// <summary>
    /// Defines an interface for model binders.
    /// </summary>
    public interface IModelBinder
    {
        Task<ModelBindingResult> BindModelAsync(string content);
    }

    
}