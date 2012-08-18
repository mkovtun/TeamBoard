namespace TeamBoard.Events

open System
open System.Runtime.Serialization

type IEvent =
    interface
    end

type IAggregateEvent =
    interface
        inherit IEvent
        abstract Id: Guid
    end

type EventMetadata =    
    {        
        Version : int    
    }

[<DataContract>]
type TeamMemberRole = 
    | Owner 
    | Contributor