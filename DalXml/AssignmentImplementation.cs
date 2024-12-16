
using Dal;
using DalApi;
using DO;
using System.Xml.Linq;


/// <summary>
/// Implementation of the IAssignment interface for managing Assignment records in the data store.
/// Provides methods for creating, reading, updating, and deleting Assignments in the XML data store.
/// </summary>
internal class AssignmentImplementation : IAssignment
{
    /// <summary>
    /// Creates a new assignment and adds it to the XML data store.
    /// </summary>
    /// <param name="item">The assignment to be created.</param>
    public void Create(Assignment item)
    {      
        XElement assignmentsXml = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);
        XElement newAssignmentElement = new XElement("Assignment",
            new XElement("Id", Config.NextAssignmentId),
            new XElement("CallId", item.CallId),
            new XElement("VolunteerId", item.VolunteerId),
            new XElement("AssignmentTime", item.AssignmentTime),
            new XElement("AssignmentTotalTime", item.AssignmentTotalTime),
            new XElement("Type", item.type.ToString())
        );
        assignmentsXml.Add(newAssignmentElement);
        XMLTools.SaveListToXMLElement(assignmentsXml, Config.s_assignments_xml);
    }

    /// <summary>
    /// Deletes an assignment by its ID from the XML data store.
    /// </summary>
    /// <param name="id">The ID of the assignment to be deleted.</param>
    /// <exception cref="DalDoesNotExistException">Thrown if the assignment does not exist in the data store.</exception>
    public void Delete(int id)
    {
        XElement assignmentsXml = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);
        XElement assignmentToRemove = assignmentsXml.Elements().FirstOrDefault(e => (int)e.Element("Id") == id);

        if (assignmentToRemove != null)
        {
            assignmentToRemove.Remove();
            XMLTools.SaveListToXMLElement(assignmentsXml, Config.s_assignments_xml);
        }
        else
        {
            throw new DalDoesNotExistException($"Assignment with ID={id} does Not exist");
        }
    }

    /// <summary>
    /// Deletes all assignments from the XML data store.
    /// </summary>
    public void DeleteAll()
    {
        XMLTools.SaveListToXMLElement(new XElement("Assignments"), Config.s_assignments_xml);
    }




    /// <summary>
    /// Reads an assignment by its ID from the XML data store.
    /// </summary>
    /// <param name="id">The ID of the assignment to be read.</param>
    /// <returns>The assignment with the specified ID, or null if not found.</returns>
    public Assignment? Read(int id)
    {
        XElement assignmentsXml = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);
        XElement assignmentElement= assignmentsXml.Elements().FirstOrDefault(e => (int)e.Element("Id") == id);
        return assignmentElement != null ? GetAssignmentFromXElement(assignmentElement) : null;
    }

    /// <summary>
    /// Reads an assignment based on a filter function from the XML data store.
    /// </summary>
    /// <param name="filter">A filter function to apply to the assignments.</param>
    /// <returns>The first assignment that matches the filter, or null if no assignment matches.</returns>
    public Assignment? Read(Func<Assignment, bool> filter)
    {
        XElement assignmentsXml = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);
        Assignment assignment = assignmentsXml.Elements().Select(e => GetAssignmentFromXElement(e)).FirstOrDefault(filter);
        return assignment;
    }

    /// <summary>
    /// Reads all assignments from the XML data store, optionally filtered by a given filter function.
    /// </summary>
    /// <param name="filter">An optional filter function to apply to the assignments.</param>
    /// <returns>An enumerable collection of assignments that match the filter, or all assignments if no filter is provided.</returns>
    public IEnumerable<Assignment> ReadAll(Func<Assignment, bool>? filter = null)
    {
        XElement assignmentsXml = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);
        var assignments = assignmentsXml.Elements()
            .Select(e => GetAssignmentFromXElement(e));

        return filter == null ? assignments : assignments.Where(filter);
    }


    /// <summary>
    /// Updates an existing assignment in the XML data store.
    /// </summary>
    /// <param name="item">The assignment to be updated.</param>
    /// <exception cref="DalDoesNotExistException">Thrown if the assignment does not exist in the data store.</exception>
    public void Update(Assignment item)
    {
        XElement assignmentsXml = XMLTools.LoadListFromXMLElement(Config.s_assignments_xml);
        XElement assignmentElement = assignmentsXml.Elements().FirstOrDefault(e => (int)e.Element("Id") == item.Id);

        if (assignmentElement != null)
        {
            assignmentElement.SetElementValue("CallId", item.CallId);
            assignmentElement.SetElementValue("VolunteerId", item.VolunteerId);
            assignmentElement.SetElementValue("AssignmentTime", item.AssignmentTime);
            assignmentElement.SetElementValue("AssignmentTotalTime", item.AssignmentTotalTime);
            assignmentElement.SetElementValue("Type", item.type.ToString());

            XMLTools.SaveListToXMLElement(assignmentsXml, Config.s_assignments_xml);
        }
        else
        {
            throw new DalDoesNotExistException($"Assignment with ID={item.Id} does Not exist");
        }
    }

    /// <summary>
    /// Converts an XElement to an Assignment object.
    /// </summary>
    /// <param name="element">The XElement to be converted.</param>
    /// <returns>The corresponding Assignment object.</returns>
    private Assignment GetAssignmentFromXElement(XElement element)
    {
        return new Assignment
        {
            Id = element.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
            CallId = element.ToIntNullable("CallId")??throw new FormatException("can't convert CallId"),
            VolunteerId = element.ToIntNullable("VolunteerId") ?? throw new FormatException("can't convert VolunteerId"),
            AssignmentTime = element.ToDateTimeNullable("AssignmentTime")?? throw new FormatException("can't convert AssignmentTime"),
            AssignmentTotalTime = element.ToDateTimeNullable("AssignmentTotalTime")??null,
            type = element.ToEnumNullable<CallClosureType>("Type")?? null,
        };
    }
}


    

   

   
