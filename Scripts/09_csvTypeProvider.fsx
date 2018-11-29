#r "packages/FSharp.Data/lib/net45/FSharp.Data.dll"

open FSharp.Data

type Stocks = CsvProvider< "Resources/MSFT.csv" >

let msft = Stocks.Load("Resources/MSFT.csv")

for row in msft.Rows do
  printfn "HLOC: (%A, %A, %A, %A)" row.High row.Low row.Open row.Close


// --- Charts
#r "packages/FSharp.Charting/lib/net45/FSharp.Charting.dll"

open FSharp.Charting

[ for row in msft.Rows -> row.Date, row.Open ]
|> Chart.FastLine
|> Chart.Show
