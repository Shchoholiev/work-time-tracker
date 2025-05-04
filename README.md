# work-time-tracker

A simple API application designed to help small companies track employee work hours efficiently.

## Table of Contents

- [Features](#features)
- [Stack](#stack)
- [Installation](#installation)
  - [Prerequisites](#prerequisites)
  - [Setup Instructions](#setup-instructions)
- [Configuration](#configuration)

## Features

- **Employee Time Tracking**: Record and manage work hours for employees.
- **RESTful API**: Provides endpoints for seamless integration with other systems.
- **Clean Architecture**: Ensures maintainability and scalability.
- **Entity Framework Core**: Simplifies database interactions.
- **ASP.NET Core**: Leverages modern web application framework capabilities.

## Stack

- **Programming Language**: C#
- **Framework**: ASP.NET Core
- **ORM**: Entity Framework Core
- **Database**: Microsoft SQL Server
- **Architecture**: Clean Architecture

## Installation

### Prerequisites

- **.NET SDK**: Version 6.0 or later
- **SQL Server**: Installed and running
- **Git**: For cloning the repository

### Setup Instructions

1. **Clone the Repository**:

   ```bash
   git clone https://github.com/Shchoholiev/work-time-tracker.git
   cd work-time-tracker
   ```

2. **Navigate to the Project Directory**:

   ```bash
   cd TimeTracker
   ```

3. **Restore Dependencies**:

   ```bash
   dotnet restore
   ```

4. **Update Database Connection String**:

   - Open `appsettings.json` in the `TimeTracker` directory.
   - Locate the `ConnectionStrings` section and update the `DefaultConnection` to match your SQL Server configuration.

5. **Apply Migrations and Update Database**:

   ```bash
   dotnet ef database update
   ```

6. **Run the Application**:

   ```bash
   dotnet run
   ```

   The API should now be running at `http://localhost:5000`.

## Configuration

- **Database Connection**: Set in `appsettings.json` under `ConnectionStrings:DefaultConnection`.
- **Logging**: Configurable in `appsettings.json` under the `Logging` section.
- **Environment Variables**: Ensure the `ASPNETCORE_ENVIRONMENT` variable is set appropriately (`Development`, `Staging`, `Production`).

For more detailed information, refer to the source code and comments within the repository.
