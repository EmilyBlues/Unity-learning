using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;

public enum BoatAction { P, D, PP, DD, PD};
//表示下一状态需要用船运走的人

//记录船的状态以及下一步的动作
public struct NextPassenger
{
    public bool isRight;
    public BoatAction boataction;
}

public class FirstController : MonoBehaviour, SceneController, UserAction {

	readonly Vector3 water_pos = new Vector3(0,0.5f,0);

	UserGUI userGUI;

	public CoastController fromCoast;
	public CoastController toCoast;
	public BoatController boat;
	private MyCharacterController[] characters;
    private NextPassenger next;
    private int gameStatus;
    //0-等待游客上船，1-等待船出发，2-等待游客下船

    private int actionCounter;

	void Awake() {
		Director director = Director.getInstance ();
		director.currentSceneController = this;
		userGUI = gameObject.AddComponent <UserGUI>() as UserGUI;
		characters = new MyCharacterController[6];
		loadResources ();
        actionCounter = 0;
        next.boataction = BoatAction.P;
        next.isRight = true;
	}

	public void loadResources() {
		GameObject water = Instantiate (Resources.Load ("Perfabs/Water", typeof(GameObject)), water_pos, Quaternion.identity, null) as GameObject;
		water.name = "water";

		fromCoast = new CoastController ("from");
		toCoast = new CoastController ("to");
		boat = new BoatController ();

		loadCharacter ();
	}

	private void loadCharacter() {
		for (int i = 0; i < 3; i++) {
			MyCharacterController cha = new MyCharacterController ("priest");
			cha.setName("priest" + i);
			cha.setPosition (fromCoast.getEmptyPosition ());
			cha.getOnCoast (fromCoast);
			fromCoast.getOnCoast (cha);

			characters [i] = cha;
		}

		for (int i = 0; i < 3; i++) {
			MyCharacterController cha = new MyCharacterController ("devil");
			cha.setName("devil" + i);
			cha.setPosition (fromCoast.getEmptyPosition ());
			cha.getOnCoast (fromCoast);
			fromCoast.getOnCoast (cha);

			characters [i+3] = cha;
		}
	}

	public void moveBoat() {
		if (boat.isEmpty ())
			return;
		boat.Move ();
		userGUI.status = check_game_over ();
	}

	public void characterIsClicked(MyCharacterController characterCtrl) {
		if (characterCtrl.isOnBoat ()) {
			CoastController whichCoast;
			if (boat.get_to_or_from () == -1) { // to->-1; from->1
				whichCoast = toCoast;
			} else {
				whichCoast = fromCoast;
			}

			boat.GetOffBoat (characterCtrl.getName());
			characterCtrl.moveToPosition (whichCoast.getEmptyPosition ());
			characterCtrl.getOnCoast (whichCoast);
			whichCoast.getOnCoast (characterCtrl);

		}
        else {									// character on coast
			CoastController whichCoast = characterCtrl.getCoastController ();

			if (boat.getEmptyIndex () == -1) {		// boat is full
				return;
			}

			if (whichCoast.get_to_or_from () != boat.get_to_or_from ())	// boat is not on the side of character
				return;

			whichCoast.getOffCoast(characterCtrl.getName());
			characterCtrl.moveToPosition (boat.getEmptyPosition());
			characterCtrl.getOnBoat (boat);
			boat.GetOnBoat (characterCtrl);
		}
		userGUI.status = check_game_over ();
	}

	int check_game_over() {	// 0->not finish, 1->lose, 2->win
		int from_priest = 0;
		int from_devil = 0;
		int to_priest = 0;
		int to_devil = 0;

		int[] fromCount = fromCoast.getCharacterNum ();
		from_priest += fromCount[0];
		from_devil += fromCount[1];

		int[] toCount = toCoast.getCharacterNum ();
		to_priest += toCount[0];
		to_devil += toCount[1];

		if (to_priest + to_devil == 6)		// win
			return 2;

		int[] boatCount = boat.getCharacterNum ();
		if (boat.get_to_or_from () == -1) {	// boat at toCoast
			to_priest += boatCount[0];
			to_devil += boatCount[1];
		} else {	// boat at fromCoast
			from_priest += boatCount[0];
			from_devil += boatCount[1];
		}
		if (from_priest < from_devil && from_priest > 0) {		// lose
			return 1;
		}
		if (to_priest < to_devil && to_priest > 0) {
			return 1;
		}
		return 0;			// not finish
	}

	public void restart() {
		boat.reset ();
		fromCoast.reset ();
		toCoast.reset ();
		for (int i = 0; i < characters.Length; i++) {
			characters [i].reset ();
		}
	}

