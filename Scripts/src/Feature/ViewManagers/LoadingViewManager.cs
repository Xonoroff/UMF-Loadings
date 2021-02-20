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

        private readonly IEventBus eventBus;

        private float currentProgress;

        private LoadingViewEntity cachedViewEntity;

        private UniTaskCompletionSource<AsyncUnit> uniTaskCompletionSource;

        private int delayBeforeViewDestroy;
        
        public LoadingViewManager(ILoadingView loadingView, ILoadingManager loadingManager)
        {
            this.loadingView = loadingView;
            this.loadingManager = loadingManager;
        }
        
        public UniTask<AsyncUnit> StartLoading(CancellationToken cancellationToken, int delayBeforeDestroy = 500)
        {
            this.delayBeforeViewDestroy = delayBeforeDestroy;
            uniTaskCompletionSource = new UniTaskCompletionSource<AsyncUnit>();
            
            Initialize();

            currentProgress = 0;
            var firstCommand = loadingManager.GetFirstCommand();
            cachedViewEntity = new LoadingViewEntity()
            {
                CurrentProgress = currentProgress,
                CurrentCommand = 0,
                TotalCommands = loadingManager.TotalCommands,
                CurrentDescription = firstCommand.Description,
            };
            
            loadingView.SetViewEntity(cachedViewEntity);
            
            Object.DontDestroyOnLoad(loadingView.GameObject);

            loadingManager.StartPreloading();

            return uniTaskCompletionSource.Task;
        }
        
        private void Initialize()
        {
            loadingManager.OnCommandStartedExecution += OnCommandStartedExecution;
            loadingManager.OnCommandCompleted += OnCommandCompletedHandler;
            loadingManager.OnCommandFailed += OnCommandFailedHandler;
            loadingManager.OnCommandProgressChanged += OnCommandProgressChanged;
            loadingManager.OnLoadingCompleted += () => OnLoadingCompletedHandler().Forget();
        }

        private void OnCommandProgressChanged(ICommand command, float progress)
        {

            cachedViewEntity.CurrentProgress = CalculateProgress(progress);
            cachedViewEntity.CurrentCommand = loadingManager.CurrentCommandIndex;
            cachedViewEntity.TotalCommands = loadingManager.TotalCommands;
            cachedViewEntity.CurrentDescription = command.Description;
            
            loadingView.SetViewEntity(cachedViewEntity);
        }

        private void OnCommandStartedExecution(ICommand command)
        {
            
            cachedViewEntity.CurrentProgress = CalculateProgress(0);
            cachedViewEntity.CurrentCommand = loadingManager.CurrentCommandIndex;
            cachedViewEntity.TotalCommands = loadingManager.TotalCommands;
            cachedViewEntity.CurrentDescription = command.Description;
            loadingView.SetViewEntity(cachedViewEntity);
        }

        private async UniTaskVoid OnLoadingCompletedHandler()
        {
            await UniTask.Delay(delayBeforeViewDestroy);
            
            Object.Destroy(loadingView.GameObject);
            Resources.UnloadUnusedAssets();
            GC.Collect();
            uniTaskCompletionSource?.TrySetResult(AsyncUnit.Default);
        }

        private void OnCommandFailedHandler(ICommand failedCommand, Exception exception)
        {
            var viewEntity = new LoadingViewEntity()
            {
                CurrentProgress = CalculateProgress(1),
                CurrentCommand = loadingManager.CurrentCommandIndex,
                TotalCommands = loadingManager.TotalCommands,
                CurrentDescription = failedCommand.Description,
            };
            loadingView.SetViewEntity(viewEntity);
        }

        private void OnCommandCompletedHandler(ICommand command)
        {
            var viewEntity = new LoadingViewEntity()
            {
                CurrentProgress = CalculateProgress(1),
                CurrentCommand = loadingManager.CurrentCommandIndex,
                TotalCommands = loadingManager.TotalCommands,
                CurrentDescription = command.Description,
            };
            loadingView.SetViewEntity(viewEntity);
        }

        private float CalculateProgress(float commandProgress)
        {
            float step = 1 / (float)(loadingManager.TotalCommands);
            float min = loadingManager.CurrentCommandIndex * step;
            float max = (loadingManager.CurrentCommandIndex + 1) * step;
            float result = min + ((max - min) * commandProgress); 
            return result;
        }
    }
}
