using BrutalAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FiendishFools.Actions
{
    public class ObsessionAction : CombatAction
    {
        public int CharacterID;

        public bool IsStalker;

        public ObsessionAction (int characterID, bool isStalker)
        {
            CharacterID = characterID;
            IsStalker = isStalker;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            int StalkerID;
            if (IsStalker)
            {
                ExtraUtils.AddStalker(CharacterID);
                StalkerID = CharacterID;
            }
            else
            {
                if (!ExtraUtils.TryRemoveObsession(CharacterID, out StalkerID))
                    yield break;
            }

            if (ExtraUtils.ContainsObsession(StalkerID))
                yield break;

            List<IUnit> Units = new List<IUnit>();

            foreach (CharacterCombat Characters in stats.CharactersOnField.Values)
            {
                if (Characters.IsAlive && Characters.ID != CharacterID || !Characters.ContainsPassiveAbility("Stalker_ID"))
                {
                    if (Characters.ContainsPassiveAbility("Obsession_ID") || ExtraUtils.UnitIsObsession(Characters.ID))
                        continue;

                    Units.Add(Characters);
                }
            }
            
            if (Units.Count == 0)
                yield break;

            IUnit ChosenUnit = Units[Random.Range(0, Units.Count)];

            ExtraUtils.AddObession(StalkerID, ChosenUnit.ID);
            ChosenUnit.AddPassiveAbility(LoadedAssetsHandler.GetPassive("Obsession_ID"));
            CombatManager._instance.AddUIAction(new ShowPassiveInformationUIAction(ChosenUnit.ID, ChosenUnit.IsUnitCharacter, "Stalked", ResourceLoader.LoadSprite("StalkedIcon")));

        }
    }
}
