//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the "EventsGenerator.fsx" script.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TeamBoard.Events

open System
open System.Runtime.Serialization

[<DataContract>]
type UserCreated = 
    {
        [<field: DataMember(Name = "Id@")>]
        Id: Guid

        [<field: DataMember(Name = "Login@")>]
        Login: String

        [<field: DataMember(Name = "Password@")>]
        Password: String

        [<field: DataMember(Name = "Name@")>]
        Name: String
    }
    interface IAggregateEvent with member this.Id = this.Id

[<DataContract>]
type UserPhotoSet = 
    {
        [<field: DataMember(Name = "Id@")>]
        Id: Guid

        [<field: DataMember(Name = "Photo@")>]
        Photo: Byte[]
    }
    interface IAggregateEvent with member this.Id = this.Id

[<DataContract>]
type ProjectCreated = 
    {
        [<field: DataMember(Name = "Id@")>]
        Id: Guid

        [<field: DataMember(Name = "Caption@")>]
        Caption: String
    }
    interface IAggregateEvent with member this.Id = this.Id

[<DataContract>]
type TaskStatusCreated = 
    {
        [<field: DataMember(Name = "Id@")>]
        Id: Guid

        [<field: DataMember(Name = "Caption@")>]
        Caption: String
    }
    interface IAggregateEvent with member this.Id = this.Id

[<DataContract>]
type UserStoryStatusCreated = 
    {
        [<field: DataMember(Name = "Id@")>]
        Id: Guid

        [<field: DataMember(Name = "Caption@")>]
        Caption: String
    }
    interface IAggregateEvent with member this.Id = this.Id

[<DataContract>]
type TaskStatusToUserStoryStatusAdded = 
    {
        [<field: DataMember(Name = "Id@")>]
        Id: Guid

        [<field: DataMember(Name = "UserStoryStatusPosition@")>]
        UserStoryStatusPosition: Int32

        [<field: DataMember(Name = "TaskStatusPosition@")>]
        TaskStatusPosition: Int32
    }
    interface IAggregateEvent with member this.Id = this.Id

[<DataContract>]
type TeamMemberAdded = 
    {
        [<field: DataMember(Name = "Id@")>]
        Id: Guid

        [<field: DataMember(Name = "CreatorId@")>]
        CreatorId: Guid

        [<field: DataMember(Name = "Role@")>]
        Role: TeamMemberRole
    }
    interface IAggregateEvent with member this.Id = this.Id
