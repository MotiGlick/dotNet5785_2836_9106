

using DalApi;

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class AssignmentImplementation : IAssignment
{
    public void Create(Assignment item)
    {
        List<DO.Assignment> list = DataSource.Assignments;
        int newId= Config.NextAssignmentId;
       Assignment copy = item with {Id=newId};
       list.Add(copy);
        ////////////////////////////////////////////////return int

    }

    public void Delete(int id)
    {
        List<DO.Assignment> list = DataSource.Assignments;
        Assignment? foundAssignment = list.Find(Assignment => Assignment.Id == id);
        if (foundAssignment == null)
        {
            throw new NotImplementedException("An object of type Assignment with such an ID does not exist");
        }
        list.Remove(foundAssignment);
    }

    public void DeleteAll()
    {
        List<DO.Assignment> list = DataSource.Assignments;
        list.Clear();
    }

    public Assignment? Read(int id)
    {
        List<DO.Assignment> list =DataSource.Assignments;
        Assignment? foundAssignment = list.Find(Assignment=>Assignment.Id==id);
        return foundAssignment;
    }

    public List<Assignment> ReadAll()
    {
        return new List<Assignment>(DataSource.Assignments);
    }

    public void Update(Assignment item)
    {
        List<DO.Assignment> list = DataSource.Assignments;
        Assignment? foundAssignment = list.Find(Assignment => Assignment.Id == item.Id);
        if (foundAssignment == null)
        {
            throw new NotImplementedException("An object of type Assignment with such an ID does not exist");
        }
        list.Remove(foundAssignment);
        list.Add(item);
    }
}



