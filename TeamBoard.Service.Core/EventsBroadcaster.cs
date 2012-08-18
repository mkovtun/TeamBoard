using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBoard.Events;

namespace TeamBoard.Service.Core
{
    public class EventsBroadcaster
    {
        private List<ITeamBoardClient> clients = new List<ITeamBoardClient>();

        public void Add(ITeamBoardClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            if (this.clients.Contains(client))
                throw new ArgumentException("This client is already present");

            this.clients.Add(client);
        }

        public bool EnsurePresent(ITeamBoardClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            if (!this.clients.Contains(client))
            {
                this.clients.Add(client);
                return false;
            }
            return true;
        }

        public void Remove(ITeamBoardClient client)
        {
            this.clients.Remove(client);
        }

        public void SendEvent(IEvent @event)
        {
            foreach (var client in this.clients)
                client.HandleEvent(@event);
        }
    }
}
