
namespace Dal;
using DalApi;
using DO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
/// <summary>
/// Implementation of the IAssignment interface for managing Assignment entities.
/// </summary>
internal class AssignmentImplementation : IAssignment
{
    /// <summary>
    /// Creates a new Assignment entity with a unique ID and adds it to the data source.
    /// </summary>
    /// <param name="item">The Assignment entity to be added.</param>
  
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void Create(Assignment item)
    {
        int newId = Config.NextAssignmentId;
        Assignment copy = item with { Id = newId };
        DataSource.Assignments.Add(copy);
    }

    /// <summary>
    /// Deletes an Assignment entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the Assignment entity to delete.</param>
    /// <exception cref="Exception">Thrown if the Assignment entity with the given ID does not exist.</exception>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void Delete(int id)
    {      
        Assignment? foundAssignment = DataSource.Assignments.Find(Assignment => Assignment.Id == id);
        if (foundAssignment == null)
        {
            throw new DalDoesNotExistException($"Assignment with ID={id} does not exist");
        }
        DataSource.Assignments.Remove(foundAssignment);
    }

    /// <summary>
    /// Deletes all Assignment entities from the data source.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void DeleteAll()
    {
        DataSource.Assignments.Clear();
    }

    /// <summary>
    /// Reads a specific Assignment entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the Assignment entity to retrieve.</param>
    /// <returns>The Assignment entity with the specified ID, or null if not found.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public Assignment? Read(int id)
    {
        Assignment? foundAssignment = DataSource.Assignments.FirstOrDefault(Assignment => Assignment.Id == id);
        return foundAssignment;
    }

    /// <summary>
    /// Reads all Assignment entities from the data source after filter the list.
    /// </summary>
    /// <returns>A list of all Assignment entities.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null) 
            => filter == null
            ? DataSource.Assignments.Select(item => item)
            : DataSource.Assignments.Where(filter);


    /// <summary>
    /// Updates an existing Assignment entity by replacing it with the provided entity.
    /// </summary>
    /// <param name="item">The updated Assignment entity.</param>
    /// <exception cref="Exception">Thrown if the Assignment entity with the specified ID does not exist.</exception>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void Update(Assignment item)
    {
       
        Assignment? foundAssignment = DataSource.Assignments.Find(Assignment => Assignment.Id == item.Id);
        if (foundAssignment == null)
        {
            throw new DalDoesNotExistException($"Assignment with ID={item.Id} does not exist");
        }
        DataSource.Assignments.Remove(foundAssignment);
        DataSource.Assignments.Add(item);
    }

    /// <summary>
    /// Reads the first assignment that matches the specified filter.
    /// </summary>
    /// <param name="filter">A function that defines the condition to match an assignment.</param>
    /// <returns>
    /// The first <see cref="Assignment"/> that matches the filter, or <c>null</c> if no matching assignment is found.
    /// </returns>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public Assignment? Read(Func<Assignment, bool> filter)
    { 
        return DataSource.Assignments.FirstOrDefault(filter);
    }
}


