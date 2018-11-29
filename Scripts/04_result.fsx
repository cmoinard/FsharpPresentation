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

type Result<'value, 'error> =
| Success of 'value
| Failure of 'error

type Error =
| UnknownLogin
| MissingUser of string
| RestrictedPrivileges

let newToken () = Token <| Guid.NewGuid().ToString()

let adminToken = newToken ()

let stagiaireToken = newToken ()

let log login =
    match login with
    | "" -> Failure UnknownLogin
    | "admin" -> Success adminToken
    | _ -> Success <| newToken ()

let getUser token =
    if stagiaireToken = token
    then Failure <| MissingUser "stagiaire inconnu"
    else
        Success <|
            if adminToken = token
            then { name = "Jeannine" ; privileges = AllPrivileges }
            else { name = "Jean-Pierre" ; privileges = Restricted }

let getAllSalaries user =
    match user.privileges with
    | Restricted -> Failure RestrictedPrivileges
    | AllPrivileges ->
        Success <|
            [
                { name = "Gontrand" ; salary = 20000 }
                { name = "Huberte" ; salary = 25000 }
                { name = "Tchang-Yves" ; salary = 30000 }
                { name = "Rosa" ; salary = 35000 }
            ]
                        
            
let getAllSalariesFromLogin' login =
    let token = log login
    match token with
    | Failure err -> Failure err
    | Success t ->
        let user = getUser t
        match user with
        | Failure err -> Failure err
        | Success u ->
            let allSalaries = getAllSalaries u
            allSalaries

let whenSuccess f v =
    match v with
    | Failure err -> Failure err
    | Success x -> f x

let getAllSalariesFromLogin'' login =
    login
    |> log
    |> whenSuccess getUser
    |> whenSuccess getAllSalaries
    
let (>>=) v f = whenSuccess f v

let getAllSalariesFromLogin'''' login =
    login
    |> log
    >>= getUser
    >>= getAllSalaries

type ResultBuilder() =
    member __.Bind(x, f) = whenSuccess f x

    member __.Return(x) = Success x

let result = ResultBuilder()

let getAllSalariesFromLogin'''''' login =
    result {
        let! token = log login
        let! user = getUser token
        let! allSalaries = getAllSalaries user
        return allSalaries
}