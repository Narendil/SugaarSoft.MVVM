using System.Threading.Tasks;

namespace SugaarSoft.MVVM.Navigation
{
    interface INavigationService
    {
        Task NavigateTo<T>();
        Task NavigateBack();
    }
}
