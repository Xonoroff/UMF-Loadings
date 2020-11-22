using Core.src.Messaging;
using Core.src.Utils;
using Scripts.src.Infrastructure.Interfaces.Messaging.Signals;
using Zenject;

namespace PreloadingModule.src.Installers
{
    public class PreloadingModuleGlobalInstaller : GlobalInstallerBase<PreloadingModuleGlobalInstaller, PreloadingModuleInstaller>
    {
        protected override string SubContainerName => "PreloadingModuleInstaller";

        [Inject]
        private SignalBus eventBus;
        
        public override void InstallBindings()
        {
            eventBus.DeclareSignal<OnPreloadingShouldBeStarted>();
            
            base.InstallBindings();
        }
    }
}