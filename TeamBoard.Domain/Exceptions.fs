//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the "ExceptionsGenerator.fsx" script.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TeamBoard.Domain

open System

// EntityNotFoundException
type EntityNotFoundException(EntityName:String, EntityId:Guid) = 
    inherit Exception()
    member this.EntityName = EntityName
    member this.EntityId = EntityId
    override this.ToString() = String.Format("Requested entity was not found: [EntityName={0}, EntityId={1}]", EntityName, EntityId)
    override this.Equals other = 
        match other with
        | :? EntityNotFoundException as other -> EntityName = other.EntityName && EntityId = other.EntityId
        | _ -> false
    override this.GetHashCode() = this.ToString().GetHashCode()

// EntityUnhandledEventException
type EntityUnhandledEventException() = 
    inherit Exception()
    override this.ToString() = String.Format("Entity does not containg handler for this event type")

// DuplicateLoginException
type DuplicateLoginException(Login:String) = 
    inherit Exception()
    member this.Login = Login
    override this.ToString() = String.Format("User with such login already exists: [Login={0}]", Login)
    override this.Equals other = 
        match other with
        | :? DuplicateLoginException as other -> Login = other.Login
        | _ -> false
    override this.GetHashCode() = this.ToString().GetHashCode()
