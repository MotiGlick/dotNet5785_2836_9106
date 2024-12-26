
using System;
using System.Collections;
namespace PL;
internal class VolunteerInListCollection : IEnumerable
{
    static readonly IEnumerable<BO.VolunteerInListField> s_enums =
    (Enum.GetValues(typeof(BO.VolunteerInListField)) as IEnumerable<BO.VolunteerInListField>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

