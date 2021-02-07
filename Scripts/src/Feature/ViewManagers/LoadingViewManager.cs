using System;
using System.Threading;
using Core.src.Infrastructure;
using Core.src.Messaging;
using Cysharp.Threading.Tasks;
using Scripts.src.Feature.Managers;
using Scripts.src.Feature.Views;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Scripts.src.Feature.ViewManagers
{
    public class LoadingViewManager : ILoadingViewManager
    {
        private readonly ILoadingView loadingView;
        
        private readonly ILoadingManager loadingManager;

        private UniTaskCompletionSource<AsyncUnit> uniTaskCompletionSource;

        public LoadingViewManager(ILoadingView loadingView, ILoadingManager loadingManager)
        {
            this.loadingView = loadingView;
            this.loadingManager = loadingManager;
        }
        
        public UniTask<AsyncUnit> StartLoading(CancellationToken cancellationToken)
        {
            uniTaskCompletionSource = new UniTaskCompletionSource<AsyncUnit>();
            
            Initialize();
            
            var firstCommand = loadingManager.GetFirstCommand();
            loadingView.SetProgressText(firstCommand.Description);
            loadingView.SetDescriptionText(loadingManager.TotalCommands.ToString());
            Object.DontDestroyOnLoad(loadingView.GameObject);

            loadingManager.StartPreloading();

            return uniTaskCompletionSource.Task;
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
            uniTaskCompletionSource?.TrySetResult(AsyncUnit.Default);
        }

        private void OnLoadingCompletedHandler()
        {
            Object.Destroy(loadingView.GameObject);
            Resources.UnloadUnusedAssets();
            GC.Collect();
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
