using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public override IEnumerator DamageCharacter(int alienationDamage, int dreamvalueDamage, float interval)
    {
        AlienationManager.Instance.alienationLevel=(AlienationLevel)(alienationDamage+(int)AlienationManager.Instance.alienationLevel);
        DreamValueManager.Instance.ChangeHP(dreamvalueDamage);
        yield return new WaitForSeconds(interval);

        EventHandler.CallExitChasingEvent(true);
        this.gameObject.SetActive(false);
    }

    public override void ResetCharacter(Direction direction)
    {
        throw new System.NotImplementedException();
    }

    public int alienationDamage;
    public int dreamvalueDamage;
    public float interval;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
            StartCoroutine(DamageCharacter(alienationDamage, dreamvalueDamage, interval));
    }

    public override void TriggerEvent(Collider2D collsion)
    {
        if (collsion.GetComponent<Player>() != null)
            StartCoroutine(DamageCharacter(alienationDamage,dreamvalueDamage,interval));
    }

    private void OnEnable()
    {
        EventHandler.ExitChasingEvent += OnExitChasingEvent;
    }

    private void OnDisable()
    {
        EventHandler.ExitChasingEvent -= OnExitChasingEvent;
    }

    private void OnExitChasingEvent(bool obj)
    {
        if(!obj)
            this.gameObject.SetActive(false);
    }
}
