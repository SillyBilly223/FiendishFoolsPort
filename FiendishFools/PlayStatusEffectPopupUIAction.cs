using System;
using System.Collections;
using UnityEngine;

namespace FiendishFools
{
    public class PlayStatusEffectPopupUIAction : CombatAction
    {
        public PlayStatusEffectPopupUIAction(int id, bool isUnitCharacter, int amount, StatusEffectInfoSO status, StatusFieldEffectPopUpType popUpType)
        {
            this._id = id;
            this._isUnitCharacter = isUnitCharacter;
            this._amount = amount;
            this._status = status;
            this._popUpType = popUpType;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            bool isUnitCharacter = this._isUnitCharacter;
            if (isUnitCharacter)
            {
                CharacterCombatUIInfo character;
                stats.combatUI._charactersInCombat.TryGetValue(this._id, out character);
                Vector3 vector = stats.combatUI._characterZone.GetCharacterPosition(character.FieldID);
                stats.combatUI.PlaySoundOnPosition(this._status.UpdatedSoundEvent, vector);
                int ppu = 32;
                Sprite sprite = Sprite.Create(this._status.icon.texture, new Rect(0f, 0f, (float)this._status.icon.texture.width, (float)this._status.icon.texture.height), new Vector2(0.5f, 0.5f), (float)ppu);
                yield return stats.combatUI._popUpHandler3D.StartStatusFieldShowcase(false, vector, this._popUpType, this._amount, sprite);
                sprite = null;
            }
            else
            {
                EnemyCombatUIInfo enemy;
                stats.combatUI._enemiesInCombat.TryGetValue(this._id, out enemy);
                Vector3 vector = stats.combatUI._enemyZone.GetEnemyPosition(enemy.FieldID);
                stats.combatUI.PlaySoundOnPosition(this._status.UpdatedSoundEvent, vector);
                int ppu2 = 32;
                Sprite sprite2 = Sprite.Create(this._status.icon.texture, new Rect(0f, 0f, (float)this._status.icon.texture.width, (float)this._status.icon.texture.height), new Vector2(0.5f, 0.5f), (float)ppu2);
                yield return stats.combatUI._popUpHandler3D.StartStatusFieldShowcase(true, vector, this._popUpType, this._amount, sprite2);
                sprite2 = null;
            }
            yield break;
        }

        public int _id;

        public bool _isUnitCharacter;

        public int _amount;

        public StatusEffectInfoSO _status;

        public StatusFieldEffectPopUpType _popUpType;
    }
}