using System;
using Core.src.Infrastructure;
using Core.src.Messaging;
using Scripts.src.Feature.Managers;
using Scripts.src.Feature.Views;
using Scripts.src.Infrastructure.Interfaces.Messaging.Signals;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Scripts.src.Feature.ViewManagers
{
    public class LoadingViewManager : ILoadingViewManager
    {
        [Inject]
        private ILoadingView loadingView;
        
        [Inject]
        private ILoadingManager loadingManager;

        [Inject]
        private IEventBus eventBus;

        public LoadingViewManager(ILoadingView loadingView,
            ILoadingManager loadingManager,
            IEventBus eventBus)
        {
            this.loadingView = loadingView;
            this.loadingManager = loadingManager;
            this.eventBus = eventBus;
        }
        
        public void StartLoading()
        {
            Initialize();
            
            var firstCommand = loadingManager.GetFirstCommand();
            loadingView.SetProgressText(firstCommand.Description);
            loadingView.SetDescriptionText(loadingManager.TotalCommands.ToString());
            Object.DontDestroyOnLoad(loadingView.GameObject);

            loadingManager.StartPreloading();
        }
        
        private void Initialize()
        {
            loadingManager.OnCommandStartedExecution += OnCommandStartedExecution;
            loadingManager.OnCommandCompleted += OnCommandCompletedHandler;
            loadingManager.OnCommandFailed += OnCommandFailedHandler;
            loadingManager.OnLoadingCompleted += OnLoadingCompletedHandler;
        }

        private void OnCommandStartedExecution(ICommand command)
        {
            var currentProgress = GetExecutingProgress(loadingManager.CurrentCommandIndex + 1);
            loadingView.SetProgressText(currentProgress);
            loadingView.SetProgressText(command.Description);
        }

        private void OnLoadingCompletedHandler()
        {
            Object.Destroy(loadingView.GameObject);
            Resources.UnloadUnusedAssets();
            GC.Collect();
            eventBus.Fire(new OnLoadingCompletedSignal());
        }

        private void OnCommandFailedHandler(ICommand failedCommand, Exception exception)
        {
            var currentProgress = GetExecutingProgress(loadingManager.CurrentCommandIndex + 2);
            loadingView.SetProgressText(currentProgress);
        }

        private void OnCommandCompletedHandler(ICommand command)
        {
            var currentProgress = GetExecutingProgress(loadingManager.CurrentCommandIndex + 2);
            loadingView.SetProgressText(currentProgress);
        }

        private string GetExecutingProgress(int currentProgress)
        {
            return $"{currentProgress}/{loadingManager.TotalCommands}";
        }
    }
}
