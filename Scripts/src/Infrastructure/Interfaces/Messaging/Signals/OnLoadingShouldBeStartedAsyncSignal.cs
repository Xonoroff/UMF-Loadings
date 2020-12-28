using System.Collections.Generic;
using Core.src.Infrastructure;
using Core.src.Signals;
using Scripts.src.Feature.Views;
using Scripts.src.Infrastructure.Interfaces.Messaging.Factories;
using Zenject;

namespace Scripts.src.Infrastructure.Interfaces.Messaging.Signals
{
    public class OnLoadingShouldBeStartedAsyncSignal : SignalBaseWithParameter<OnLoadingShouldBeStartedAsyncSignal.OnLoadingShouldBeStartedSignalData>
    {
        public class OnLoadingShouldBeStartedSignalData
        {
            public ILoadingViewAsyncFactory LoadingViewSyncFactory { get; set; }
            
            public List<ICommand> Commands { get; set; }
        }
        
        public OnLoadingShouldBeStartedAsyncSignal(OnLoadingShouldBeStartedSignalData model) : base(model)
        {
        }
    }
}
