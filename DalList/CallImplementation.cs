namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

/// <summary>
/// Implementation of the ICall interface for managing Call entities.
/// </summary>
public class CallImplementation : ICall
{
    /// <summary>
    /// Creates a new Call entity with a unique ID and adds it to the data source.
    /// </summary>
    /// <param name="item">The Call entity to be added.</param>
    public void Create(Call item)
    {
        List<DO.Call> list = DataSource.Calls;
        int newId = Config.NextAssignmentId;
        Call copy = item with { Id = newId };
        list.Add(copy);
    }

    /// <summary>
    /// Deletes a Call entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the Call entity to delete.</param>
    /// <exception cref="Exception">Thrown if the Call entity with the given ID does not exist.</exception>
    public void Delete(int id)
    {
        List<DO.Call> list = DataSource.Calls;
        Call? foundAssignment = list.Find(Call => Call.Id == id);
        if (foundAssignment == null)
        {
            throw new Exception($"An object of type Call with ID={id} does not exist");
        }
        list.Remove(foundAssignment);
    }

    /// <summary>
    /// Deletes all Call entities from the data source.
    /// </summary>
    public void DeleteAll()
    {
        List<DO.Call> list = DataSource.Calls;
        list.Clear();
    }

    /// <summary>
    /// Reads a specific Call entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the Call entity to retrieve.</param>
    /// <returns>The Call entity with the specified ID, or null if not found.</returns>
    public Call? Read(int id)
    {
        List<DO.Call> list = DataSource.Calls;
        Call? foundAssignment = list.Find(Call => Call.Id == id);
        return foundAssignment;
    }

    /// <summary>
    /// Reads all Call entities from the data source.
    /// </summary>
    /// <returns>A list of all Call entities.</returns>
    public List<Call> ReadAll()
    {
        return new List<Call>(DataSource.Calls);
    }

    /// <summary>
    /// Updates an existing Call entity by replacing it with the provided entity.
    /// </summary>
    /// <param name="item">The updated Call entity.</param>
    /// <exception cref="Exception">Thrown if the Call entity with the specified ID does not exist.</exception>
    public void Update(Call item)
    {
        List<DO.Call> list = DataSource.Calls;
        Call? foundAssignment = list.Find(Call => Call.Id == item.Id);
        if (foundAssignment == null)
        {
            throw new Exception($"An object of type Call with such an ID={item.Id} does not exist");
        }
        list.Remove(foundAssignment);
        list.Add(item);
    }
}
