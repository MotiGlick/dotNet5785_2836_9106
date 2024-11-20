namespace Dal;

using DalApi;
using DO;
using System.Collections.Generic;

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
    public void Create(Volunteer item)
    {
        if (Read(item.Id) == null)
        {
            List<DO.Volunteer> list = DataSource.Volunteers;
            list.Add(item);
        }
        else
        {
            throw new Exception($"An object of type Volunteer with ID = {item.Id} already exists");
        }
    }

    /// <summary>
    /// Deletes a Volunteer entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the Volunteer entity to delete.</param>
    /// <exception cref="Exception">Thrown if the Volunteer with the given ID does not exist.</exception>
    public void Delete(int id)
    {
        List<DO.Volunteer> list = DataSource.Volunteers;
        Volunteer? foundVolunteer = list.Find(Volunteer => Volunteer.Id == id);
        if (foundVolunteer == null)
        {
            throw new Exception($"An object of type Volunteer with ID = {id} does not exist");
        }
        list.Remove(foundVolunteer);
    }

    /// <summary>
    /// Deletes all Volunteer entities from the data source.
    /// </summary>
    public void DeleteAll()
    {
        List<DO.Volunteer> list = DataSource.Volunteers;
        list.Clear();
    }

    /// <summary>
    /// Reads a specific Volunteer entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the Volunteer entity to retrieve.</param>
    /// <returns>The Volunteer entity with the specified ID, or null if not found.</returns>
    public Volunteer? Read(int id)
    {
        List<DO.Volunteer> list = DataSource.Volunteers;
        Volunteer? foundVolunteer = list.FirstOrDefault(Volunteer => Volunteer.Id == id);
        return foundVolunteer;
    }

    /// <summary>
    /// Reads all Volunteer entities from the data source.
    /// </summary>
    /// <returns>A list of all Volunteer entities.</returns>
    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null) => filter == null
            ? DataSource.Volunteers.Select(item => item)
            : DataSource.Volunteers.Where(filter);
    /// <summary>
    /// Updates an existing Volunteer entity by replacing it with the provided entity.
    /// </summary>
    /// <param name="item">The updated Volunteer entity.</param>
    /// <exception cref="Exception">Thrown if the Volunteer with the specified ID does not exist.</exception>
    public void Update(Volunteer item)
    {
        List<DO.Volunteer> list = DataSource.Volunteers;
        Volunteer? foundVolunteer = list.Find(Volunteer => Volunteer.Id == item.Id);
        if (foundVolunteer == null)
        {
            throw new Exception($"An object of type Volunteer with ID = {item.Id} does not exist");
        }
        list.Remove(foundVolunteer);
        list.Add(item);
    }

    public Volunteer? Read(Func<Volunteer, bool> filter)
    {

        return DataSource.Volunteers.FirstOrDefault(filter);
    }
}
