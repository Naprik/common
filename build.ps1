$nunitrunners = "tools\NUnit.Runners.Net4"
$fxcopbuildtools = "tools\FxCop.BuildTools"
$faketools = "tools\FAKE\tools"
$nowutc = Get-Date
$timestamp = '{0:yyyyMMdd-HHmmss}' -f $nowutc
if(!(Test-Path -Path $nunitrunners )){
    & "tools\nuget\nuget.exe" "install" "NUnit.Runners.Net4" "-Version" "2.6.4" "-OutputDirectory" "tools" "-ExcludeVersion" "-source" "https://www.nuget.org/api/v2;https://api.nuget.org/v3/index.json"
}

if(!(Test-Path -Path $fxcopbuildtools )){
    & "tools\nuget\nuget.exe" "install" "FxCop.BuildTools" "-OutputDirectory" "tools" "-ExcludeVersion" "-source" "https://www.nuget.org/api/v2;https://api.nuget.org/v3/index.json"
}

if(!(Test-Path -Path $faketools )){
	Write-Output "FAKE is not detected thus installing..."
    & "tools\nuget\nuget.exe" "install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion" "-source" "https://www.nuget.org/api/v2;https://api.nuget.org/v3/index.json"
}

if(Test-Path -Path $faketools ){
	Write-Output "Running FAKE..."
	Write-Output "Started: $timestamp"
	Write-Output "Configuration: $($args[0])"
	Write-Output "BuildVersion: $($args[1])"
    & "tools\FAKE\tools\Fake.exe" build.fsx configuration=$($args[0]) version=$($args[1])
	exit $LastExitCode
}