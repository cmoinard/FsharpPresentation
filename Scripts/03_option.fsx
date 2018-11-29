open System

type Token = Token of string

type Privileges =
    | AllPrivileges
    | Restricted

type User = {
    name: string
    privileges: Privileges
}

type Employee = {
    name: string
    salary: int
}

(*
type Option<'a> =
| Some of 'a
| None
*)

let newToken () = Token <| Guid.NewGuid().ToString()

let adminToken = newToken ()

let stagiaireToken = newToken ()

let log login =
    match login with
    | "" -> None
    | "admin" -> Some adminToken
    | _ -> Some <| newToken ()

let getUser token =
    if stagiaireToken = token
    then None
    else
        Some (
            if adminToken = token
            then { name = "Jeannine" ; privileges = AllPrivileges }
            else { name = "Jean-Pierre" ; privileges = Restricted })

let getAllSalaries user =
    match user.privileges with
    | Restricted -> None
    | AllPrivileges ->
        Some (
            [
                { name = "Gontrand" ; salary = 20000 }
                { name = "Huberte" ; salary = 25000 }
                { name = "Tchang-Yves" ; salary = 30000 }
                { name = "Rosa" ; salary = 35000 }
            ])            
            
let getAllSalariesFromLogin login =
    let token = log login
    match token with
    | None -> None
    | Some t ->
        let user = getUser t
        match user with
        | None -> None
        | Some u ->
            let allSalaries = getAllSalaries u
            allSalaries

getAllSalariesFromLogin ""
getAllSalariesFromLogin "toto"
getAllSalariesFromLogin "admin"

let whenSome f v =
    match v with
    | None -> None
    | Some x -> f x

let getAllSalariesFromLogin'' login =
    login
    |> log
    |> whenSome getUser
    |> whenSome getAllSalaries

let getAllSalariesFromLogin''' login =
    login
    |> log
    |> Option.bind getUser
    |> Option.bind getAllSalaries

let (>>=) v f =
    match v with
    | None -> None
    | Some x -> f x

let getAllSalariesFromLogin'''' login =
    login
    |> log
    >>= getUser
    >>= getAllSalaries

type OptionBuilder() =
    member __.Bind(x, f) = Option.bind f x

    member __.Return(x) = Some x

let opt = OptionBuilder()

let getAllSalariesFromLogin'''''' login =
    opt {
        let! token = log login
        let! user = getUser token
        let! allSalaries = getAllSalaries user
        return allSalaries
    }