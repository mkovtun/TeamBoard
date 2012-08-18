using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBoard.DomainServices;
using TeamBoard.Events;

namespace TeamBoard.DomainTests
{
    public class InMemoryEventStore: IEventStore
    {
        private List<IEvent> events = new List<IEvent>();
        private List<IEvent> newEvents = new List<IEvent>();

        public IEnumerable<IEvent> GetEventsForAggregate(Guid id)
        {
            if (id == Guid.Empty)
                return this.events.Where(x => !(x is IAggregateEvent));

            return this.events.Where(x => x is IAggregateEvent).Cast<IAggregateEvent>().Where(x => x.Id == id);
        }

        public void SaveEvents(Guid id, int version, IEnumerable<IEvent> events)
        {
            this.events.AddRange(events);
            this.newEvents.AddRange(events);
        }

        public void SetupEventsHistory(IEnumerable<IEvent> events)
        {
            this.events.AddRange(events);
        }

        public IEnumerable<IEvent> NewEvents
        {
            get { return this.newEvents; }
        }
    }
}
