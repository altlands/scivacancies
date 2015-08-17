pushd %~dp0
pushd ..
powershell buildtools\build.ps1 -MainVersion 1.0 -BuildNumber 69 -BuildScript "buildtools\releaseToOctopus.fsx"
popd
popd