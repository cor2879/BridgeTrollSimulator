using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Combat
{
    [CreateAssetMenu(menuName = "BridgeTroll/Combat/Disposition Matrix")]
    public class DispositionMatrix : ScriptableObject
    {
        [Serializable]
        public class FactionDisposition
        {
            public CombatFaction faction;
            public List<CombatFaction> hostileTo;
        }

        [SerializeField]
        private List<FactionDisposition> dispositions;

        private Dictionary<CombatFaction, HashSet<CombatFaction>> lookup;

        private void OnEnable()
        {
            lookup = new Dictionary<CombatFaction, HashSet<CombatFaction>>();

            foreach (var d in dispositions)
            {
                lookup[d.faction] = new HashSet<CombatFaction>(d.hostileTo);
            }
        }

        public  bool IsHostile(CombatFaction a, CombatFaction b)
        {
            if (!lookup.TryGetValue(a, out var set))
            {
                return false;
            }

            return set.Contains(b);
        }
    }
}