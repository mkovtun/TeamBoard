using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using EventStore;
using EventStore.Dispatcher;
using EventStore.Serialization;
using TeamBoard.DomainServices;
using TeamBoard.Events;

namespace TeamBoard.Service.Core
{
    public class MongoEventStore: IEventStore
    {
        private static IStoreEvents mongoStore;
        private static MySerializer serializer;
        private static IDispatchCommits commitsDispatcher;

        static MongoEventStore()
        {
            var ieventType = typeof(IEvent);
            var eventTypes = ieventType.Assembly.GetTypes().Where(x => ieventType.IsAssignableFrom(x)).ToArray();

            serializer = new MySerializer(eventTypes);
            commitsDispatcher = new CommitsDispatcher(serializer);

            mongoStore = Wireup
                .Init()
                .LogToOutputWindow()
                //.UsingMongoPersistence("EventStore", new DocumentObjectSerializer())
                .UsingAsynchronousDispatchScheduler()
                .DispatchTo(commitsDispatcher)
                .Build();
        }

        public IEnumerable<IEvent> GetEventsForAggregate(Guid aggregateId)
        {
            return mongoStore.OpenStream(aggregateId, int.MinValue, int.MaxValue).CommittedEvents.Select(x => serializer.Deserialize<IEvent>((string)x.Body));
        }

        public void SaveEvents(Guid aggregateId, int version, IEnumerable<IEvent> events)
        {
            using (var stream = mongoStore.OpenStream(aggregateId, int.MinValue, int.MaxValue))
            {
                foreach (var @event in events)
                {
                    stream.Add(new EventMessage { Body = serializer.Serialize(@event) });
                }
                stream.CommitChanges(Guid.NewGuid());
            }
        }

        public IEnumerable<IEvent> GetHistory()
        {
            return mongoStore.Advanced.GetFrom(DateTime.MinValue).SelectMany(x => x.Events).Select(x => serializer.Deserialize<IEvent>((string)x.Body));
        }
    }

    public class CommitsDispatcher : IDispatchCommits
    {
        private MySerializer serializer;

        public CommitsDispatcher(MySerializer serializer)
        {
            this.serializer = serializer;
        }

        public void Dispatch(Commit commit)
        {
            foreach (var @event in commit.Events)
                TeamBoardService.broadcaster.SendEvent(this.serializer.Deserialize<IEvent>((string)@event.Body));
        }

        public void Dispose()
        {
        }
    }

    public class MySerializer : ISerialize
    {
        private IEnumerable<Type> types;

        public MySerializer(params Type[] knownTypes)
        {
            this.types = knownTypes;
        }

        public T Deserialize<T>(string data)
        {
            using (var mem = new MemoryStream(data.Length))
            {
                using (var w = new StreamWriter(mem))
                {
                    w.Write(data);
                    w.Flush();
                    mem.Seek(0, SeekOrigin.Begin);
                    return this.Deserialize<T>(mem);
                }
            }
        }

        public string Serialize<T>(T obj)
        {
            using (var mem = new MemoryStream())
            {
                this.Serialize(mem, obj);
                mem.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(mem))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public T Deserialize<T>(Stream input)
        {
            var dcs = new DataContractJsonSerializer(typeof(T), this.types);
            return (T)dcs.ReadObject(input);
        }

        public void Serialize<T>(Stream output, T graph)
        {
            var dcs = new DataContractJsonSerializer(typeof(T), this.types);
            dcs.WriteObject(output, graph);
        }
    }
}