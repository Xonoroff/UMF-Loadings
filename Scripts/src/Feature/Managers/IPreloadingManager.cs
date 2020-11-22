using System;
using Core.src.Infrastructure;

namespace PreloadingModule.src.Managers
{
    public interface IPreloadingManager
    {
        event Action<ICommand> OnCommandStartedExecution;
        
        event Action<ICommand> OnCommandCompleted;
        
        event Action<ICommand, Exception> OnCommandFailed;
        
        event Action OnPreloadingCompleted;

        ICommand CurrentCommand { get; }
        
        int TotalCommands { get; }
        int CurrentCommandIndex { get; }

        void StartPreloading();
        
        ICommand GetFirstCommand();
    }
}