using Core.src.Messaging;
using Scripts.src.Feature.ViewManagers;
using Scripts.src.Feature.Views;
using Scripts.src.Infrastructure.Interfaces.Messaging.Signals;
using Zenject;

namespace Scripts.src.Feature.Installers
{
    public class LoadingModuleSignalsInstaller : Installer<LoadingModuleSignalsInstaller>
    {
        [Inject]
        private IEventBus eventBus;
        
        public override void InstallBindings()
        {
            eventBus.Subscribe<OnLoadingShouldBeStartedSignal>(OnPreloadingShouldBeStartedHandler);
        }

        private void OnPreloadingShouldBeStartedHandler(OnLoadingShouldBeStartedSignal signal)
        {
            var instance = signal.Model.LoadingViewFactory.Create();
            Container.Rebind<ILoadingView>().FromInstance(instance).AsCached();
            var viewManager = Container.Resolve<ILoadingViewManager>();
            viewManager.StartLoading();
        }
    }
}