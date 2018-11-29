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


let newToken () = Token <| Guid.NewGuid().ToString()

let adminToken = newToken ()

let log login =
    match login with
    | "admin" -> adminToken
    | _ -> newToken ()

let getUser token =
    if adminToken = token
    then { name = "Jeannine" ; privileges = AllPrivileges }
    else { name = "Jean-Pierre" ; privileges = Restricted }

let getAllSalaries user =
    match user.privileges with
    | Restricted -> []
    | AllPrivileges ->
        [
            { name = "Gontrand" ; salary = 20000 }
            { name = "Huberte" ; salary = 25000 }
            { name = "Tchang-Yves" ; salary = 30000 }
            { name = "Rosa" ; salary = 35000 }
        ]
        

let getAllSalariesFromLogin login =
    let token = log login
    let user = getUser token
    let allSalaries = getAllSalaries user
    allSalaries

getAllSalariesFromLogin "admin"
getAllSalariesFromLogin "chris"

let getAllSalariesFromLogin' login =
    login
    |> log
    |> getUser
    |> getAllSalaries

let getAllSalariesFromLogin''' =
    log >> getUser >> getAllSalaries
