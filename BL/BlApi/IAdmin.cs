
using System;
using System.Security.Cryptography;

namespace BlApi;



/// <summary>
/// Interface for administrative operations in the system.
/// </summary>
public interface IAdmin
{

    #region Stage 4
    /// <summary>
    /// Initializes the database with default or predefined values.
    /// </summary>
    void InitializeDB();

    /// <summary>
    /// Resets the database to its initial state.
    /// </summary>
    void ResetDB();


    /// <summary>
    /// Retrieves the maximum range setting for the system.
    /// </summary>
    /// <returns>The maximum range as a TimeSpan object.</returns>
    TimeSpan GetMaxRange();

    /// <summary>
    /// Sets the maximum range setting for the system.
    /// </summary>
    /// <param name="maxRange">The new maximum range as a TimeSpan object.</param>
    void SetMaxRange(TimeSpan maxRange);

    /// <summary>
    /// Retrieves the current system clock value.
    /// </summary>
    /// <returns>The current time as a DateTime object.</returns>
    DateTime GetClock();

    /// <summary>
    /// Advances the system clock by a specified time unit.
    /// </summary>
    /// <param name="unit">The amount of time to advance the clock, represented as a BO.TimeUnit object.</param>
    void ForwardClock(BO.TimeUnit unit);
    #endregion Stage 4





    #region Stage 5
    void AddConfigObserver(Action configObserver);
    void RemoveConfigObserver(Action configObserver);
    void AddClockObserver(Action clockObserver);
    void RemoveClockObserver(Action clockObserver);
    #endregion Stage 5

    void StartSimulator(int interval); //stage 7
    void StopSimulator(); //stage 7
}
