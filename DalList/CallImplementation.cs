namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Implementation of the ICall interface for managing Call entities.
/// </summary>
internal class CallImplementation : ICall
{
    /// <summary>
    /// Creates a new Call entity with a unique ID and adds it to the data source.
    /// </summary>
    /// <param name="item">The Call entity to be added.</param>
    public void Create(Call item)
    {
        int newId = Config.NextCallId;         /////////
        Call copy = item with { Id = newId };
        DataSource.Calls.Add(copy);
    }

    /// <summary>
    /// Deletes a Call entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the Call entity to delete.</param>
    /// <exception cref="Exception">Thrown if the Call entity with the given ID does not exist.</exception>
    public void Delete(int id)
    {    
        Call? foundAssignment = DataSource.Calls.Find(Call => Call.Id == id);
        if (foundAssignment == null)
        {
            throw new DalDoesNotExistException($"Call with ID={id} does not exist");
        }
        DataSource.Calls.Remove(foundAssignment);
    }

    /// <summary>
    /// Deletes all Call entities from the data source.
    /// </summary>
    public void DeleteAll()
    {
        DataSource.Calls.Clear();
    }

    /// <summary>
    /// Reads a specific Call entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the Call entity to retrieve.</param>
    /// <returns>The Call entity with the specified ID, or null if not found.</returns>
    public Call? Read(int id)
    {
        Call? foundAssignment = DataSource.Calls.FirstOrDefault(Call => Call.Id == id);
        return foundAssignment;
    }

    /// <summary>
    /// Reads all Call entities from the data source.
    /// </summary>
    /// <returns>A list of all Call entities.</returns>
    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null) => filter == null
            ? DataSource.Calls.Select(item => item)
            : DataSource.Calls.Where(filter);
    


    /// <summary>
    /// Updates an existing Call entity by replacing it with the provided entity.
    /// </summary>
    /// <param name="item">The updated Call entity.</param>
    /// <exception cref="Exception">Thrown if the Call entity with the specified ID does not exist.</exception>
    public void Update(Call item)
    {
        Call? foundAssignment = DataSource.Calls.Find(Call => Call.Id == item.Id);
        if (foundAssignment == null)
        {
            throw new DalDoesNotExistException($"Call with ID={item.Id} does not exist");
        }
        DataSource.Calls.Remove(foundAssignment);
        DataSource.Calls.Add(item);
    }

    /// <summary>
    /// Reads the first call that matches the specified filter.
    /// </summary>
    /// <param name="filter">A function that defines the condition to match a call.</param>
    /// <returns>
    /// The first Call that matches the filter, or null if no matching call is found.
    /// </returns>
    public Call? Read(Func<Call, bool> filter)
    {

        return DataSource.Calls.FirstOrDefault(filter);
    }
}
