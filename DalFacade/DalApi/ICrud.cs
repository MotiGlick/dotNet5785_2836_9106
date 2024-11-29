using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    /// <summary>
    /// Generic interface for performing CRUD (Create, Read, Update, Delete) operations 
    /// on data entities in the Data Access Layer (DAL). 
    /// </summary>
    /// <typeparam name="T">The type of the entity the operations are performed on</typeparam>
    public interface ICrud<T> where T : class
    {
        void Create(T item); //Creates new entity object in DAL
        T? Read(int id); //Reads entity object by its ID 
        IEnumerable<T> ReadAll(Func<T, bool>? filter = null);// stage 2
        void Update(T item); //Updates entity object
        void Delete(int id); //Deletes an object by its Id
        void DeleteAll(); //Delete all entity objects

        T? Read(Func<T, bool> filter);// stage 2
    }

}
