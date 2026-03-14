using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();

    public void RegisterEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public void ResetEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.ResetPosition();
        }
    }
}
