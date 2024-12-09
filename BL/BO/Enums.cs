

namespace BO;


public enum Duty
{
    manager,    // Manager role, overseeing other volunteers and operations
    volunteer   // Regular volunteer role, assisting in tasks
}

/// <summary>
/// Enum representing types of distances relevant to the volunteer's tasks.
/// Different types may require different transportation modes or time calculations.
/// </summary>
public enum TypeOfDistance
{
    Air,   // Distance as the crow flies (straight line)
    walk,  // Walking distance
    car    // Driving distance
}


/// <summary>
/// Enum representing different types of calls for assistance.
/// Each type defines a specific kind of help requested by a user.
/// </summary>
public enum TypeOfCall
{
    ProvideTransportation, // Transportation assistance
    MedicalAssistance,     // Medical help or support
    EmotionalSupport,      // Emotional or psychological support
    HomeCare,              // Home care assistance
    HospitalVisit,         // Support through a hospital visit
    DeliverMedicine,       // Delivery of medication to the requester
    CleaningAssistance,     // Assistance with cleaning
    None                   // No call in progress
}


/// <summary>
/// Enum for the status of a call after it has been handled or closed.
/// Each status describes the reason or outcome of the call.
/// </summary>
public enum StatusOfCall
{
    HandledOnTime,       // The call was handled within the allowed time
    SelfCancellation,    // The volunteer chose to cancel the call
    ManagerCancellation, // The manager canceled the call assignment
    ExpiredCancellation  // The call was canceled because the allowed time expired
}