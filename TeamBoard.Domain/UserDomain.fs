namespace TeamBoard.Domain

open System
open TeamBoard.Domain.Core
open TeamBoard.Domain.Utils
open TeamBoard.Events

module User = 
    type userState =
        {
            Id: Guid
            Login: string
            Password: string
            Name: string
            Photo: byte[]
        }

    let create id login password name =
        if id = Guid.Empty then raise(ArgumentNullException "id")
        if String.IsNullOrEmpty(login) then raise(ArgumentNullException "login")
        if String.IsNullOrEmpty(name) then raise(ArgumentNullException "name")

        fire { UserCreated.Id = id; Login = login; Password = password; Name = name }

    let setPhoto photo state =
        fire { UserPhotoSet.Id = state.Id; Photo = photo }

    let createProject projectId caption state =
        Project.create projectId state.Id caption

    let apply state (e: IEvent) =        
        match e with        
        | :? UserCreated as e -> { state with userState.Id = e.Id; Login = e.Login; Password = e.Password; Name = e.Name }
        | :? UserPhotoSet as e -> { state with userState.Photo = e.Photo }
        | _ -> raise(EntityUnhandledEventException())
        
    let replayUser events =        
        let empty = { Id = Guid.Empty; Login = String.Empty; Password = String.Empty; Name = String.Empty; Photo = Array.empty }
        Seq.fold apply empty events