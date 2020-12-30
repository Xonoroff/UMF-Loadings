using System.Collections.Generic;
using Core.src.Infrastructure;
using Core.src.Signals;
using Scripts.Core.src.Infrastructure;
using Scripts.src.Feature.Views;

namespace Scripts.src.Infrastructure.Interfaces.Messaging.Signals
{
    public class OnLoadingShouldBeStartedSignal : SignalBaseWithParameter<OnLoadingShouldBeStartedSignal.OnLoadingShouldBeStartedSignalData>
    {
        public class OnLoadingShouldBeStartedSignalData
        {
            public IFactorySync<ILoadingView> LoadingViewSyncFactory { get; set; }
            
            public List<ICommand> Commands { get; set; }
        }
        public OnLoadingShouldBeStartedSignal(OnLoadingShouldBeStartedSignalData model) : base(model)
        {
        }
    }
}
