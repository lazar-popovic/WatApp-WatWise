``<h1>WattApp</h1>
<h2>About</h2>
<p>
This repository contains a web application built using ASP.NET Core 7 for the backend and Angular 15 for the frontend. The application uses a SQLite and a MongoDB database. This readme file provides instructions on how to run the application on a local machine.
</p>

<h2>Required technologies</h2>
<ul>
<li>.NET 7 SDK</li>
<li>Angular CLI (version 15 or later)</li>
<li>SQLite3</li>
<li>MongoDB Server</li>
</ul>

<h2>Running web application on Local</h2>

<p>Clone the repository to your local machine.</p>

<h3>Running MongoDB server</h3>
<ol>
<li>Run MongoDB server.</li>
<li>Set up MongoDB server connection in appsettings.json file in .NET project
<b><br>"ConnectionStrings": {<br>
        "MongoDB": "mongodb://localhost:27017"<br>
  }<br></b>
</li>
<li>Create database named <b>database</b> with collection called <b>devices</b></li>
<li>Import data manually as json file or run the python script in folder <b>src/python/devices_scripts</b> to fill collection with necessary data</li>
</ol>

<h3>Running .NET webapi</h3>
<ol>
<li>Open a terminal and navigate to the <b>src/back/API</b> folder.</li>
<li>Run the following command to install the required NuGet packages:<br><b>dotnet restore</b></li>
<li>Set up SQLite database connection in appsettings.json file in .NET project
<b><br>"ConnectionStrings": {<br>
     "DefaultConnection": "Data Source = database.db"<br>
  }<br></b>
<li>Run the following command to create SQLite database based of migrations:<br><b>dotnet ef database update</b></li>
<li>Run the following commands to start the backend server:<br><b>dotnet run</b></li>
<li>Server is located on http://localhost:5226/ web address</li>
</ol>

<h3>Running Angular application</h3>
<ol>
<li>Open a terminal and navigate to the <b>src/front/client</b> folder.</li>
<li>Run the following commands to install the required Node.js packages:<br><b>npm install</b></li>
<li>Run the following commands to start the frontend server:<br><b>ng serve</b></li>
<li>Navigate to http://localhost:4200/. The application will automatically reload if you change any of the source files.</li>
</ol>



