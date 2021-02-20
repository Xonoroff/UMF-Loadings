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
        private readonly ILoadingView loadingView;
        
        private readonly ILoadingManager loadingManager;

        private readonly IEventBus eventBus;

        private float currentProgress;

        private LoadingViewEntity cachedViewEntity;

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
        }
        
        private void Initialize()
        {
            loadingManager.OnCommandStartedExecution += OnCommandStartedExecution;
            loadingManager.OnCommandCompleted += OnCommandCompletedHandler;
            loadingManager.OnCommandFailed += OnCommandFailedHandler;
            loadingManager.OnCommandProgressChanged += OnCommandProgressChanged;
            loadingManager.OnLoadingCompleted += OnLoadingCompletedHandler;
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

        private void OnLoadingCompletedHandler()
        {
            Object.Destroy(loadingView.GameObject);
            Resources.UnloadUnusedAssets();
            GC.Collect();
            eventBus.Fire(new OnLoadingCompletedSignal());
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
            var maxProgressForCurrentCommand = (loadingManager.CurrentCommandIndex + 1) / loadingManager.TotalCommands + 1;
            return maxProgressForCurrentCommand * commandProgress;
        }
    }
}
