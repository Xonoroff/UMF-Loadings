using System.Threading;
using Cysharp.Threading.Tasks;

namespace Scripts.src.Feature.ViewManagers
{
    public interface ILoadingViewManager
    {
        UniTask<AsyncUnit> StartLoading(CancellationToken cancellationToken, int delayBeforeDestroy = 500);
    }
}