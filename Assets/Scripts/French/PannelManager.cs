using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PannelManager : MonoBehaviour
{

    public GameObject player1, player2, playerVariables;
    private PlayerManager player_v;
    public int turn = 1;
    public Sprite[,] tab = new Sprite[2, 10];
    public Sprite moveSprite, waitSprite, useSprite;

    public GameObject IconPrefab = null;
    Vector3 initialPosition = Vector3.zero; //  new Vector3(-3.45f, 0.45f, 0);
    bool player1Finish = false, player2Finish = false;

    // Use this for initialization
    void Start()
    {

        player_v = playerVariables.GetComponent<PlayerManager>();

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
            if (player_v.p1isSelected && player_v.p1isMove)
            {
                changeIcon(turn, 1, useSprite);
                player1Finish = true;
            }

            if (player_v.p2isSelected && player_v.p2isMove)
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
