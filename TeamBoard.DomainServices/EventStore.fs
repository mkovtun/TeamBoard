namespace TeamBoard.DomainServices

open System
open TeamBoard.Events

type IEventStore =    
    abstract member GetEventsForAggregate : Guid -> IEvent seq
    abstract member SaveEvents : Guid -> int -> IEvent seq -> unit
