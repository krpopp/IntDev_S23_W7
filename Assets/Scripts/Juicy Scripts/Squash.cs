using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squash : MonoBehaviour
{

    public RuntimeAnimatorController playerController;
    Animator playerAnim;

    // Start is called before the first frame update
    void Start()
    {
        //IGNORE: all of this is for setting up the animator if you don't already have it
        GameObject player = transform.parent.gameObject;
        if (player.GetComponent<Animator>() == null)
        {
            playerAnim = player.AddComponent<Animator>();
            playerAnim.runtimeAnimatorController = playerController;
        }
        else {
            playerAnim = player.GetComponent<Animator>();
        }
    }

    //set the animation trigger "landed"
    //this is setup in the editor
    public void TriggerSquash()
    {
        playerAnim.SetTrigger("landed");
    }

}
