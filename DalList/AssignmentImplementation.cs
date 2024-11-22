
namespace Dal;
using DalApi;
using DO;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Implementation of the IAssignment interface for managing Assignment entities.
/// </summary>
internal class AssignmentImplementation : IAssignment
{
    /// <summary>
    /// Creates a new Assignment entity with a unique ID and adds it to the data source.
    /// </summary>
    /// <param name="item">The Assignment entity to be added.</param>
    public void Create(Assignment item)
    {
        List<DO.Assignment> list = DataSource.Assignments;
        int newId = Config.NextAssignmentId;
        Assignment copy = item with { Id = newId };
        list.Add(copy);
    }

    /// <summary>
    /// Deletes an Assignment entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the Assignment entity to delete.</param>
    /// <exception cref="Exception">Thrown if the Assignment entity with the given ID does not exist.</exception>
    public void Delete(int id)
    {
        List<DO.Assignment> list = DataSource.Assignments;
        Assignment? foundAssignment = list.Find(Assignment => Assignment.Id == id);
        if (foundAssignment == null)
        {
            throw new DalDoesNotExistException($"Assignment with ID={id} does not exist");
        }
        list.Remove(foundAssignment);
    }

    /// <summary>
    /// Deletes all Assignment entities from the data source.
    /// </summary>
    public void DeleteAll()
    {
        List<DO.Assignment> list = DataSource.Assignments;
        list.Clear();
    }

    /// <summary>
    /// Reads a specific Assignment entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the Assignment entity to retrieve.</param>
    /// <returns>The Assignment entity with the specified ID, or null if not found.</returns>
    public Assignment? Read(int id)
    {
        List<DO.Assignment> list = DataSource.Assignments;
        Assignment? foundAssignment = list.FirstOrDefault(Assignment => Assignment.Id == id);
        return foundAssignment;
    }

    /// <summary>
    /// Reads all Assignment entities from the data source.
    /// </summary>
    /// <returns>A list of all Assignment entities.</returns>
    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null) 
            => filter == null
            ? DataSource.Assignments.Select(item => item)
            : DataSource.Assignments.Where(filter);
 

    /// <summary>
    /// Updates an existing Assignment entity by replacing it with the provided entity.
    /// </summary>
    /// <param name="item">The updated Assignment entity.</param>
    /// <exception cref="Exception">Thrown if the Assignment entity with the specified ID does not exist.</exception>
    public void Update(Assignment item)
    {
        List<DO.Assignment> list = DataSource.Assignments;
        Assignment? foundAssignment = list.Find(Assignment => Assignment.Id == item.Id);
        if (foundAssignment == null)
        {
            throw new DalDoesNotExistException($"Assignment with ID={item.Id} does not exist");
        }
        list.Remove(foundAssignment);
        list.Add(item);
    }

    public Assignment? Read(Func<Assignment, bool> filter)
    { 
        return DataSource.Assignments.FirstOrDefault(filter);
    }
}


