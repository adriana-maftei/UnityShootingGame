using UnityEngine;

public class MeleeGun : MonoBehaviour
{
    private Animator anim;
    private SoundManager soundMNG;
    [SerializeField] GameObject bloodParticle;
    float damage = 50f;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        soundMNG = GetComponent<SoundManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.Play("melee");
            soundMNG.PlaySound("shoot");
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null) enemy.takeDamageEnemy(damage);
        } 
    }
}
