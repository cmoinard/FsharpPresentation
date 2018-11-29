open System
open System.Threading.Tasks

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
    Task.Delay(20)
        .ContinueWith(fun _ -> 
            match login with
            | "admin" -> adminToken
            | _ -> newToken ()
        )

let getUser token =  
    Task.Delay(20)
        .ContinueWith(fun _ -> 
            if adminToken = token
            then { name = "Jeannine" ; privileges = AllPrivileges }
            else { name = "Jean-Pierre" ; privileges = Restricted }
        )

let getAllSalaries user =
    Task.Delay(20)
        .ContinueWith(fun _ -> 
            match user.privileges with
            | Restricted -> []
            | AllPrivileges ->
                [
                    { name = "Gontrand" ; salary = 20000 }
                    { name = "Huberte" ; salary = 25000 }
                    { name = "Tchang-Yves" ; salary = 30000 }
                    { name = "Rosa" ; salary = 35000 }
                ]
        )

let getAllSalariesFromLogin login =
    let token = log login
    token.ContinueWith(fun (t:Task<Token>) -> 

        let user = getUser t.Result
        user.ContinueWith(fun (u:Task<User>) -> 

            let allSalaries = getAllSalaries u.Result
            allSalaries

        ).Unwrap()
    ).Unwrap()

getAllSalariesFromLogin ""
getAllSalariesFromLogin "toto"
getAllSalariesFromLogin "admin"

let inline continueWith (f: 'a -> Task<'b>) (v: Task<'a>) =
    v.ContinueWith(fun (u:Task<'a>) ->
        f u.Result
        ).Unwrap()

let getAllSalariesFromLogin'' login =
    login
    |> log
    |> continueWith getUser
    |> continueWith getAllSalaries

let (>>=) v f = continueWith f v

let getAllSalariesFromLogin'''' login =
    login
    |> log
    >>= getUser
    >>= getAllSalaries

type TaskBuilder() =
    member __.Bind(x, f) = continueWith f x

    member __.Return(x) = Task.FromResult(x)

let task = TaskBuilder()

let getAllSalariesFromLogin'''''' login =
    task {
        let! token = log login
        let! user = getUser token
        let! allSalaries = getAllSalaries user
        return allSalaries
    }