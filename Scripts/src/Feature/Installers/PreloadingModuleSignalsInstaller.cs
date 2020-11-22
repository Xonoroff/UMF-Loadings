using Core.src.Messaging;
using PreloadingModule.src.Managers;
using Scripts.src.Infrastructure.Interfaces.Messaging.Signals;
using Zenject;

namespace PreloadingModule.src.Installers
{
    public class PreloadingModuleSignalsInstaller : Installer<PreloadingModuleSignalsInstaller>
    {
        [Inject]
        private IEventBus eventBus;
        
        public override void InstallBindings()
        {
            eventBus.Subscribe<OnPreloadingShouldBeStarted>(OnPreloadingShouldBeStartedHandler);
        }

        private void OnPreloadingShouldBeStartedHandler(OnPreloadingShouldBeStarted signal)
        {
            var viewManager = Container.Resolve<IPreloadingViewManager>();
            viewManager.StartPreloading();
        }
    }
}