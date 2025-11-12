/**************************************************
 *  GameObjectExtension.cs
 *  
 *  copyright (c) 2020 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Extensions
{
    using UnityEngine;

    using System.Collections.Generic;
    using System.Linq;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;

    public static class GameObjectExtension
    {
        public static bool HasComponent<TComponent>(this GameObject gameObject)
            where TComponent : MonoBehaviour
        {
            Validator.ArgumentIsNotNull(gameObject, nameof(gameObject));

            return gameObject.GetComponent<TComponent>() != null;
        }
    }
}
