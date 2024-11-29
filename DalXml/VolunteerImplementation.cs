
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This class implements the IVolunteer interface for managing volunteer data. 
/// </summary
internal class VolunteerImplementation : IVolunteer
{

    /// <summary>
    /// Creates a new volunteer record.
    /// </summary>
    /// <param name="item">The volunteer object to be added.</param>
    /// <exception cref="DalAlreadyExistsException">Thrown when a volunteer with the same ID already exists.</exception>
    public void Create(Volunteer item)
    {
        List<Volunteer> Volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        if (Read(item.Id) == null)
        {
            Volunteers.Add(item);
            XMLTools.SaveListToXMLSerializer(Volunteers, Config.s_volunteers_xml);
        }
        else
        {
            throw new DalAlreadyExistsException($"Volunteer with ID={item.Id} already exists");

        }
    }


    /// <summary>
    /// Deletes a volunteer record by ID.
    /// </summary>
    /// <param name="id">The ID of the volunteer to be deleted.</param>
    /// <exception cref="DalDoesNotExistException">Thrown when no volunteer with the specified ID exists.</exception>
    public void Delete(int id)
    {
        List<Volunteer> Volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);

        if (Volunteers.RemoveAll(Volunteer => Volunteer.Id == id) == 0)
        {
            throw new DalDoesNotExistException($"Volunteer with ID={id} does Not exist");
        }

        XMLTools.SaveListToXMLSerializer(Volunteers, Config.s_volunteers_xml);
    }

    /// <summary>
    /// Deletes all volunteer records.
    /// </summary>
    public void DeleteAll()
    {
        
        XMLTools.SaveListToXMLSerializer(new List<Volunteer>(), Config.s_volunteers_xml);
    }


    /// <summary>
    /// Reads a volunteer record by ID.
    /// </summary>
    /// <param name="id">The ID of the volunteer to be read.</param>
    /// <returns>The volunteer record, or null if no volunteer with the specified ID exists.</returns>
    public Volunteer? Read(int id)
    {
        List<Volunteer> Volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        return Volunteers.FirstOrDefault(volunteer => volunteer.Id == id);
    }

    /// <summary>
    /// Reads a volunteer record based on a custom filter.
    /// </summary>
    /// <param name="filter">A function to filter volunteers by custom criteria.</param>
    /// <returns>The first volunteer that matches the filter, or null if no match is found.</returns>
    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        List<Volunteer> Volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        return Volunteers.FirstOrDefault(filter);
    }

    /// <summary>
    /// Reads all volunteer records, optionally filtered by custom criteria.
    /// </summary>
    /// <param name="filter">An optional function to filter volunteers by custom criteria.</param>
    /// <returns>A collection of all volunteers, optionally filtered.</returns>
    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null)
    {
        List<Volunteer> Volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        return filter is null ? Volunteers : Volunteers.Where(filter);
    }

    /// <summary>
    /// Updates an existing volunteer record.
    /// </summary>
    /// <param name="item">The volunteer object with updated data.</param>
    /// <exception cref="DalDoesNotExistException">Thrown when no volunteer with the specified ID exists.</exception>
    public void Update(Volunteer item)
    {
        List<Volunteer> Volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        Volunteer? foundVolunteer = Volunteers.Find(volunteer => volunteer.Id == item.Id);
        if (foundVolunteer == null)
        {
            throw new DalDoesNotExistException($"Volunteer with ID={item.Id} does Not exist");
        }

        Volunteers.Remove(foundVolunteer);
        Volunteers.Add(item);
        XMLTools.SaveListToXMLSerializer(Volunteers, Config.s_volunteers_xml);
    }
}
