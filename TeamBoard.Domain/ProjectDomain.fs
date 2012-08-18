namespace TeamBoard.Domain

open System
open TeamBoard.Domain.Core
open TeamBoard.Domain.Utils
open TeamBoard.Events

module Project = 
    type TaskStatus =
        {
            Caption: string
        }

    type UserStoryStatus =
        {
            Caption: string
            TaskStatuses: int[]
        }

    type TeamMember =
        {
            UserId: Guid
            Role: TeamMemberRole
        }

    type ProjectState =
        {
            Id: Guid
            Caption: string
            TaskStatuses: TaskStatus[]
            UserStoryStatuses: UserStoryStatus[]
            Team: TeamMember[]
        }

    let applyOnProject state (e: IEvent) =        
        match e with        
        | :? ProjectCreated as e -> {state with ProjectState.Id = e.Id; Caption = e.Caption}
        | :? TaskStatusCreated as e -> {state with TaskStatuses = Array.append state.TaskStatuses [| {TaskStatus.Caption = e.Caption} |]}
        | :? UserStoryStatusCreated as e -> {state with UserStoryStatuses = Array.append state.UserStoryStatuses [| {UserStoryStatus.Caption = e.Caption; TaskStatuses = Array.empty} |]}
        | :? TaskStatusToUserStoryStatusAdded as e -> {state with UserStoryStatuses = state.UserStoryStatuses |> Array.mapi (fun index elem -> if (index = e.UserStoryStatusPosition) then {elem with TaskStatuses = Array.append elem.TaskStatuses [| e.TaskStatusPosition |]} else elem) }
        | _ -> raise(EntityUnhandledEventException())
        
    let replayProject events =        
        let empty = { Id = Guid.Empty; Caption = String.Empty; TaskStatuses = Array.empty; UserStoryStatuses = Array.empty; Team = Array.empty }
        Seq.fold applyOnProject empty events

    let (==>) event action = Seq.append event ((replayProject event) |> action)

    let addTeamMember userId role state =
        fire {TeamMemberAdded.Id = state.Id;  CreatorId = userId; Role = role}

    let create id creatorId caption =
        if id = Guid.Empty then raise(ArgumentNullException "id")
        if creatorId = Guid.Empty then raise(ArgumentNullException "creatorId")
        if String.IsNullOrEmpty(caption) then raise(ArgumentNullException "caption")

        fire {ProjectCreated.Id = id; Caption = caption} 
        ==> (addTeamMember creatorId TeamMemberRole.Owner)

    let createTaskStatus caption state =
        fire {TaskStatusCreated.Id = state.Id; Caption = caption}

    let createUserStoryStatus caption state =
        fire {UserStoryStatusCreated.Id = state.Id; Caption = caption}

    let addTaskStatusToUserStoryStatus userStoryStatusPosition taskStatusPosition state =
        fire {TaskStatusToUserStoryStatusAdded.Id = state.Id; UserStoryStatusPosition = userStoryStatusPosition; TaskStatusPosition = taskStatusPosition}

