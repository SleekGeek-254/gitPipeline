# gitPipeline


deploy your ASP.NET API to a remote IIS server using Git Actions:

Step 1: Set up the remote IIS server

You will need to have a remote IIS server set up and ready to receive the deployment. Make sure that you have the appropriate permissions and access to the server. You will also need to have the necessary software installed on the server, including Git and the .NET framework.

Step 2: Create the deployment script

Create a PowerShell script that will clone the Git repository to the IIS server and perform any necessary configuration or setup. Here is an example script that you can modify to suit your needs:

Bash

# Variables
$repositoryUrl = "https://github.com/yourusername/yourrepository.git"
$localPath = "C:\inetpub\wwwroot\yourapplication"
$applicationPoolName = "yourapplicationpool"
$websiteName = "yourwebsite"
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

This script will:
Clone the Git repository to the specified local path
Create an application pool and website in IIS, and configure the website to use the application pool
Set the managed runtime version to .NET 6.0
Configure the ASP.NET Core module to use the correct runtime version
Install the .NET Core Hosting Bundle for .NET 6.0 (if it isn't already installed)
Restart the IIS service to apply the changes
You would need to modify the $repositoryUrl, $localPath, $applicationPoolName, and $websiteName variables to match your specific deployment scenario. Additionally, you may need to modify the $runtimeVersion variable if you are using a different version of .NET 6.0.

Save the script as "deploy.ps1" in your project's root directory.

Step 3: Create the deployment workflow

Create a deployment workflow that will run the deployment script on the remote IIS server. Here is an example workflow that you can modify to suit your needs:

Yaml

name: Deploy to IIS


on:
  push:
    branches:
      - main


jobs:
  deploy:
    runs-on: windows-latest
    steps:
    - uses: actions/checkout@v2
    - name: Run deployment script
      uses: jexuscorp/iis-web-app-deploy@v1.0.0
      with:
        sourcePath: '.'
        webAppName: 'gitPipeline'
        iisWebSiteName: 'gitPipeline'
        iisPhysicalPath: 'E:\testSite\gitPipeline\bin\Debug\net6.0\publish\wwwroot'
        appPoolName: 'gitPipeline'
        extraAppcmd: 'set config -section:system.webServer/security/authentication/anonymousAuthentication /enabled:true'



This workflow will trigger on a push to the main branch, and will run on a Windows machine. It will check out the code from the repository, and then run the deployment script using the "jexuscorp/iis-web-app-deploy" action.

You will need to replace "yourwebsite" and "yourapplicationpool" with the appropriate names for your website and application pool. You will also need to replace the "username" and "password" values with the appropriate credentials for accessing the remote IIS server. These values should be stored as secrets in your repository.

Save the workflow as ".github/workflows/deploy.yml" in your project's root directory.



Step 4: Push the changes and trigger the deployment

Commit and push the changes to your repository. This will trigger the deployment workflow, which will

What you push your changes to the repository:

Git Actions will detect the push event on the main branch and start the deployment workflow.

The workflow will run on a Windows machine, specified by the "runs-on" field in the workflow file.

The workflow will check out the code from the repository using the "actions/checkout" action.

The "jexuscorp/iis-web-app-deploy" action will be invoked, which will:

a. Use the "sourcePath" parameter to specify the current directory as the source of the web application code.

b. Use the "webAppName" parameter to specify the name of the web application that will be created in IIS.

c. Use the "iisWebSiteName" parameter to specify the name of the IIS website that the web application will be deployed to. In this case, the website is the default website, "Default Web Site".

d. Use the "iisPhysicalPath" parameter to specify the physical path of the IIS website on the remote server. In this case, it is "C:\inetpub\wwwroot".

e. Use the "username" and "password" parameters to specify the credentials for accessing the remote IIS server. These values are stored as secrets in the repository.

f. Use the "appPoolName" parameter to specify the name of the application pool that the web application will be associated with.

g. Use the "extraAppcmd" parameter to specify any additional AppCmd commands that need to be run during deployment. In this case, it enables Windows authentication in the web application.

The deployment script will run on the remote IIS server, and perform the following actions:

a. Clone the Git repository to the specified local path.

b. Create an application pool with the specified name.

c. Configure the application pool to use the .NET framework version 4.0, and disable 32-bit applications.

d. Create a website with the specified name and bindings, and associate it with the specified application pool.

The deployment is complete, and the web application is now running on the remote IIS server.
