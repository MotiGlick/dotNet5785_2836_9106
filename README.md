VolunteerCare - Volunteer & Call Management System 

VolunteerCare is a comprehensive desktop application built with C# and WPF (.NET). It is designed to streamline the dispatch and management of volunteers for various support calls, featuring real-time geographical tracking, assignment management, and a dedicated workspace for both managers and volunteers.

Architecture

This project is built using a classic 3-Tier Architecture to ensure separation of concerns, scalability, and maintainability:

Presentation Layer (PL): Developed using WPF (Windows Presentation Foundation), providing a rich and responsive user interface.

Business Logic Layer (BLL): Handles all the core rules of the system, algorithms (such as finding the closest volunteer to a call), data validation, and state management.

Data Access Layer (DAL): Manages all data persistence (XML/Database), ensuring data integrity and providing a seamless API for the BLL to perform CRUD operations.

Key Features & Screenshots

1. Authentication

Secure login system separating Managers and Volunteers, directing each to their dedicated workspace.

2. Manager Dashboard & Simulator

A centralized control panel for the manager to oversee the entire system, manage time settings (simulator), and view real-time statistics of open and closed calls.

3. Volunteer Management

View, filter, add, and manage the active volunteers in the system.

4. Call (Task) Management

Managers can view all calls, filter them by status or type, and track their urgency (e.g., Open, InRisk, Closed).

5. Volunteer Workspace & Map Integration

Volunteers have their own dashboard to view their history and current assignments.
The system integrates Google Maps API via Microsoft.Web.WebView2 to display the volunteer's current location, nearby open calls, and dynamically generate driving/walking routes.

6. Call History

A detailed log for volunteers to review their past handled calls.

Getting Started

Follow these instructions to get a copy of the project up and running on your local machine.

Prerequisites

Visual Studio 2022 (or later)

.NET Framework / .NET Core (as configured in the solution)

Installation & Setup

Clone the repository:

git clone https://github.com/MotiGlick/dotNet5785_2836_9106.git


Google Maps API Key Setup:
For security reasons, the Google Maps API key is not included in this repository. To enable map features:

Create a new text file named api_key.txt inside the PL project directory (same folder as api_key.example.txt).

Paste your valid Google Maps API Key into api_key.txt.

Note: Ensure api_key.txt is set to "Copy if newer" in its Properties -> Copy to Output Directory.

Required Packages (Manual Setup):
This project uses Extended.Wpf.Toolkit and Microsoft.Web.WebView2. If the NuGet restore fails, you can manually link the files included in the project:

Navigate to the addons folder in the project directory.

For Extended.Wpf.Toolkit: Copy the DLLs from addons -> Xceed into your project's bin folder.

For WebView2: Copy the required files from addons -> WebView2 into your project's bin folder.

Build and Run:
Open the solution in Visual Studio, build the project to restore dependencies, and click Start.

Developed by Moti Glick & Avraham Shaviro