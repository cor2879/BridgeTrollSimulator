/**************************************************
 *  IDataBoundRow.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines an interface for data rows that can be bound to a specific type
    /// </summary>
    public interface IDataBoundRow<TDataType>
    {
        /// <summary>
        /// Binds a strongly typed item to the DataRow.
        /// </summary>
        /// <param name="collection"></param>
        void DataBind(TDataType item);
    }
}
