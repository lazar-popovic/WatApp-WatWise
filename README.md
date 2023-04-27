<h1>WattApp</h1>
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

<h2>Running web application on Local machine</h2>

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
<li>Run the following command to create SQLite database based of migrations:<br><b>dotnet ef database update</b>.</li>
<li>Run the following commands to start the backend server:<br><b>dotnet run</b></li>
<li>Server is located on http://localhost:5226/ web address.</li>
<li>To build project, execute <b>dotnet build</b>. This command builds a .NET application or project by compiling the source code and producing the output files based on the configuration specified in the project file.</li>
<li>When you want to publish project on server, execute <b>dotnet publish</b>. This command builds and publishes a .NET application or project, producing a self-contained deployment package that can be executed on a target machine without requiring the .NET SDK.</li>
</ol>

<h3>Running Angular application</h3>
<ol>
<li>Open a terminal and navigate to the <b>src/front/client</b> folder.</li>
<li>Run the following commands to install the required Node.js packages:<br><b>npm install</b></li>
<li>Run the following commands to start the frontend server:<br><b>ng serve</b>.</li>
<li>Navigate to http://localhost:4200/. The application will automatically reload if you change any of the source files.</li>
<li>When you want to build project, execute <b>ng build</b>. This command builds an Angular application, compiling the TypeScript code into JavaScript and creating a production-ready package that can be deployed to a web server.</li>
</ol>

<h2>Running web application on Web server</h2>

<p>Install RemoteSSH extention for easier configuration of application</p>
<p>Connect to ssh remote server</p>

<h3>Running MongoDB server on remote</h3>
<ol>
<li>Create folder /mongo on remote server.</li>
<li>To start MongoDb server on remote, run command <b>mongod --dbpath path/to/mongo/folder --port MONGO_PORT --bind_all</b>.</li>
<li>You can connect to connection using MongoDbCompass by connecting to <b>mongodb://website-address:MONGO_PORT/</b>.</li>
</ol>

<h3>Running .NET project on remote</h3>
<ol>
<li>Set up MongoDB server connection in appsettings.json file in .NET project that was set above
<b><br>"ConnectionStrings": {<br>
        "MongoDB": "mongodb://website-address:MONGO_PORT/"<br>
  }<br></b>
</li>
<li>Publish application on your machine using <b>dotnet publish</b>, as said above.</li>
<li>On remote, create folder <b>/back</b> and copy the content of <b>publish</b> folder into it.</li>
<li>Set port of application by running command <b>export ASPNETCORE_URLS="http:///website-address:API_PORT"</b>.</li>
<li>Start .NET application by running <b>dotnet back/API.dll</b>.</li>
<li>Your webapi is now active.</li>
</ol>

<h3>Running Angular project on remote</h3>
<ol>
<li>Set up API server connection in <b>enviroment.ts</b> file in Angular project that was set above
<b><br>apiUrl: "http:///website-address:API_PORT"</b>
</li>
<li>Build application on your machine using <b>ng build</b>, as said above.</li>
<li>Download and extract <b>[server.rar](https://cdn.discordapp.com/attachments/1100106229482786848/1100110066000081007/server.rar)</b> that is necessary for running ng application.</li>
<li>Extract folder <b>server.rar</b> and copy content from folder <b>dist</b> into it. <b>dist</b> folder contains builded Angular application.</li>
<li>On remote, create folder <b>/front</b> and copy the content of <b>server</b> folder into it.</li>
<li>Start Angular application by navigating to folder <b>/front</b> and running command <b>node index.js</b>.</li>
<li>Your webapi is now active.</li>
</ol>

<h2>Informations for testers</h2>

<p>Application is running on url <b>[WattApp](http://softeng.pmf.kg.ac.rs:10082/)</b>
<p>Accounts for accessing application</p>
<ul>
  <li>admin@admin.com - admin123</li>
  <li>employee@employee.com - epmloyee123</li>
  <li>prosumer@prosumer.com - prosumer123</li>
</ul>
