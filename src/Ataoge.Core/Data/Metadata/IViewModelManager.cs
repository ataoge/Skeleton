using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Ataoge.Data.Metadata
{
    public interface IViewModelManager
    {
        IViewModel GetOrAddViewModel([NotNull] Type clrType);

        IViewModel FindViewModel([NotNull] Type clrType);
        
        IViewModel AddViewModel([NotNull]Type clrType);

        IViewModel RemoveViewModel([NotNull] Type clrType);

        //IEnumerable<IViewModel> GetViewModels();
    }
}