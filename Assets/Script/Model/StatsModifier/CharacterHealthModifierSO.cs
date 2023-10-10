using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        PlayerBasicLogic health = character.GetComponent<PlayerBasicLogic>();
        if (health != null)
        {
            health.Health += val;
        }
    }

}
