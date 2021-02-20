using System;
using Core.src.Infrastructure;

namespace Scripts.src.Feature.Managers
{
    public interface ILoadingManager
    {
        event Action<ICommand> OnCommandStartedExecution;
        
        event Action<ICommand> OnCommandCompleted;
        
        event Action<ICommand, Exception> OnCommandFailed;

        event Action<ICommand, float> OnCommandProgressChanged;

        event Action OnLoadingCompleted;

        ICommand CurrentCommand { get; }
        
        int TotalCommands { get; }
        int CurrentCommandIndex { get; }

        void StartPreloading();
        
        ICommand GetFirstCommand();
    }
}