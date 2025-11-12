/**************************************************
 *  IDataBoundTable.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines an interface for data tables that can be bound to a specific type
    /// </summary>
    public interface IDataBoundTable<TDataType>
    {
        /// <summary>
        /// Binds a strongly typed collection to the DataTable
        /// </summary>
        /// <param name="collection"></param>
        void DataBind(IEnumerable<TDataType> collection);
    }
}
