# Variables
$repositoryUrl = "https://github.com/yourusername/yourrepository.git"
$localPath = "C:\inetpub\wwwroot\yourapplication"
$applicationPoolName = "yourapplicationpool"
$websiteName = "yourwebsite"


# Clone repository
git clone $repositoryUrl $localPath


# Set up IIS
Import-Module WebAdministration
New-Item IIS:\AppPools\$applicationPoolName -Force
Set-ItemProperty IIS:\AppPools\$applicationPoolName -Name "managedRuntimeVersion" -Value "v4.0"
Set-ItemProperty IIS:\AppPools\$applicationPoolName -Name "enable32BitAppOnWin64" -Value $false
New-Item IIS:\Sites\$websiteName -Bindings @{protocol="http";bindingInformation="*:80:"+$websiteName} -PhysicalPath $localPath -ApplicationPool $applicationPoolName
