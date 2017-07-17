using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PannelManager : MonoBehaviour
{


    public GameObject players;
    private GameObject player1, player2;
    private PlayerManager playerManager;
    public int turn = 1;
    public Sprite[,] tab = new Sprite[2, 10];
    public Sprite moveSprite, waitSprite, useSprite;

    public GameObject IconPrefab = null;
    Vector3 initialPosition = Vector3.zero; //  new Vector3(-3.45f, 0.45f, 0);
    bool player1Finish = false, player2Finish = false;

    // Use this for initialization
    void Start()
    {
        playerManager = players.GetComponent<PlayerManager>();

        player1 = playerManager.player1;
        player2 = playerManager.player2;

    }

    // Update is called once per frame
    void Update()
    {
        //Change turn       
        if (player1Finish && player2Finish)
        {
            turn++;
            player1Finish = false;
            player2Finish = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (playerManager.p1isSelected && playerManager.p1isMove)
            {
                Debug.Log("ilDioladrobastardo e porco!!!!!");
                changeIcon(turn, 1, useSprite);
                player1Finish = true;
            }

            if (playerManager.p2isSelected && playerManager.p2isMove)
            {
                changeIcon(turn, 2, useSprite);
                player2Finish = true;
            }
        }


    }

    public void changeIcon(int turn, int player, Sprite icon)
    {
       
        if (turn == 1)
        {
            if (player == 1)
            {
                Debug.Log("ilDioladrobastardo e porco!!!!!");
                 
                Instantiate(useSprite);

                GameObject s = Instantiate(IconPrefab, this.transform);
                Image ii = s.GetComponent<Image>();
                ii.sprite = icon;

            }

            if (player == 2)
            {
                Debug.Log("laMadonna");
                Instantiate(icon, new Vector3(initialPosition.x, -initialPosition.y, initialPosition.z), Quaternion.identity);
            }
        }
    }


}
