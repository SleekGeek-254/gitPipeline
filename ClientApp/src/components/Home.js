import React, { Component } from 'react';

const containerStyle = {
  padding: '20px',
  backgroundColor: '#F4F4F4',
  borderRadius: '5px',
  boxShadow: '0px 0px 5px 1px #C7C7C7',
};

const titleStyle = {
  marginBottom: '20px',
  fontSize: '28px',
  fontWeight: 'bold',
};

const summaryStyle = {
  marginBottom: '20px',
  fontSize: '16px',
};

const listItemStyle = {
  marginBottom: '10px',
  fontSize: '14px',
  listStyleType: 'decimal',
  marginLeft: '20px',
};

const preStyle = {
  backgroundColor: '#FFFFFF',
  color: '#000000',
  padding: '20px',
  overflowX: 'scroll',
};

export class Home extends Component {
  static displayName = Home.name;

  

  render() {
    return (
      <div>
        <h1>gitPipeline.io</h1>
        <p>Welcome to your gitPipeline application, built with:</p>
        <ul>
          <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
          <li><a href='https://facebook.github.io/react/'>React</a> for client-side code</li>
          <li><a href='http://getbootstrap.com/'>Bootstrap</a> for layout and styling</li>
        </ul>



        <p>To help you get started,see the Dashboard for more info:</p>
        
        <div style={{display: 'flex', flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center'}}>
          <div style={{width: '25%', backgroundColor: '#F8F8F8', padding: '20px'}}>
            <h2>Builds</h2>
            <p>Number of builds: --</p>
            <p>Success rate: --%</p>
          </div>
          <div style={{width: '25%', backgroundColor: '#F8F8F8', padding: '20px'}}>
            <h2>Tests</h2>
            <p>Number of tests: --</p>
            <p>Success rate: --%</p>
          </div>
          <div style={{width: '25%', backgroundColor: '#F8F8F8', padding: '20px'}}>
            <h2>Deployments</h2>
            <p>Number of deployments: --</p>
            <p>Success rate: --%</p>
          </div>
        </div>

        <div style={containerStyle}>
              <h2 style={titleStyle}>TL: DR;</h2>
              <p style={summaryStyle}>Set of steps to build and deploy a .NET Core app to an IIS server using GitHub Actions. Here is a summary of the steps:</p>
              <ol>
                <li style={listItemStyle}>Use the command "dot net new… ||Asp.net ||react" to create a new ASP.NET React application.</li>
                <li style={listItemStyle}>Set up IIS on your server.</li>
                <li style={listItemStyle}>Install the .NET hosting bundle on your server.*</li>
                <li style={listItemStyle}>Create a new GitHub Actions runner using the link https://github.com/&lt;user&gt;/&lt;repo&gt;/settings/actions/runners/</li>
                <li style={listItemStyle}>In your GitHub repository, create a new workflow named "deploy-lis.yml".</li>
                <li style={listItemStyle}>Define the workflow trigger to run when a push is made to the master branch.</li>
                <li style={listItemStyle}>Define environment variables for the publish directory and website directory.</li>
                <li style={listItemStyle}>Define a job named "build-and-deploy" that runs on a Windows latest runner.</li>
                <li style={listItemStyle}>Define steps to checkout the code, set up .NET Core, build the project, publish the project, stop the IIS server, copy the files to the website directory, and start the IIS server.</li>
              </ol>
              <p style={summaryStyle}>By following these steps, you should be able to automate the deployment of your .NET Core app to an IIS server using GitHub Actions.</p>
              <pre style={preStyle}>
                workflows &gt; m deploy-lis.yml
                name: Build and deploy .NET Core app to IIS

                on:
                  push:
                    branches:
                      - master

                env:
                  PUBLISH_DIR: C:\actions-runner\_work\gitPipeline\gitPipeline\myapp
                  WEBSITE_DIR: E:\testSite\gitPipeline\bin\Debug\net6.0\publish

                jobs:
                  build-and-deploy:
                    runs-on: self-hosted
                    steps:
                      # Checkout code
                      - name: Checkout code
                        uses: actions/checkout@v2

                      # Setup .NET Core
                      - name: Setup .NET Core
                        uses: actions/setup-dotnet@v1
                        with:
                          dotnet-version: '6.0.x'

                      # Build with dotnet
                      - name: Build with dotnet
                        shell: cmd
                        run: dotnet build -c Release

                      # Publish with dotnet
                      - name: Publish with dotnet
                        shell: cmd
                        run: dotnet publish -c Release -o $ env.PUBLISH_DIR 

                      # Stop IIS server
                      - name: Stop IIS server
                        shell: cmd
                        run: iisreset /stop

                      # Copy files to website directory
                      - name: Copy files to website directory
                        shell: cmd
                        run: xcopy /s /y $ env.PUBLISH_DIR \* $ env.WEBSITE_DIR 

                      # Start IIS server
                      - name: Start IIS server
                        shell: cmd
                        run: iisreset /start
              </pre>
          
            </div>
                
      
      </div>
    );
  }
}
