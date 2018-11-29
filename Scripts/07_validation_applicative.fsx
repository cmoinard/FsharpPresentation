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

let map f v =
    match v with
    | Invalid errs -> Invalid errs
    | Valid x -> Valid (f x)

let (<!>) = map

let apply f v =
    match f, v with
    | Invalid errsF, Invalid errsV -> Invalid <| List.append errsF errsV
    | Invalid errs, _ -> Invalid errs
    | _, Invalid errs -> Invalid errs
    | Valid f', Valid v' -> Valid <| f' v'

let (<*>) = apply


createEmail
    <!> validateDest "toto@tata.com"
    <*> validateSubject "toto"
    <*> validateContent "bonjour"

createEmail
    <!> validateDest ""
    <*> validateSubject ""
    <*> validateContent ""