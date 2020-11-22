using System;
using Core.src.Infrastructure;
using PreloadingModule.src.Views;
using Zenject;
using Object = UnityEngine.Object;

namespace PreloadingModule.src.Managers
{
    public class PreloadingViewManager : IPreloadingViewManager
    {
        [Inject]
        private IPreloadingView preloadingView;
        
        [Inject]
        private IPreloadingManager preloadingManager;

        public PreloadingViewManager(IPreloadingView preloadingView, IPreloadingManager preloadingManager)
        {
            this.preloadingView = preloadingView;
            this.preloadingManager = preloadingManager;
        }
        
        public void StartPreloading()
        {
            Initialize();
            
            var firstCommand = preloadingManager.GetFirstCommand();
            preloadingView.SetProgressText(firstCommand.Description);
            preloadingView.SetDescriptionText(preloadingManager.TotalCommands.ToString());
            Object.DontDestroyOnLoad(preloadingView.GameObject);

            preloadingManager.StartPreloading();
        }
        
        private void Initialize()
        {
            preloadingManager.OnCommandStartedExecution += OnCommandStartedExecution;
            preloadingManager.OnCommandCompleted += OnCommandCompletedHandler;
            preloadingManager.OnCommandFailed += OnCommandFailedHandler;
            preloadingManager.OnPreloadingCompleted += OnPreloadingCompletedHandler;
        }

        private void OnCommandStartedExecution(ICommand command)
        {
            var currentProgress = GetExecutingProgress(preloadingManager.CurrentCommandIndex + 1);
            preloadingView.SetProgressText(currentProgress);
            preloadingView.SetProgressText(command.Description);
        }

        private void OnPreloadingCompletedHandler()
        {
            
        }

        private void OnCommandFailedHandler(ICommand failedCommand, Exception exception)
        {
            var currentProgress = GetExecutingProgress(preloadingManager.CurrentCommandIndex + 2);
            preloadingView.SetProgressText(currentProgress);
        }

        private void OnCommandCompletedHandler(ICommand command)
        {
            var currentProgress = GetExecutingProgress(preloadingManager.CurrentCommandIndex + 2);
            preloadingView.SetProgressText(currentProgress);
        }

        private string GetExecutingProgress(int currentProgress)
        {
            return $"{currentProgress}/{preloadingManager.TotalCommands}";
        }
    }
}
