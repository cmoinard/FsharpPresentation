#r "packages/SQLProvider/lib/net451/FSharp.Data.SqlProvider.dll"

open System
open FSharp.Data.Sql
open System.Data

let [<Literal>] connectionString = 
    "Data Source=" + 
    __SOURCE_DIRECTORY__ + @"/northwindEF.db;" + 
    "Version=3;foreign keys=true"
    

let [<Literal>] resolutionPath = 
    __SOURCE_DIRECTORY__ + 
    @"\Dlls"

type sql = SqlDataProvider<
                Common.DatabaseProviderTypes.SQLITE, 
                SQLiteLibrary = Common.SQLiteLibrary.SystemDataSQLite,
                ConnectionString = connectionString, 
                ResolutionPath = resolutionPath, 
                CaseSensitivityChange = Common.CaseSensitivityChange.ORIGINAL>


// open FSharp.Data.Sql

// let [<Literal>] connectionString = @"DataSource=" + __SOURCE_DIRECTORY__ + @"\northwindEF.db;Version=3"
// let [<Literal>] resolutionPath = __SOURCE_DIRECTORY__ + @"packages\System.Data.SQLite.Core\lib\net451\System.Data.SQLite.dll"

// type sql = SqlDataProvider< 
//               ConnectionString = connectionString,
//               DatabaseVendor = Common.DatabaseProviderTypes.SQLITE,
//               ResolutionPath = resolutionPath,
//               IndividualsAmount = 1000,
//               UseOptionTypes = true >