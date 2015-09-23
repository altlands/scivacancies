pushd %~dp0
pushd ..
powershell buildtools\build.ps1 -MainVersion 1.0 -BuildNumber 101 -BuildScript "buildtools\releaseToOctopus.fsx"
popd
popd