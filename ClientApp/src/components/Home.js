import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
        <h1> gitPipeline.io !</h1>
        <p>Welcome to your first gitPipeline.io , built with:</p>
        <ul>
          <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
          <li><a href='https://facebook.github.io/react/'>React</a> for client-side code</li>
          <li><a href='http://getbootstrap.com/'>Bootstrap</a> for layout and styling</li>
        </ul>
        <p>To help you get started, we have also set up:</p>
          <div style={{display: 'flex', flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center'}}>
      <div style={{width: '25%', backgroundColor: '#F8F8F8', padding: '20px'}}>
        <h2>Builds</h2>
        <p>Number of builds: 10</p>
        <p>Success rate: 80%</p>
      </div>
      <div style={{width: '25%', backgroundColor: '#F8F8F8', padding: '20px'}}>
        <h2>Tests</h2>
        <p>Number of tests: 20</p>
        <p>Success rate: 90%</p>
      </div>
      <div style={{width: '25%', backgroundColor: '#F8F8F8', padding: '20px'}}>
        <h2>Deployments</h2>
        <p>Number of deployments: 5</p>
        <p>Success rate: 100%</p>
      </div>
    </div>
 </div>
    );
  }
}
