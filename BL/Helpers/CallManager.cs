

namespace Helpers;

internal class CallManager
{
    public static bool logicaCheck(BO.Call boCall)
    {

        if (!Tools.AddressExists(boCall.Address) || (boCall.MaxTime > boCall.OpeningTime))
            return false;

        boCall.Latitude = Tools.GetCoordinates(boCall.Address)[0];
        boCall.Longitude = Tools.GetCoordinates(boCall.Address)[1];


        return true;

    }
}
