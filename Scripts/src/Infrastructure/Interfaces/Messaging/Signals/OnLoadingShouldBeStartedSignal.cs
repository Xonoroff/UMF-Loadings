using System.Collections.Generic;
using Core.src.Infrastructure;
using Core.src.Signals;
using Scripts.src.Feature.Views;
using Zenject;

namespace Scripts.src.Infrastructure.Interfaces.Messaging.Signals
{
    public class OnLoadingShouldBeStartedSignal : SignalBaseWithParameter<OnLoadingShouldBeStartedSignal.OnLoadingShouldBeStartedSignalData>
    {
        public class OnLoadingShouldBeStartedSignalData
        {
            public IFactory<ILoadingView> LoadingViewFactory { get; set; }
            
            public List<ICommand> Commands { get; set; }
        }
        public OnLoadingShouldBeStartedSignal(OnLoadingShouldBeStartedSignalData model) : base(model)
        {
        }
    }
}
