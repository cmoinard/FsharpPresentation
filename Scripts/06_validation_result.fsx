open System

type Email = {
    dest: string
    subject: string
    content: string
}

let createEmail dest subject content =
    { dest = dest
      subject = subject
      content = content }

createEmail "toto@tata.com" "title" "hello"

type Validation<'a> =
    | Valid of 'a
    | Invalid of string list

let isRequired label s =
    if String.IsNullOrWhiteSpace(s) |> not
    then Valid s
    else Invalid [ label + " is required"]

let mustContain (c:string) label (s: string) =
    if s.Contains(c)
    then Valid s
    else Invalid [ label + " must contains " + c ]

let maxLength length label (s: string) =
    if s.Length <= length
    then Valid s
    else Invalid [ label + " must not exceed " + (length |> string) + " characters" ]

let (&&&) rule1 rule2 v =
    match rule1 v, rule2 v with
    | Invalid errs1, Invalid errs2 -> Invalid <| List.append errs1 errs2
    | Invalid errs, _ -> Invalid errs
    | _, Invalid errs -> Invalid errs
    | _ -> Valid v

let validateDest =
    isRequired "dest" &&&
    maxLength 30 "dest" &&&
    mustContain "@" "dest"

let validateSubject =
    isRequired "subject" &&&
    maxLength 100 "subject"

let validateContent =
    isRequired "content" &&&
    maxLength 1000 "content"

let bind f v =
    match v with
    | Invalid errs -> Invalid errs
    | Valid x -> f x

let return' v = Valid v

let (>>=) v f = bind f v

let createValidatedEmail dest subject content =
    validateDest dest >>= (fun d ->
        validateSubject subject >>= (fun s ->
            validateContent content >>= (fun c ->
                return' <| createEmail d s c
            )
        )
    )

type ValidationBuilder() =
    member __.Bind(v, f) = bind f v
    member __.Return(v) = return' v

let valid = ValidationBuilder()

let createValidatedEmail' dest subject content =
    valid {
        let! d = validateDest dest
        let! s = validateSubject subject
        let! c = validateContent content
        return createEmail d s c
    }

createValidatedEmail'
    "toto@tata.com"
    "toto"
    "bonjour"