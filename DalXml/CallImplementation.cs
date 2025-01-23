
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;


/// <summary>
/// Implementation of the ICall interface for managing call records in the data store.
/// Provides methods for creating, reading, updating, and deleting calls in the XML data store.
/// </summary>
internal class CallImplementation : ICall
{
    /// <summary>
    /// Creates a new call and adds it to the XML data store.
    /// </summary>
    /// <param name="item">The call to be created.</param>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void Create(Call item)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        int newId = Config.NextCallId;
        Call copy = item with { Id = newId };
        Calls.Add(copy);
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }


    /// <summary>
    /// Deletes a call by its ID from the XML data store.
    /// </summary>
    /// <param name="id">The ID of the call to be deleted.</param>
    /// <exception cref="DalDoesNotExistException">Thrown if the call does not exist in the data store.</exception>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void Delete(int id)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (Calls.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistException($"Call with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }


    /// <summary>
    /// Deletes all calls from the XML data store.
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Call>(), Config.s_calls_xml);
    }


    /// <summary>
    /// Reads a call by its ID from the XML data store.
    /// </summary>
    /// <param name="id">The ID of the call to be read.</param>
    /// <returns>The call with the specified ID, or null if not found.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public Call? Read(int id)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        return Calls.FirstOrDefault(Call => Call.Id == id);
    }


    /// <summary>
    /// Reads a call based on a filter function from the XML data store.
    /// </summary>
    /// <param name="filter">A filter function to apply to the calls.</param>
    /// <returns>The first call that matches the filter, or null if no call matches.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public Call? Read(Func<Call, bool> filter)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        return Calls.FirstOrDefault(filter);
    }


    /// <summary>
    /// Reads all calls from the XML data store, optionally filtered by a given filter function.
    /// </summary>
    /// <param name="filter">An optional filter function to apply to the calls.</param>
    /// <returns>An enumerable collection of calls that match the filter, or all calls if no filter is provided.</returns>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        return filter is null ? Calls : Calls.Where(filter);
    }


    /// <summary>
    /// Updates an existing call in the XML data store.
    /// </summary>
    /// <param name="item">The call to be updated.</param>
    /// <exception cref="DalDoesNotExistException">Thrown if the call does not exist in the data store.</exception>
    [MethodImpl(MethodImplOptions.Synchronized)] //stage 7
    public void Update(Call item)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        Call? foundAssignment = Calls.Find(Call => Call.Id == item.Id);
        if (foundAssignment == null)
        {
            throw new DalDoesNotExistException($"Call with ID={item.Id} does Not exist");
        }
        Calls.Remove(foundAssignment);
        Calls.Add(item);
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }
}


