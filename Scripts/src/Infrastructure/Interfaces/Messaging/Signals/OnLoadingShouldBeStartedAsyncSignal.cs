using System.Collections.Generic;
using Core.src.Infrastructure;
using Core.src.Signals;
using Scripts.Core.src.Infrastructure;
using Scripts.src.Feature.Views;

namespace Scripts.src.Infrastructure.Interfaces.Messaging.Signals
{
    public class OnLoadingShouldBeStartedAsyncSignal : SignalBaseWithParameter<OnLoadingShouldBeStartedAsyncSignal.OnLoadingShouldBeStartedSignalData>
    {
        public class OnLoadingShouldBeStartedSignalData
        {
            public IFactoryAsync<ILoadingView> LoadingViewSyncFactory { get; set; }
            
            public List<ICommand> Commands { get; set; }
        }
        
        public OnLoadingShouldBeStartedAsyncSignal(OnLoadingShouldBeStartedSignalData model) : base(model)
        {
        }
    }
}
