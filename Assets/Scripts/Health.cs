using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int health = 1;

    /// <summary>
    /// Add to health by the amount
    /// </summary>
    /// <param name="amount"></param>
    public void Add(int amount)
    {
        health += amount;
    }

    /// <summary>
    /// Remove health by the amount
    /// </summary>
    /// <param name="amount"></param>
    public void Remove(int amount)
    {
        health -= amount;

        // if health is lower than 0, set it to 0
        if (health < 0)
        {
            health = 0;
        }
    }
}
