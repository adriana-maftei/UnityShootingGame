using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [Header("Health system")]
    [SerializeField] HealthBar healthBar;
    [SerializeField] GameObject lowHealthImage;
    int healthLevel = 100;
    [HideInInspector] public int currentHealth, maxHealth;

    private void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    private int SetMaxHealthFromHealthLevel ()
    {
        maxHealth = healthLevel;
        return maxHealth;
    }
    public void takeDamagePlayer(int damage)
    {
        currentHealth = currentHealth - damage;
        healthBar.SetCurrentHealth(currentHealth);

        if (currentHealth <= 25)
            lowHealthImage.SetActive(true);
        else
            lowHealthImage.SetActive(false);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            SceneManager.LoadScene(2);
        }
    }
}
