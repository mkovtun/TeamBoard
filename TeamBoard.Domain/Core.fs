namespace TeamBoard.Domain

open TeamBoard.Events

module Core =
    let fire o =        
        [o :> IEvent]

module Utils =
    let ApplyToNth list number func = 
        let rec donth = function
            | [] -> (0, [])
            | xs :: tail -> donth tail |> fun (x, y) -> x+1, (if x = number then func xs else xs) :: y
        donth list |> fun (_, tail) -> tail

    let DeleteNth list number = 
        let rec donth = function
            | [] -> (0, [])
            | xs :: tail -> donth tail |> fun (x, y) -> x+1, (if x = number then y else xs :: y)
        
        donth list |> fun (_, tail) -> tail