using System;
using System.Collections.Generic;
using Core.src.Infrastructure;

namespace Scripts.src.Feature.Managers
{
    public class LoadingManager : ILoadingManager
    {
        public event Action<ICommand> OnCommandStartedExecution;
        public event Action<ICommand> OnCommandCompleted;
        public event Action<ICommand, Exception> OnCommandFailed;
        public event Action OnLoadingCompleted;

        public ICommand CurrentCommand => commandExecutor.Current;

        public int TotalCommands => commandExecutor.TotalCommands;
        public int CurrentCommandIndex => commandExecutor.CurrentCommandIndex;

        private ICommandExecutor commandExecutor;

        private List<ICommand> commandsToExecute;

        public LoadingManager(ICommandExecutor commandExecutor, List<ICommand> commandsToExecute)
        {
            this.commandsToExecute = commandsToExecute;
            this.commandExecutor = commandExecutor;
            Initialize();
        }

        private void Initialize()
        {
            commandExecutor.Initialize(commandsToExecute);
            commandExecutor.OnCommandStartedExecution += OnCommandStartedExecution;
            commandExecutor.OnCommandCompleted += OnCommandCompleted;
            commandExecutor.OnCommandFailed += OnCommandFailed;
            commandExecutor.OnAllCompleted += (wasErrors) => OnLoadingCompleted?.Invoke();
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