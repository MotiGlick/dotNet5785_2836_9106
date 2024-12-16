

namespace DO;

/// <summary>
/// Enum representing the main menu options in the system.
/// Each option directs to a different functionality or submenu.
/// </summary>
public enum MainMenuOption
{
    Exit,                // Exits the main menu
    VolunteerMenu,       // Displays the submenu for volunteer-related actions
    CallMenu,            // Displays the submenu for call-related actions
    AssignmentMenu,      // Displays the submenu for assignment-related actions
    InitializeData,      // Initializes the system data
    ShowAllData,         // Displays all system data
    ConfigurationMenu,   // Displays the submenu for configuration settings
    ResetDatabase        // Resets the database and configuration settings
}

/// <summary>
/// Enum representing the submenu options for managing entities in the system.
/// Each option allows for a different operation on entities, such as create, read, update, or delete.
/// </summary>
public enum SubMenuOption
{
    Exit,         // Exits the submenu
    Create,       // Adds a new entity to the list
    Read,         // Displays an entity by its ID
    ReadAll,      // Displays a list of all entities of a given type
    Update,       // Updates an existing entity's data
    Delete,       // Removes an existing entity from the list
    DeleteAll     // Removes all entities from the list
}

/// <summary>
/// Enum for configuration menu options to adjust system settings.
/// This includes advancing the system clock and displaying or modifying configuration values.
/// </summary>
public enum ConfigMenu
{
    Exit,                     // Exits the configuration menu
    AdvanceClockMinute,       // Advances the system clock by one minute
    AdvanceClockHour,         // Advances the system clock by one hour
    AdvanceClockDay,          // Advances the system clock by one day
    AdvanceClockMonth,        // Advances the system clock by one month
    AdvanceClockYear,         // Advances the system clock by one year
    DisplayCurrentClock,      // Displays the current system clock
    SetNewConfigValue,        // Sets a new value for a specified configuration variable
    DisplayCurrentConfigValue,// Displays the current value of a specified configuration variable
    ResetAllConfigValues      // Resets all configuration values to their initial state
}

/// <summary>
/// Enum representing different types of calls for assistance.
/// Each type defines a specific kind of help requested by a user.
/// </summary>
public enum CallType
{
    ProvideTransportation, // Transportation assistance
    MedicalAssistance,     // Medical help or support
    EmotionalSupport,      // Emotional or psychological support
    HomeCare,              // Home care assistance
    HospitalVisit,         // Support through a hospital visit
    DeliverMedicine,       // Delivery of medication to the requester
    CleaningAssistance     // Assistance with cleaning
}

/// <summary>
/// Enum defining job roles for volunteers within the organization.
/// Each value represents a specific role or level of responsibility.
/// </summary>
public enum job
{
    manager,    // Manager role, overseeing other volunteers and operations
    volunteer   // Regular volunteer role, assisting in tasks
}

/// <summary>
/// Enum representing types of distances relevant to the volunteer's tasks.
/// Different types may require different transportation modes or time calculations.
/// </summary>
public enum DistanceType
{
    Air,   // Distance as the crow flies (straight line)
    walk,  // Walking distance
    car    // Driving distance
}


/// <summary>
/// Enum for the status of a call after it has been handled or closed.
/// Each status describes the reason or outcome of the call.
/// </summary>
public enum CallClosureType
{
    HandledOnTime,       // The call was handled within the allowed time
    SelfCancellation,    // The volunteer chose to cancel the call
    ManagerCancellation, // The manager canceled the call assignment
    ExpiredCancellation  // The call was canceled because the allowed time expired
}





