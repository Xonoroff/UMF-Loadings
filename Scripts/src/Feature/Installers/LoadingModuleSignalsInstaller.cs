using System.Collections.Generic;
using System.Threading;
using Core.src.Infrastructure;
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

        private CancellationTokenSource loadingCancellationTokenSource;

        public override void InstallBindings()
        {
            eventBus.Subscribe<OnLoadingShouldBeStartedSignal>(OnPreloadingShouldBeStartedHandler);
            eventBus.Subscribe<OnLoadingShouldBeStartedAsyncSignal>(OnPreloadingShouldBeStartedAsyncHandler);
            eventBus.Subscribe<OnLoadingCompletedSignal>(OnLoadingCompletedHandler);
        }

        private void OnPreloadingShouldBeStartedHandler(OnLoadingShouldBeStartedSignal signal)
        {
            var instance = signal.Model.LoadingViewSyncFactory.Create();
            var commands = signal.Model.Commands;

            StartLoading(instance, commands);
        }

        private async void OnPreloadingShouldBeStartedAsyncHandler(OnLoadingShouldBeStartedAsyncSignal signal)
        {
            loadingCancellationTokenSource = new CancellationTokenSource();
            var instance = await signal.Model.LoadingViewSyncFactory.CreateAsync(loadingCancellationTokenSource.Token);
            var commands = signal.Model.Commands;

            StartLoading(instance, commands);
        }

        private void StartLoading(ILoadingView loadingView, List<ICommand> commands)
        {
            Container.Rebind<ILoadingView>().FromInstance(loadingView).AsTransient();
            Container.Rebind<List<ICommand>>().FromInstance(commands).AsTransient();
            var viewManager = Container.Resolve<ILoadingViewManager>();
            //Here's a bit magic to unload from memory view
            Container.Unbind<ILoadingView>();
            Container.Unbind<List<ICommand>>();
            viewManager.StartLoading();
        }

        private void OnLoadingCompletedHandler()
        {
            loadingCancellationTokenSource?.Cancel();
        }
    }
}