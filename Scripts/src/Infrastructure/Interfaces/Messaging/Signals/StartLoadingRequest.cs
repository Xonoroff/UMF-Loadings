using System.Collections.Generic;
using System.Threading;
using Core.src.Infrastructure;
using Core.src.Messaging;
using Scripts.Core.src.Infrastructure;
using Scripts.src.Feature.Views;

namespace External.Scripts.src.Infrastructure.Interfaces.Messaging.Signals
{
    public class StartLoadingRequest : EventBusRequest<DummyResponse>
    {
        public IFactoryAsync<ILoadingView> LoadingViewSyncFactory { get; set; }
            
        public List<ICommand> Commands { get; set; }
        
        public CancellationToken CancellationToken { get; set; }
    }
}