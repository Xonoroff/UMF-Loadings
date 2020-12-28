using Scripts.src.Feature.Views;

namespace Scripts.src.Infrastructure.Interfaces.Messaging.Factories
{
    public interface ILoadingViewSyncFactory
    {
        ILoadingView Create();
    }
}