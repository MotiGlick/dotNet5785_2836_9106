

using System;

namespace BlApi;


/// <summary>
/// Interface for managing volunteers.
/// Provides methods for creating, reading, updating, and deleting volunteers,
/// as well as logging into the system and retrieving lists of volunteers.
/// </summary>
public interface IVolunteer :IObservable
{

    /// <summary>
    /// Logs a user into the system and determines their job role.
    /// </summary>
    /// <param name="name">The name of the volunteer attempting to log in.</param>
    /// <param name="passward">The password of the volunteer (optional).</param>
    /// <returns>The job role of the volunteer (e.g., Manager, Volunteer).</returns>
    BO.job EnterToTheSystem(string? name , string? passward ,int? id);

    /// <summary>
    /// Creates a new volunteer.
    /// </summary>
    /// <param name="s">The volunteer object to be created.</param>
    void Create(BO.Volunteer s);

    /// <summary>
    /// Reads the details of a specific volunteer.
    /// </summary>
    /// <param name="id">The unique identifier of the volunteer.</param>
    /// <returns>The volunteer object containing all details.</returns>
    BO.Volunteer Read(int id);


    /// <summary>
    /// Retrieves a list of volunteers based on their activity status and sort preferences.
    /// </summary>
    /// <param name="isActive">Filters volunteers based on their activity status (optional).</param>
    /// <param name="sortField">Specifies the field to sort the volunteer list by (optional).</param>
    /// <returns>A collection of volunteers in a summarized list format.</returns>
    IEnumerable<BO.VolunteerInList> ReadAll(bool? isActive, BO.VolunteerInListField? sortField , BO.CallType? filterValue);

    /// <summary>
    /// Updates the details of an existing volunteer.
    /// </summary>
    /// <param name="boVolunteer">The volunteer object containing updated details.</param>
    /// <param name="id">The unique identifier of the volunteer being updated.</param>
    void Update(BO.Volunteer boVolunteer ,int id);


    /// <summary>
    /// Deletes a volunteer from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the volunteer to be deleted.</param>
    void Delete(int id);



    bool CanDeleted(int id);


}