    public void NextMove()
    {
        Debug.Log(next.boataction);
        Debug.Log(gameStatus);
        if (gameStatus == 0)
        {
            GetNextPassager();
            if (next.isRight == true && next.boataction == BoatAction.PP)
            {
                MyCharacterController priest1 = fromCoast.FindCharacterOnTheCoast(0);
                characterIsClicked(priest1);
                MyCharacterController priest2 = fromCoast.FindCharacterOnTheCoast(0);
                characterIsClicked(priest2);
            }
            else if (next.isRight == true && next.boataction == BoatAction.P)
            {
                MyCharacterController priest1 = fromCoast.FindCharacterOnTheCoast(0);
                characterIsClicked(priest1);
            }
            else if (next.isRight == true && next.boataction == BoatAction.PD)
            {
                MyCharacterController priest1 = fromCoast.FindCharacterOnTheCoast(0);
                characterIsClicked(priest1);
                MyCharacterController devil1 = fromCoast.FindCharacterOnTheCoast(1);
                characterIsClicked(devil1);
            }
            else if (next.isRight == true && next.boataction == BoatAction.D)
            {
                MyCharacterController devil1 = fromCoast.FindCharacterOnTheCoast(1);
                characterIsClicked(devil1);
            }
            else if (next.isRight == true && next.boataction == BoatAction.DD)
            {
                MyCharacterController devil1 = fromCoast.FindCharacterOnTheCoast(1);
                characterIsClicked(devil1);
                MyCharacterController devil2 = fromCoast.FindCharacterOnTheCoast(1);
                characterIsClicked(devil2);
            }
            else if (next.isRight == false && next.boataction == BoatAction.PP)
            {
                MyCharacterController priest1 = toCoast.FindCharacterOnTheCoast(0);
                characterIsClicked(priest1);
                MyCharacterController priest2 = toCoast.FindCharacterOnTheCoast(0);
                characterIsClicked(priest2);
            }
            else if (next.isRight == false && next.boataction == BoatAction.P)
            {
                MyCharacterController priest1 = toCoast.FindCharacterOnTheCoast(0);
                characterIsClicked(priest1);
            }
            else if (next.isRight == false && next.boataction == BoatAction.PD)
            {
                MyCharacterController priest1 = toCoast.FindCharacterOnTheCoast(0);
                characterIsClicked(priest1);
                MyCharacterController devil1 = toCoast.FindCharacterOnTheCoast(1);
                characterIsClicked(devil1);
            }
            else if (next.isRight == false && next.boataction == BoatAction.D)
            {
                MyCharacterController devil1 = toCoast.FindCharacterOnTheCoast(1);
                characterIsClicked(devil1);
            }
            else if (next.isRight == false && next.boataction == BoatAction.DD)
            {
                MyCharacterController devil1 = toCoast.FindCharacterOnTheCoast(1);
                characterIsClicked(devil1);
                MyCharacterController devil2 = toCoast.FindCharacterOnTheCoast(1);
                characterIsClicked(devil2);
            }
            next.isRight = !next.isRight;
            gameStatus = 1;
        }
        else if(gameStatus == 1)
        {
            moveBoat();
            gameStatus = 2;
        }
        else if(gameStatus == 2)
        {
            MyCharacterController[] pass = boat.GetPassengerOnTheBoat();
            for(int i = 0; i < pass.Length; i++)
            {
                if(pass[i] != null)
                    characterIsClicked(pass[i]);
            }
            gameStatus = 0;
        }
    }

    private int randomValue()
    {
        float num = Random.Range(0f, 1f);
        if (num <= 0.5f)
            return 1;
        else
            return 2;
    }

    public void GetNextPassager()
    {
        int from_priest = 0;
        int from_devil = 0;
        int to_priest = 0;
        int to_devil = 0;

        int[] fromCount = fromCoast.getCharacterNum();
        from_priest += fromCount[0];
        from_devil += fromCount[1];

        int[] toCount = toCoast.getCharacterNum();
        to_priest += toCount[0];
        to_devil += toCount[1];
        if (from_priest == 3 && from_devil == 3 && next.isRight)
        {
            int turn = randomValue();
            if (turn == 1)
            {
                next.boataction = BoatAction.PD;
            }
            else
            {
                next.boataction = BoatAction.DD;
            }
        }
        else if (next.isRight == false && from_priest == 2 && from_devil == 2)
        {
            next.boataction = BoatAction.P;
        }
        else if (next.isRight == false && from_priest == 3 && from_devil == 2)
        {
            next.boataction = BoatAction.D;
        }
        else if (next.isRight == false && from_priest == 3 && from_devil == 1)
        {
            next.boataction = BoatAction.D;
        }
        else if (next.isRight == true && from_priest == 3 && from_devil == 2)
        {
            next.boataction = BoatAction.DD;
        }
        else if (next.isRight == false && from_priest == 3 && from_devil == 0)
        {
            next.boataction = BoatAction.D;
        }
        else if(next.isRight == true && from_priest == 3 && from_devil == 1)
        {
            next.boataction = BoatAction.PP;
        }
        else if (next.isRight == false && from_priest == 1 && from_devil == 1)
        {
            next.boataction = BoatAction.PD;
        }
        else if(next.isRight == true && from_priest == 2 && from_devil == 2)
        {
            next.boataction = BoatAction.PP;
        }
        else if(next.isRight == false && from_priest == 0 && from_devil == 0)
        {
            next.boataction = BoatAction.D;
        }
        else if(next.isRight == true && from_priest == 0 && from_devil == 3)
        {
            next.boataction = BoatAction.DD;
        }
        else if(next.isRight == false && from_priest == 0 && from_devil == 1)
        {
            int turn = randomValue();
            if(turn == 1)
            {
                next.boataction = BoatAction.D;
            }
            else
            {
                next.boataction = BoatAction.P;
            }
        }
        else if(next.isRight == false && from_priest == 0 && from_devil == 2)
        {
            next.boataction = BoatAction.D;
        }
        else if(next.isRight == true && from_priest == 2 && from_devil == 1)
        {
            next.boataction = BoatAction.P;
        }
        else if(next.isRight == true && from_priest == 0 && from_devil == 2)
        {
            next.boataction = BoatAction.DD;
        }
        else if(next.isRight == true && from_priest == 1 && from_devil == 1)
        {
            next.boataction = BoatAction.PD;
        }
    }
}
