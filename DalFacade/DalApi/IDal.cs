using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface IDal
    {
        IVolunteer Volunteer{get;}
        IAssignment Assignment { get;}
        ICall Call {  get;}
        IConfig Config {  get;}
        void ResetDB();

    }

}
