using System.Collections.Generic;
using System.Threading;
using Core.src.Infrastructure;
using Core.src.Messaging;
using Cysharp.Threading.Tasks;
using External.Scripts.src.Infrastructure.Interfaces.Messaging.Signals;
using Scripts.src.Feature.ViewManagers;
using Scripts.src.Feature.Views;
using Zenject;

namespace External.Scripts.src.Feature.Installers
{
    public class LoadingModuleSignalsInstaller : Installer<LoadingModuleSignalsInstaller>
    {
        [Inject]
        private IEventBus eventBus;

        public override void InstallBindings()
        {
            eventBus.Subscribe<StartLoadingRequest>((request) => OnStartLoadingRequestHandler(request).Forget());
        }

        private async UniTaskVoid OnStartLoadingRequestHandler(StartLoadingRequest request)
        {
            var instance = await request.LoadingViewSyncFactory.CreateAsync(request.CancellationToken);
            var commands = request.Commands;

            await StartLoading(instance, commands, request.CancellationToken);
            
            request.Callback?.Invoke(DummyResponse.Default);
        }
        

        private async UniTask<AsyncUnit> StartLoading(ILoadingView loadingView, List<ICommand> commands, CancellationToken cancellationToken)
        {
            Container.Rebind<ILoadingView>().FromInstance(loadingView).AsTransient();
            Container.Rebind<List<ICommand>>().FromInstance(commands).AsTransient();
            
            var viewManager = Container.Resolve<ILoadingViewManager>();
            //Here's a bit magic to unload from memory view
            Container.Unbind<ILoadingView>();
            Container.Unbind<List<ICommand>>();
            
            await viewManager.StartLoading(cancellationToken);

            return AsyncUnit.Default;
        }
    }
}