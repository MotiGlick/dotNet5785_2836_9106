namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

/// <summary>
/// Implementation of the IVolunteer interface for managing Volunteer entities.
/// </summary>
internal class VolunteerImplementation : IVolunteer
{
    /// <summary>
    /// Creates a new Volunteer entity and adds it to the data source.
    /// </summary>
    /// <param name="item">The Volunteer entity to be added.</param>
    /// <exception cref="Exception">Thrown if a Volunteer with the same ID already exists.</exception>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void Create(Volunteer item)
    {

        
        if (Read(item.Id) == null)
        {
            DataSource.Volunteers.Add(item);
        }
        else
        {
            throw new DalAlreadyExistsException($"Volunteer with  ID={item.Id}  already exists");
        }
    }

    /// <summary>
    /// Deletes a Volunteer entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the Volunteer entity to delete.</param>
    /// <exception cref="Exception">Thrown if the Volunteer with the given ID does not exist.</exception>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void Delete(int id)
    {
        Volunteer? foundVolunteer = DataSource.Volunteers.Find(Volunteer => Volunteer.Id == id);
        if (foundVolunteer == null)
        {
            throw new DalDoesNotExistException($"Volunteer with ID = {id} does not exist");
        }
        DataSource.Volunteers.Remove(foundVolunteer);
    }

    /// <summary>
    /// Deletes all Volunteer entities from the data source.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void DeleteAll()
    {
        DataSource.Volunteers.Clear();
    }

    /// <summary>
    /// Reads a specific Volunteer entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the Volunteer entity to retrieve.</param>
    /// <returns>The Volunteer entity with the specified ID, or null if not found.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public Volunteer? Read(int id)
    {
        Volunteer? foundVolunteer = DataSource.Volunteers.FirstOrDefault(Volunteer => Volunteer.Id == id);
        return foundVolunteer;
    }

    /// <summary>
    /// Reads all Volunteer entities from the data source.
    /// </summary>
    /// <returns>A list of all Volunteer entities.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null) => filter == null
            ? DataSource.Volunteers.Select(item => item)
            : DataSource.Volunteers.Where(filter);

    /// <summary>
    /// Updates an existing Volunteer entity by replacing it with the provided entity.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="DalDoesNotExistException">Thrown if the Volunteer with the specified ID does not exist</exception>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void Update(Volunteer item)
    {
        
        Volunteer? foundVolunteer = DataSource.Volunteers.Find(Volunteer => Volunteer.Id == item.Id);
        if (foundVolunteer == null)
        {
            throw new DalDoesNotExistException($"Volunteer with  ID={item.Id} does not exist");

        }
        DataSource.Volunteers.Remove(foundVolunteer);
        DataSource.Volunteers.Add(item);
    }

    /// <summary>
    /// Reads the first volunteer that matches the specified filter.
    /// </summary>
    /// <param name="filter">A function that defines the condition to match a volunteer.</param>
    /// <returns>
    /// The first Volunteer that matches the filter, or null if no matching volunteer is found.
    /// </returns>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        return DataSource.Volunteers.FirstOrDefault(filter);
    }
}
