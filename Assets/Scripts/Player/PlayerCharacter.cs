using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileVelocity = 10f;
    [SerializeField] private float attackSpeed = 1f;

    private float timeSinceLastAttack = 0;
    public void TryRangedAttack()
    {
        float attackRate = 1 / attackSpeed;
        if (timeSinceLastAttack > attackRate)
        {
            PerformRangedAttack();
            timeSinceLastAttack = 0;
        }
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    private void PerformRangedAttack()
    {
        GameObject projectile = Instantiate(projectilePrefab,transform.position,transform.rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = transform.up * projectileVelocity;
    }
}
