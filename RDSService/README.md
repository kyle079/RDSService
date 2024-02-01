# RDS Service

This project presents a RESTFul API for RDS (Remote Desktop Services) management. 
It is designed to run on IIS (Internet Information Services) and is written in C# using the .NET 7 Framework.

## Features

- [x] Get a list of all active RDS sessions
- [x] Disconnect a specific RDS session
- [x] Logoff a specific RDS session

## Installation

1. Clone the repository
2. Open the solution in IDE
3. Build the solution
4. Publish the project to a folder
5. Copy the published files to the desired location on the server
6. Create a new IIS site and point it to the published folder
7. Configure the site to use a specific port and SSL certificate