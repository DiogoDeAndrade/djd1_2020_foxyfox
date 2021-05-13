using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{
    [SerializeField] Character.Faction faction;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Character character = collision.GetComponentInParent<Character>();
        if (character)
        {
            if (character.IsHostile(faction))
            {
                Vector2 hitDirection = character.transform.position - transform.position;

                character.DealDamage(1, hitDirection);
            }
        }
    }
}
