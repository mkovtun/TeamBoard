namespace TeamBoard.DomainServices

open System
open TeamBoard.Domain
open TeamBoard.Domain.User
open TeamBoard.Commands

type UserService (eventStore: IEventStore) =
    let load id = eventStore.GetEventsForAggregate id |> fun x -> if Seq.isEmpty x then raise(EntityNotFoundException("User", id)) else replayUser x

    let save = eventStore.SaveEvents 
    
    let applyOn id version f =        
        load id |>                          
        f |>        
        save id version 

    member x.Handle (c: CreateUser) =
        create c.UserId c.Login c.Password c.Name |>
        save c.UserId -1

    member x.Handle (c: SetUserPhoto) =
        setPhoto c.Photo |>
        applyOn c.UserId c.OriginalVersion

    member x.Handle (c: CreateProject) =
        load c.CreatorId |>
        createProject c.ProjectId c.Caption |>
        save c.ProjectId -1
