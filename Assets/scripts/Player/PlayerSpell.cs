﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This controls the player spellcard
public class PlayerSpell : MonoBehaviour
{
    public Spell defaultSpell;
    public Player player;
    public Animator animator;
    public float animationDelay = 0.5f;
    void Start()
    {
        player.ActivateSpell += CastSpell;
    }
    private void OnDestroy()
    {
        player.ActivateSpell -= CastSpell;
    }

    // Update is called once per frame
    void CastSpell() {
        if (PlayerStats.bombCount > 0) {
            PlayerStats.UseBomb();
            Instantiate(defaultSpell, transform.position, Quaternion.identity).SetDamage(PlayerStats.bombDamage*PlayerStats.bombEffectiveness);

            StartCoroutine(Invul());
        }
        
    }

    IEnumerator Invul() {
        player.ActivateSpell -= CastSpell;
        PlayerStats.SetPlayerInvulnerable();
        animator.SetBool("IsBlinking", true);
        yield return new WaitForSeconds(defaultSpell.invulTimer - animationDelay);
        animator.SetBool("IsBlinking", false);
        yield return new WaitForSeconds(animationDelay);
        PlayerStats.SetPlayerVulnerable();
        player.ActivateSpell += CastSpell;
    }
}
