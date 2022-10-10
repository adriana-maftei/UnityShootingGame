using UnityEngine;

public class OpenChestAnimation : MonoBehaviour
{
    Animator anim;
    //uncheck Loop time box
    //check Has Exit Time box and set exit time at 0.01

    private void Start() => anim = GetComponent<Animator>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
            anim.SetTrigger("open");
    }
    private void OnTriggerExit(Collider other) => anim.enabled = true;
    void pauseAnimationEvent() => anim.enabled = false;
}
