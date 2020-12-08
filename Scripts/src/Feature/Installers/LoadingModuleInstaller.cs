using Scripts.src.Feature.Managers;
using Scripts.src.Feature.ViewManagers;
using Zenject;

namespace Scripts.src.Feature.Installers
{
    public class LoadingModuleInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<ILoadingManager>().To<LoadingManager>().AsTransient();
            Container.Bind<ILoadingViewManager>().To<LoadingViewManager>().AsTransient();

            LoadingModuleSignalsInstaller.Install(Container);
        }
    }
}