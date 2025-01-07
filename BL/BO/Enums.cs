

namespace BO;


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
    CleaningAssistance,     // Assistance with cleaning
    None,           // No call in progress
    No_Filter
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

/// <summary>
/// Represents fields in the CallInList class.
/// </summary>
public enum CallInListField
{
    Id,     
    CallId,
    CallType,
    OpeningTime,
    TimeLeft,
    LastVolunteerName,
    HandlingTime,
    status,
    AmountOfAssigns,
    //No_Filter  //stage 6 
}


/// <summary>
/// Represents fields in the VolunteerInList class.
/// </summary>
public enum VolunteerInListField
{
    Id,
    Name,
    CallType,
    IsActive,
    TotalHandledCalls,
    TotalCanceledCalls,
    TotalExpiredCalls,
    CurrentCallId,
    CurrentCallType
}


/// <summary>
/// Represents fields in the OpenCallInList class.
/// </summary>
public enum OpenCallInListField
{
    Id,
    CallType,
    Description,
    Address,
    OpeningTime,
    MaxTime,
    DistanceFromVolunteer,
}


/// <summary>
/// Represents fields in the ClosedCallInList class.
/// </summary>
public enum ClosedCallInListField
{
    Id,
    CallType,
    Address,
    OpeningTime,
    HndalStartTime,
    EndTime,
    ClosureType
}


/// <summary>
/// Represents options in the main menu.
/// </summary>
public enum MainMenuOptions
{
    Exit,                // Exits the main menu
    VolunteerMenu,       // Displays the submenu for volunteer actions
    CallMenu,            // Displays the submenu for call actions
    AdminMenu,          // Displays the submenu for   Admin actions

}

/// <summary>
/// Represents options in the volunteer menu.
/// </summary>
public enum VolunteerMenu
{
    Exit,
    EnterToTheSystem,
    Create,
    Update,
    Delete,
    Read,
    ReadAll,

}

/// <summary>
/// Represents options in the call menu.
/// </summary>
public enum CallMenuOptions
{
    Exit,                // Exits the  menu
    ChoosingCallForHandling,
    EndHandling,
    CancelHandling,
    UpdateCallDetails,
    AddCall,
    DeleteCall,
    GetCallList,
    GetOpenCallsForVolunteer,
    GetClosedCallsByVolunteer,
    GetCallDetails,
    CallsAmount,

}

/// <summary>
/// Represents options in the admin menu.
/// </summary>
public enum AdminMenu
{
    Exit,                // Exits the  menu
    AdvanceClock,
    GetMaxRange,
    GetClock,
    InitializeDB,
    ResetDB,
    SetMaxRange,

}



/// <summary>
/// Represents the possible statuses of a call.
/// </summary>
public enum StatusOfCall
{
    Open,                // The call is open and not currently assigned to any volunteer
    InProgress,          // The call is currently being handled by a volunteer
    Closed,              // The call has been successfully resolved
    Expired,             // The call was not handled in time or expired without being assigned
    OpenAtRisk,           // The call is open and approaching its maximum allowed time (within the risk threshold)
    InProgressAtRisk,     // The call is InProgress and approaching its maximum allowed time (within the risk threshold)
    No_Filter //stage 6
}



/// <summary>
/// Represents units of time.
/// </summary>
public enum TimeUnit
{
    Minute,
    Hour,
    Day,
    Month,
    Year
}

