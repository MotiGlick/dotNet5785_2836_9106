

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Xml.Linq;

internal class AssignmentImplementation : IAssignment
{
    
   

    public void Create(Assignment item)
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        int newId = Config.NextAssignmentId;
        Assignment copy = item with { Id = newId };
        Assignments.Add(copy);
        XMLTools.SaveListToXMLSerializer(Assignments, Config.s_assignments_xml);
    }

    public void Delete(int id)
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        if (Assignments.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistException($"Assignment with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Assignments, Config.s_assignments_xml);
    }

    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Assignment>(), Config.s_assignments_xml);
    }

    public Assignment? Read(int id)
    {
        XElement? AssignmentElem =
        XMLTools.LoadListFromXMLElement(Config.s_assignments_xml).Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        return AssignmentElem is null ? null : getAssignment(AssignmentElem);
    }

    public Assignment? Read(Func<Assignment, bool> filter)
    {

        return XMLTools.LoadListFromXMLElement(Config.s_assignments_xml).Elements().Select(s => getAssignment(s)).FirstOrDefault(filter);
    }

    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null)
    {

        IEnumerable<Assignment> assignments = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml).Elements().Select(s => getAssignment(s));
        return filter is null ? assignments : assignments.Where(filter);

    }


    public void Update(Assignment item)
    {
        List<Assignment> Assignments = XMLTools.LoadListFromXMLSerializer<Assignment>(Config.s_assignments_xml);
        if (Assignments.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Assignment with ID={item.Id} does Not exist");
        Assignments.Add(item);
        XMLTools.SaveListToXMLSerializer(Assignments, Config.s_assignments_xml);
    }


    static Assignment getAssignment(XElement s)
    {
        return new DO.Assignment()
        {
            Id = s.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
            CallId  = (int)s.ToIntNullable("CallId") ,
            VolunteerId = (int)s.ToIntNullable("VolunteerId"),
            AssignmentTime = (DateTime)s.ToDateTimeNullable("AssignmentTime"),//?? // throw new FormatException("can't convert CallTime"),
            AssignmentTotalTime = s.ToDateTimeNullable("AssignmentTotalTime"),
            type = s.ToEnumNullable<CallStatus>("type") ?? CallStatus.ExpiredCancellation,

        };
    }
}


   