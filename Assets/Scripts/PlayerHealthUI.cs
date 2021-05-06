using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField]
    private GameObject[]    hearts;

    Player player;

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }


        int nHearts = 0;
        
        if (player != null)
        {
            nHearts = player.nHearts;
        }

        for (int i = 0; i < nHearts; i++)
        {
            hearts[i].SetActive(true);
        }

        for (int i = nHearts; i < hearts.Length; i++)
        {
            hearts[i].SetActive(false);
        }
    }
}
