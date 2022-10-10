using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    //for water/fire/enemy
    //work with a triggered collider or more on empty children for water depth

    int damage = 25;
    private void OnTriggerEnter(Collider other)
    {
        PlayerStats ps = other.GetComponent<PlayerStats>();
        if (ps != null) ps.takeDamagePlayer(damage);
    }
}
