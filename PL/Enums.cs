namespace PL;
using BO;
using System;
using System.Collections;
internal class CallTypeCollection : IEnumerable
{
    static readonly IEnumerable<BO.CallType> s_enums =
    (Enum.GetValues(typeof(BO.CallType)) as IEnumerable<BO.CallType>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class JubCollection : IEnumerable
{
    static readonly IEnumerable<BO.job> s_enums =
    (Enum.GetValues(typeof(BO.job)) as IEnumerable<BO.job>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class DistanceTypeCollection : IEnumerable
{
    static readonly IEnumerable<BO.DistanceType> s_enums =
    (Enum.GetValues(typeof(BO.DistanceType)) as IEnumerable<BO.DistanceType>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}


internal class StatusOfCallCollection : IEnumerable
{
    static readonly IEnumerable<BO.StatusOfCall> s_enums =
    (Enum.GetValues(typeof(BO.StatusOfCall)) as IEnumerable<BO.StatusOfCall>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}


internal class CallInListFieldCollection : IEnumerable
{
    static readonly IEnumerable<BO.CallInListField> s_enums =
    (Enum.GetValues(typeof(BO.CallInListField)) as IEnumerable<BO.CallInListField>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}




internal class ClosedCallInLisCollection : IEnumerable
{
    static readonly IEnumerable<BO.ClosedCallInListField> s_enums =
    (Enum.GetValues(typeof(BO.ClosedCallInListField)) as IEnumerable<BO.ClosedCallInListField>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}