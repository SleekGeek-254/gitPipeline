# Variables
$repositoryUrl = "https://github.com/SleekGeek-254/gitPipeline.git"
$localPath = "E:\testSite\gitPipeline\bin\Debug\net6.0\publish\wwwroot"
$applicationPoolName = "gitPipeline"
$websiteName = "gitPipeline"
$runtimeVersion = "v6.0"

# Clone repository
git clone $repositoryUrl $localPath

# Set up IIS
Import-Module WebAdministration
New-Item IIS:\AppPools\$applicationPoolName -Force
Set-ItemProperty IIS:\AppPools\$applicationPoolName -Name "managedRuntimeVersion" -Value $runtimeVersion
Set-ItemProperty IIS:\AppPools\$applicationPoolName -Name "enable32BitAppOnWin64" -Value $false
New-Item IIS:\Sites\$websiteName -Bindings @{protocol="http";bindingInformation="*:80:"+$websiteName} -PhysicalPath $localPath -ApplicationPool $applicationPoolName
Set-WebConfigurationProperty -Filter /system.webServer/handlers -Value @{name='aspNetAppPool';modules='AspNetCoreModuleV3';path='*';verb='*';preCondition='integratedMode,runtimeVersionv$runtimeVersion'} -PSPath IIS:\ -Location $websiteName

# Configure .NET Core Hosting Bundle
$hostingBundleDownloadUrl = "https://download.visualstudio.microsoft.com/download/pr/10c848d2-4d05-4a94-945d-649c1d8f5688/f28c881b19d30d3a3f8a1c952ed102a3/dotnet-hosting-6.0.0-win.exe"
$hostingBundleFilePath = "C:\temp\dotnet-hosting-6.0.0-win.exe"

Invoke-WebRequest $hostingBundleDownloadUrl -OutFile $hostingBundleFilePath
Start-Process -Wait -FilePath $hostingBundleFilePath -ArgumentList "/install", "/quiet", "/norestart"

# Restart IIS
Restart-Service -Name W3SVC