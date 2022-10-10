using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [SerializeField] GameObject bloodParticle;
    float damage = 35f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null) enemy.takeDamageEnemy(damage);
        } 
    }
}
