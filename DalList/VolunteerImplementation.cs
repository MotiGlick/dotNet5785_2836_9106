

namespace Dal;


using DalApi;
using DO;
using System.Collections.Generic;

public class VolunteerImplementation : IVolunteer
{
    public void Create(Volunteer item)
    {
        if (Read(item.Id) == null)
        {
            List<DO.Volunteer> list = DataSource.Volunteers;
            list.Add(item);
            //return item.Id;
        }

        else
            throw new Exception($"An object of type Volunteer with  ID = { item.Id} already exist");
    }

    public void Delete(int id)
    {
        List<DO.Volunteer> list = DataSource.Volunteers;
        Volunteer? foundVolunteer = list.Find(Volunteer => Volunteer.Id == id);
        if (foundVolunteer == null) 
            throw new Exception($"An object of type Volunteer with ID = {id} does not exist");
        list.Remove(foundVolunteer);
    }

    public void DeleteAll()
    {
        List<DO.Volunteer> list = DataSource.Volunteers;
        list.Clear();   
    }

    public Volunteer? Read(int id)
    {
        List<DO.Volunteer> list = DataSource.Volunteers;
        Volunteer? foundVolunteer = list.Find(Volunteer => Volunteer.Id == id);
        if (foundVolunteer == null)  return null; 
        return foundVolunteer;
    }

    public List<Volunteer> ReadAll()
    {
        return new List<Volunteer>(DataSource.Volunteers);
    }

    public void Update(Volunteer item)
    {
        List<DO.Volunteer> list = DataSource.Volunteers;
        Volunteer? foundVolunteer = list.Find(Volunteer => Volunteer.Id == item.Id);
        if (foundVolunteer == null)
                throw new Exception($"An object of type Volunteer with ID = {item.Id} does not exist");
        list.Remove(foundVolunteer);
        list.Add(item);
    }
}
