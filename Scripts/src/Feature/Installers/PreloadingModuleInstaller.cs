using PreloadingModule.src.Managers;
using PreloadingModule.src.Views;
using Scripts.src.Feature.Entities;
using Zenject;

namespace PreloadingModule.src.Installers
{
    public class PreloadingModuleInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<PreloadingModuleConfig>().FromResource(PreloadingModuleConfig.FILE_NAME).AsTransient();
            Container.Bind<IPreloadingManager>().To<PreloadingManager>().AsCached();
            Container.Bind<IPreloadingViewManager>().To<PreloadingViewManager>().AsTransient();
            Container.Bind<IPreloadingView>().FromResolveGetter<PreloadingModuleConfig>(
                (x) =>
                {
                    var prefab = x.GetPreloadingView();
                    var instantiatedPrefab = Container.InstantiatePrefab(prefab.GameObject);
                    return instantiatedPrefab.GetComponent<IPreloadingView>();
                })
                .AsCached();

            PreloadingModuleSignalsInstaller.Install(Container);
        }
    }
}