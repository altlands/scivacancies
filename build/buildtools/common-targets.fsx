#r @"FAKE\tools\FakeLib.dll"
#r @"AltLanDS.Fake\lib\net45\AltLanDS.Fake.dll"


open Fake
open AltLanDS.Fake.ALtLanBuild
open System
open System.IO
open Fake.OctoTools
open Fake.AssemblyInfoFile
open Fake.AssemblyInfoHelper

Target "CopyToDrop" (fun _ ->
    if (not (directoryExists dropDir)) then CreateDir dropDir
    let dropZipFile = Path.Combine(dropDir,"Release_" + fileVersion + ".zip")
    !! (artifactsDir + "/**/*.*") -- "*.zip" |> Zip artifactsDir dropZipFile
)

Target "Clean" (fun _ -> 
    CleanDirs [baseBuildDir]
)

Target "Start" (fun _ ->
   trace " --- Start --- "
)

Target "Default" (fun _ ->
   trace " --- End --- "
)

Target "Nuget <add_name>" (fun _ ->
    SetReadOnly false (!! (artifactsDir @@ "<add_name>.nuspec"))
    AltLanDS.Fake.NuGetHelper.NuGet (fun p -> 
        {p with                       
            Version = nugetVersion
            Project = "<add_name>"
            WorkingDir = artifactsDir
            OutputPath = artifactsDir            
            PublishUnc=nugetPublishDir
            Publish = true}) 
            (Path.Combine(artifactsDir,"<add_name>.nuspec"))
)

Target "RestoreDeployPackages" (fun _ -> 
     "buildtools/packages.config"      
     |> RestorePackage (fun p ->
         { p with
             Sources = "https://www.myget.org/F/altlan-ds/" :: "https://api.nuget.org/v3/index.json" :: p.Sources
             OutputPath = "./build/deployment"                         
             Retries = 1 })
 )

let buildOctoPackage project =  
    let octo = ExecProcess (fun info -> 
        info.FileName <- "./build/deployment/OctopusTools.2.6.1.52/Octo.exe" 
        info.WorkingDirectory <- "./build/" + project
        info.Arguments <- "pack --id="+project + " --outFolder=.. --version=" + fileVersion
    )
    let result = octo (TimeSpan.FromMinutes 1.0)
    in result

Target "BuildOctoPackages" (fun _ ->
    trace "-- Build Octopus Deploy Package ---"
    let result = buildOctoPackage "SciVacancies.WebApp"
    if result <> 0 then failwithf "Octo.exe returned with a non-zero exit code"    
)

let publishOctoPackage project = 
    NuGetPublish (fun p -> 
        {p with
            AccessKey = "API-RGAPTO3M9TKRPDVUMZZDMNVKQ9E"
            PublishUrl = "http://alt-dev001/nuget/packages"      
            WorkingDir = "./build"                  
            Version = fileVersion
            Project = "../" + "build" + "/" + project
        })

Target "PublishOctoPackages" (fun _ ->
    trace "-- Publish Octo Nugets ---"
    publishOctoPackage "SciVacancies.WebApp"        
)

let OctoRelease () =
    let release = { releaseOptions with 
                        Project = "SciVacancies"
                        PackageVersion = fileVersion }
    let server = { Server = "http://alt-dev001/api"; ApiKey = "API-RGAPTO3M9TKRPDVUMZZDMNVKQ9E" }
    Octo (fun octoParams ->
        { octoParams with
            ToolPath = "./build/deployment/OctopusTools.2.6.1.52"
            Server   = server
            Command  = CreateRelease (release, None) }
    )

let OctoReleaseAndDeploy environment = 
    let release = { 
        releaseOptions with 
            Project = "SciVacancies"
            PackageVersion = fileVersion
            Version = fileVersion
        }
    let deploy  = { deployOptions with 
                        DeployTo = environment
        }
    let server = {Server = "http://alt-dev001/api"; ApiKey = "API-RGAPTO3M9TKRPDVUMZZDMNVKQ9E" }
    Octo (fun octoParams ->
        { octoParams with
            ToolPath = "./build/deployment/OctopusTools.2.6.1.52"
            Server   = server
            Command  = CreateRelease (release, Some deploy) })

Target "ReleaseAndDeployToTest" (fun _ ->
    trace "-- ReleaseAndDeployToTest ---"
    OctoReleaseAndDeploy "TEST"
)

Target "Release" (fun _ ->
    trace "-- Release ---"
    OctoRelease()
)