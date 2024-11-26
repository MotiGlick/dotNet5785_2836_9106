

namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

internal class CallImplementation : ICall
{
    public void Create(Call item)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        int newId = Config.NextAssignmentId;
        Call copy = item with { Id = newId };
        Calls.Add(copy);
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }

    public void Delete(int id)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (Calls.RemoveAll(it => it.Id == id) == 0)
            throw new DalDoesNotExistException($"Call with ID={id} does Not exist");
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }

    public void DeleteAll()
    {
        XMLTools.SaveListToXMLSerializer(new List<Call>(), Config.s_calls_xml);
    }

    public Call? Read(int id)
    {
        XElement? callElem =
        XMLTools.LoadListFromXMLElement(Config.s_calls_xml).Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        return callElem is null ? null : getCall(callElem);
    }

    public Call? Read(Func<Call, bool> filter)
    {

        return XMLTools.LoadListFromXMLElement(Config.s_calls_xml).Elements().Select(s => getCall(s)).FirstOrDefault(filter);
    }

    public IEnumerable<Call> ReadAll(Func<Call, bool>? filter = null)
    {

        IEnumerable<Call> calls = XMLTools.LoadListFromXMLElement(Config.s_calls_xml) .Elements().Select(s => getCall(s));
        return filter is null ? calls : calls.Where(filter);

    }


    public void Update(Call item)
    {
        List<Call> Calls = XMLTools.LoadListFromXMLSerializer<Call>(Config.s_calls_xml);
        if (Calls.RemoveAll(it => it.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Call with ID={item.Id} does Not exist");
        Calls.Add(item);
        XMLTools.SaveListToXMLSerializer(Calls, Config.s_calls_xml);
    }


    static Call getCall(XElement s)
    {
        return new DO.Call()
        {
            Id = s.ToIntNullable("Id") ?? throw new FormatException("can't convert id"),
            type = s.ToEnumNullable<CallType>("type") ?? CallType.HomeCare,
            CallAddress = (string?)s.Element("CallAddress") ?? "",
            Description = (string?)s.Element("Description") ?? null,
            Latitude = s.ToDoubleNullable("Latitude") ?? 0,//throw new FormatException("can't convert Latitude"),
            Longitude = s.ToDoubleNullable("Longitude") ?? 0,// throw new FormatException("can't convert Longitude"),
            CallTime = (DateTime)s.ToDateTimeNullable("CallTime"),//?? // throw new FormatException("can't convert CallTime"),
            TotalTime = s.ToDateTimeNullable("TotalTime")


        };
    }

}


