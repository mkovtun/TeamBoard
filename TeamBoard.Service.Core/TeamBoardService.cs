using System;
using System.ServiceModel;
using TeamBoard.Commands;
using TeamBoard.DomainServices;
using TeamBoard.Events;
using System.Linq;

namespace TeamBoard.Service.Core
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Reentrant)]
    public class TeamBoardService : ITeamBoardService
    {
        internal static EventsBroadcaster broadcaster = new EventsBroadcaster();
        private MongoEventStore eventStore = null;

        public void Ping()
        {
            // If the client was not present, then send him history
            if (!TeamBoardService.broadcaster.EnsurePresent(this.Callback))
            {
                this.Callback.HandleHistory(this.EventStore.GetHistory().ToArray());
            }
            this.Callback.PingBack();
        }

        public void ProcessCommand(ICommand command)
        {
            CommandsDispatcher.Dispatch(command, this.EventStore);
        }
        
        private MongoEventStore EventStore
        {
            get
            {
                if (this.eventStore == null)
                    this.eventStore = new MongoEventStore();

                return this.eventStore;
            }
        }

        private ITeamBoardClient Callback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<ITeamBoardClient>();
            }
        }
    }
}
