#r "packages/FSharp.Text.RegexProvider/lib/net40/FSharp.Text.RegexProvider.dll"

open FSharp.Text.RegexProvider

type DateProvider = Regex< @"(?<day>\d\d)/(?<month>\d\d)/(?<year>\d{4})" >

let date = DateProvider().TypedMatch("10/04/1985")

printfn
    "%s-%s-%s"
    date.day.Value
    date.month.Value
    date.year.Value
