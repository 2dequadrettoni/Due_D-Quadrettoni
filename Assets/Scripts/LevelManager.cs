
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    const int LEVEL_COUNT = 18;

    List< Transform> vButtons = new List<Transform>();

	private		Sprite[]			vUnlockedNumberSprites	= null;
	private		Sprite[]			vLockedNumberSprites	= null;
	private		Sprite[]			vSelectedNumberSprites	= null;

	private		Menu				pMenu					= null;


    private void OnEnable()
    {
		pMenu = GameObject.Find( "Menu_Scripts" ).GetComponent<Menu>();

		vUnlockedNumberSprites	= Resources.LoadAll<Sprite>( "Menu/UnlockedNumberSprites" );
		vLockedNumberSprites	= Resources.LoadAll<Sprite>( "Menu/LockedNumberSprites" );
		vSelectedNumberSprites	= Resources.LoadAll<Sprite>( "Menu/SelectedNumberSprites" );
		
		
		// Get last saved level and if is invalid or the last set at zero
		int SavedLevel = SaveLoad.GetSavedlevel();
		if ( SavedLevel == -1 || SavedLevel >= LEVEL_COUNT ) {
			SaveLoad.SaveLevel( SavedLevel = 0 );
		}

		// Recupera i bottoni
        for (int i = 0; i < LEVEL_COUNT; i++)
        {

            vButtons.Add( transform.GetChild(i + 2 ) );
			vButtons[i].GetComponent<Button>().interactable = false;

			Selectable p = vButtons[i].GetComponent<Button>() as Selectable;

			p.targetGraphic = vButtons[i].GetComponent<Image>();

			SpriteState pSpriteState = new SpriteState();
			pSpriteState.highlightedSprite = Array.Find( vSelectedNumberSprites, sprite => sprite.name == ( i + 1 ).ToString() );
			p.spriteState = pSpriteState;

			vButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
			vButtons[i].GetComponent<Image>().sprite = Array.Find( vLockedNumberSprites, sprite => sprite.name == ( i + 1 ).ToString() );

        }

		// decide se il bottone è abilitato oppure no in base al fatto che il bottone corrente
		// si riferisca ad un numero di livello inferiore a quello raggiunto
        for (int i = 1; i < SavedLevel+1; i++)
        {
         
			vButtons[i].GetComponent<Button>().interactable = true;
			vButtons[i].GetComponent<Button>().onClick.AddListener( () => pMenu.Loadlevel( i + 1 ) );
			vButtons[i].GetComponent<Image>().sprite = Array.Find( vUnlockedNumberSprites, sprite => sprite.name == ( i + 1 ).ToString() );
			//	qui settare immagine del lucchetto aperto

        }

		// first level always avaiable
        vButtons[0].GetComponent<Button>().interactable = true;
		vButtons[0].GetComponent<Button>().onClick.AddListener( () => pMenu.Loadlevel( 1 ) );
		vButtons[0].GetComponent<Image>().sprite = vUnlockedNumberSprites[0];


    }


}