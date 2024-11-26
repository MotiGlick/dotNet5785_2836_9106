

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class VolunteerImplementation : IVolunteer
{
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
            throw new DalAlreadyExistsException($"Volunteer with  ID={item.Id}  already exists");
        }
    }

    public void Delete(int id)
    {
        List<Volunteer> Volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        if (Volunteers.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistException($"Volunteer with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Volunteers, Config.s_volunteers_xml);
    }

    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Volunteer>(), Config.s_volunteers_xml);
    }

    
    static Volunteer getVolunteer(XElement s)
    {
        return new DO.Volunteer()
        {
            Id = s.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
            Name = (string)s.Element("Name"),
            PhoneNumber = (string)s.Element("PhoneNumber") ,
            Email = (string)s.Element("Email") ,
            pasword = (string?)s.Element("pasword") ?? null,
            AddressVolunteer = (string?)s.Element("AddressVolunteer") ?? null,
            Latitude = s.ToDoubleNullable("Latitude"),
            Longitude = s.ToDoubleNullable("Longitude"),
            maximumDistace = s.ToDoubleNullable("maximumDistace"),
            duty = s.ToEnumNullable<job>("duty") ?? job.volunteer,
            IsActive = (bool?)s.Element("IsActive") ?? false,
            distance = s.ToEnumNullable<distanceType>("distance") ?? distanceType.Air,

        };
    }

    public Volunteer? Read(int id)
    {
        XElement? volunteerElem =
    XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml).Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        return volunteerElem is null ? null : getVolunteer(volunteerElem);
    }


    public Volunteer? Read(Func<Volunteer, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml).Elements().Select(s => getVolunteer(s)).FirstOrDefault(filter);
    }

    public IEnumerable<Volunteer> ReadAll(Func<Volunteer, bool>? filter = null)
    {
        IEnumerable<Volunteer> volunteers = XMLTools.LoadListFromXMLElement(Config.s_volunteers_xml)
        .Elements()
        .Select(s => getVolunteer(s)); 
        return filter is null ? volunteers : volunteers.Where(filter);
    }

    public void Update(Volunteer item)
    {
        List<Volunteer> Volunteers = XMLTools.LoadListFromXMLSerializer<Volunteer>(Config.s_volunteers_xml);
        if (Volunteers.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Volunteer with ID={item.Id} does Not exist");
        Volunteers.Add(item);
        XMLTools.SaveListToXMLSerializer(Volunteers, Config.s_volunteers_xml);
    }
}
