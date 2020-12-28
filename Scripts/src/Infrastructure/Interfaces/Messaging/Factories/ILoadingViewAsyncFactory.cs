using System.Threading.Tasks;
using Scripts.src.Feature.Views;

namespace Scripts.src.Infrastructure.Interfaces.Messaging.Factories
{
    public interface ILoadingViewAsyncFactory
    {
#if UNITASK_ENABLED
         Cysharp.Threading.Tasks.UniTask<ILoadingView> CreateAsync();
#else
        Task<ILoadingView> CreateAsync();
#endif
    }
}