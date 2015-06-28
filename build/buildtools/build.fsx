#r @"FAKE\tools\FakeLib.dll"
#r @"AltLanDS.Fake\lib\net45\AltLanDS.Fake.dll"

// 
open Fake
open AltLanDS.Fake.ALtLanBuild
open Fake.ProcessHelper
open System
open System.IO

#load "common-targets.fsx"

Target "Build" (fun _ ->    
    trace "-- Build ---"
    let octo = ExecProcess (fun info -> 
        info.FileName <- "buildtools\\build.bat" 
    )
    let result = octo (TimeSpan.FromMinutes 1.0)
    if result <> 0 then failwithf "brrrr" 

    //CopyDir (Path.Combine(artifactsDir, "AltLanDS.Beeline.IdentityServer.Database/up")) "AltLanDS.Beeline.IdentityServer.Database/up" (fun _ -> true)
)

"Start"
    ==> "Clean"
    ==> "Build"   
    ==> "RestoreDeployPackages"
    ==> "BuildOctoPackages"            
    ==> "Default"
 
RunTargetOrDefault "Default"


