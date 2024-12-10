
namespace Helpers;


using System.Text.Json;


internal class CallManager
{


    //BO.StatusOfCall GetCallStatus(int id)
    //{
    //}


    public static bool logicaCheck(BO.Call boCall)
    {

        return (boCall.MaxTime > boCall.OpeningTime) &&Tools.AddressExists(boCall.Address);


    }
    
    









}