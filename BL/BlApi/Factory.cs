

using System;

namespace BlApi;



/// <summary>
/// Factory class to provide an instance of the Business Logic (BL) layer.
/// </summary>
public static class Factory
{
    /// <summary>
    /// Retrieves an implementation of the IBl interface.
    /// </summary>
    /// <returns>An instance of the BL implementation.</returns>
    public static IBl Get() => new BlImplementation.Bl();
}
