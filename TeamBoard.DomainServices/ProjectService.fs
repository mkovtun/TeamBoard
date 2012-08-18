namespace TeamBoard.DomainServices

open System
open TeamBoard.Domain
open TeamBoard.Domain.Project
open TeamBoard.Commands

type ProjectService (eventStore: IEventStore) =
    let load id = eventStore.GetEventsForAggregate id |> fun x -> if Seq.isEmpty x then raise(EntityNotFoundException("Project", id)) else replayProject x
    let save = eventStore.SaveEvents 
    
    let applyOn id version f =        
        load id |>                          
        f |>        
        save id version 
                
//    member x.Handle (c: CreateProject) =
//        create c.ProjectId c.Caption |>
//        save c.ProjectId -1
        
    member x.Handle (c: CreateTaskStatus) =
        createTaskStatus c.TaskStatusCaption |>
        applyOn c.ProjectId c.OriginalVersion

    member x.Handle (c: CreateUserStoryStatus) =
        createUserStoryStatus c.UserStoryStatusCaption |>
        applyOn c.ProjectId c.OriginalVersion

    member x.Handle (c: AddTaskStatusToUserStoryStatus) =
        addTaskStatusToUserStoryStatus c.UserStoryPosition c.TaskStatusPosition |>
        applyOn c.ProjectId c.OriginalVersion
