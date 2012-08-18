namespace TeamBoard.DomainServices

open System
open TeamBoard.Commands

module CommandsDispatcher =

    let Dispatch (command: ICommand) (eventStore: IEventStore) =
        match command with
        | :? CreateUser as c -> UserService(eventStore).Handle(c)
        | :? SetUserPhoto as c -> UserService(eventStore).Handle(c)
        | :? CreateProject as c -> UserService(eventStore).Handle(c)
        | :? CreateUserStoryStatus as c -> ProjectService(eventStore).Handle(c)
        | :? CreateTaskStatus as c -> ProjectService(eventStore).Handle(c)
        | :? AddTaskStatusToUserStoryStatus as c -> ProjectService(eventStore).Handle(c)
        | _ -> raise<unit> (ArgumentException("Command is not registered in dispatcher", "command"))