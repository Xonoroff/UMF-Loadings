using Core.src.Utils;
using Scripts.src.Infrastructure.Interfaces.Messaging.Signals;
using Zenject;

namespace Scripts.src.Feature.Installers
{
    public class LoadingModuleGlobalInstaller : GlobalInstallerBase<LoadingModuleGlobalInstaller, LoadingModuleInstaller>
    {
        protected override string SubContainerName => "LoadingModuleInstaller";

        [Inject]
        private SignalBus eventBus;
        
        public override void InstallBindings()
        {
            eventBus.DeclareSignal<OnLoadingShouldBeStartedSignal>();
            eventBus.DeclareSignal<OnLoadingShouldBeStartedAsyncSignal>();
            eventBus.DeclareSignal<OnLoadingCompletedSignal>();
            
            base.InstallBindings();
        }
    }
}