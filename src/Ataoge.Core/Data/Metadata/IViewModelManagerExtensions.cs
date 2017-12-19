using System.Collections.Generic;
using System.Linq;

namespace Ataoge.Data.Metadata
{
    public static class ViewModelManagerExtensions
    {
        public static IViewModel GetOrAddViewModel<TViewModel>(this IViewModelManager viewModelManager) 
            => viewModelManager.GetOrAddViewModel(typeof(TViewModel)); 

        public static IViewModel FindViewModel<TViewModel>(this IViewModelManager viewModelManager) 
            => viewModelManager.FindViewModel(typeof(TViewModel)); 

         public static IViewModel AddViewModel<TViewModel>(this IViewModelManager viewModelManager) 
            => viewModelManager.AddViewModel(typeof(TViewModel)); 

        public static IEnumerable<UiColumnInfo> GetSortedColumnInfos(this IViewModel viewModel)
            => viewModel.GetColumnInfos().OrderByDescending(t => t.Weight);
        public static IEnumerable<UiColumnInfo> GetSearchableColumnInfos(this IViewModel viewModel)
            => viewModel.GetColumnInfos().Where(t => t.Searchable == true);
        public static IEnumerable<UiColumnInfo> GetFilterableColumnInfos(this IViewModel viewModel)
            => viewModel.GetColumnInfos().Where(t => t.Filterable == true);

    }
}