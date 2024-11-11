

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class CallImplementation : ICall
{
    public void Create(Call item)
    {
        List<DO.Call> list = DataSource.Calls;
        int newId = Config.NextAssignmentId;
        Call copy = item with { Id = newId };
        list.Add(copy);
        /////////////////////////////////////////////////////////return int
    }

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
    
    public void DeleteAll()
    {
        List<DO.Call> list = DataSource.Calls;
        list.Clear();
    }

    public Call? Read(int id)
    {
        List<DO.Call> list = DataSource.Calls;
        Call? foundAssignment = list.Find(Call => Call.Id == id);
        return foundAssignment;
    }

    public List<Call> ReadAll()
    {
        return new List<Call>(DataSource.Calls);
    }

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
