# Enable -Verbose option
[CmdletBinding()]
	
# Disable parameter
# Convenience option so you can debug this script or disable it in 
# your build definition without having to remove it from
# the 'Post-build script path' build process parameter.
param(
[switch]$Disable,
[switch]$LocalBuild,
[string]$MainVersion,
[string]$BuildNumber,
[string]$PrereleaseStage,
[string]$BuildScript = "buildtools\build.fsx",
[string]$BuildTarget = "Default",
[string]$DropDir = "./drop",
[string]$BuildBaseDir = "",
[string]$NugetPublishDir = "C:/temp/NugetPackages",
[string]$NugetConfig = "buildtools\NuGet.Config",
[string[]]$AdditionalParams
)

$path  = split-path $SCRIPT:MyInvocation.MyCommand.Path -parent
set-location $path
set-location -path ..

if ($PSBoundParameters.ContainsKey('Disable'))
{
	Write-Verbose "Script disabled; no actions will be taken on the files."
	exit 1
}

if (-not $MainVersion)
{
	Write-Error "Main Version is not defined. Stopping Build."
	exit 1
}

if (-not $PSBoundParameters.ContainsKey('$LocalBuild') -and -not $Env:TF_BUILD)
{
	$LocalBuild = $true
}

if ($LocalBuild) 
{
	Write-Host "Running Local Build"
	
	if (-not $BuildNumber)
	{
		Write-Error "Build Number is not defined. Stopping Build."
		exit 1
	}
}
else
{
	Write-Host "Running TFS Build"

	# Make sure path to source code directory is available
	if (-not $Env:TF_BUILD_SOURCESDIRECTORY)
	{
		Write-Error ("TF_BUILD_SOURCESDIRECTORY environment variable is missing.")
		exit 1
	}
	elseif (-not (Test-Path $Env:TF_BUILD_SOURCESDIRECTORY))
	{
		Write-Error "TF_BUILD_SOURCESDIRECTORY does not exist: $Env:TF_BUILD_SOURCESDIRECTORY"
		exit 1
	}

	Write-Verbose "TF_BUILD_SOURCESDIRECTORY: $Env:TF_BUILD_SOURCESDIRECTORY"

	if (-not $BuildNumber)
	{
		$BuildNumber = $Env:TF_BUILD_BUILDNUMBER		
	}

	#Fix bug with buildnumber
	$BuildNumber = $Env:TF_BUILD_BUILDNUMBER.TrimStart($Env:TF_BUILD_BUILDDEFINITIONNAME + "_")

	# Make sure there is a build number
	if (-not $BuildNumber)
	{		
		Write-Error ("Build Number or TF_BUILD_BUILDNUMBER environment variable is not defined. Stopping Build.")
		exit 1
	}

	if ($Env:TF_BUILD_DROPLOCATION)
	{
		$DropDir = $Env:TF_BUILD_DROPLOCATION
	}
}


if (-not (Test-Path "buildtools\FAKE"))
{
	buildtools\nuget.exe install FAKE -Version 2.6.5 -OutputDirectory buildtools -ExcludeVersion -ConfigFile $NugetConfig
}

if (-not (Test-Path "buildtools\FAKE"))
{
	Write-Error ("Couldn't install FAKE. Stopping Build.")
	exit 1
}

if (-not (Test-Path "buildtools\AltLanDS.Fake"))
{
	buildtools\nuget.exe install AltLanDS.Fake -OutputDirectory buildtools -ExcludeVersion -ConfigFile $NugetConfig
}

#if (-not (Test-Path "buildtools\AltLanDS.Fake"))
#{
#	Write-Error ("Couldn't install AltLanDS.Fake. Stopping Build.")
#	exit 1
#}

Write-Host "Main Version: " $MainVersion
Write-Host "Build Number: " $BuildNumber

buildtools\nuget.exe restore -ConfigFile $NugetConfig

buildtools\FAKE\tools\FAKE.exe $BuildScript $BuildTarget versionAssembly versionNumber=$MainVersion buildNumber=$BuildNumber dropDir=$DropDir nugetPublishDir=$NugetPublishDir $AdditionalParams

if(-not ($?))
{
	Write-Error ("FAKE build failed")
	exit 1
}