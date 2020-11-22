using System;
using System.Collections.Generic;
using Core.src.Infrastructure;

namespace PreloadingModule.src.Managers
{
    public class PreloadingManager : IPreloadingManager
    {
        public event Action<ICommand> OnCommandStartedExecution;
        public event Action<ICommand> OnCommandCompleted;
        public event Action<ICommand, Exception> OnCommandFailed;
        public event Action OnPreloadingCompleted;

        public ICommand CurrentCommand => commandExecutor.Current;

        public int TotalCommands => commandExecutor.TotalCommands;
        public int CurrentCommandIndex => commandExecutor.CurrentCommandIndex;

        private ICommandExecutor commandExecutor;

        private List<ICommand> commandsToExecute;

        public PreloadingManager(ICommandExecutor commandExecutor, List<ICommand> commandsToExecute)
        {
            this.commandsToExecute = commandsToExecute;
            this.commandExecutor = commandExecutor;
            Initialize();
        }

        private void Initialize()
        {
            commandExecutor.Initialize(commandsToExecute);
        }
        
        public void StartPreloading()
        {
            commandExecutor.StartExecution();
        }

        public ICommand GetFirstCommand()
        {
            return commandsToExecute[0];
        }
    }
}