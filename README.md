# VolunteerCare - Volunteer & Call Management System

VolunteerCare is a comprehensive desktop application built with C# and WPF (.NET). It is designed to streamline the dispatch and management of volunteers for various support calls, featuring real-time geographical tracking, assignment management, and a dedicated workspace for both managers and volunteers.

## Architecture

This project is built using a classic **3-Tier Architecture** to ensure separation of concerns, scalability, and maintainability:

*   **Presentation Layer (PL):** Developed using WPF (Windows Presentation Foundation), providing a rich and responsive user interface.
*   **Business Logic Layer (BLL):** Handles all the core rules of the system, algorithms (such as finding the closest volunteer to a call), data validation, and state management.
*   **Data Access Layer (DAL):** Manages all data persistence (XML/Database), ensuring data integrity and providing a seamless API for the BLL to perform CRUD operations.

---

## Key Features & Screenshots

### 1. Authentication
Secure login system distinguishing between Administrators and Volunteers.
![Login Window](images/login.png)

### 2. Manager Dashboard & Simulator
Central hub for managers to oversee the system, adjust configurations (like Risk Ranges), and run a time-advancement simulator to test call expiration and risk states.
![Main Window](images/main_window.png)

### 3. Volunteer Management
Comprehensive grid to view, filter, and manage all registered volunteers, including their handled calls and duty status.
![Volunteer List](images/volunteer_list.png)

![Add Volunteer](images/add_volunteer.png)

### 4. Call Management
Dynamic list tracking all open and in-progress calls, sorted by urgency and status, preventing deletion of assigned calls.
![Call List](images/call_list.png)

![Add Call](images/add_call.png)

### 5. Volunteer Workspace
A dedicated view for volunteers to monitor their active assignments, update their availability, and review their history.
![Volunteer Dashboard](images/volunteer_dashboard.png)

### 6. Interactive Geographical Tracking (API Integration)
Integration with **Google Maps API** via `Microsoft.Web.WebView2`. Volunteers can view their current location, nearby open calls, and calculate driving routes directly within the app.
![Call Treatment Map](images/call_treatment_map.png)

### 7. Call History
Detailed logs for volunteers to review previously handled calls and their resolution times.
![Call History](images/call_history.png)

---

## Required Packages (Manual Setup)

This project utilizes two external UI and functionality packages. If Visual Studio does not restore them automatically, please follow these steps:

**1. Extended.Wpf.Toolkit (v4.6.1)**
Used for advanced UI controls like DateTimePicker and Watermarks.
*   Copy all `.dll` files from the `addons -> Xceed` folder in the project directory into the target `bin` folder.

**2. Microsoft.Web.WebView2**
Used to render the interactive Google Maps interface.
*   Copy all files from the `addons -> WebView2` folder in the project directory into the target `bin` folder.

## Google Maps Configuration

For security reasons, the Google Maps API key is not included in this repository. 
To enable the interactive map feature:
1. Locate `api_key.example.txt` in the root/bin directory.
2. Rename the file to `api_key.txt`.
3. Replace the placeholder text with your own Google Maps API Key.
*(Note: If the key is missing, the application will gracefully disable the map view while all other management features remain fully functional).*