using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Patrols
{
    public class SceneController : System.Object, IUserAction, IAddAction, IGameStatusOp
    {
        private static SceneController instance;
        private GameModel myGameModel;
        private GameEventManager myGameEventManager;

        public static SceneController getInstance()
        {
            if (instance == null)
                instance = new SceneController();
            return instance;
        }

        internal void setGameModel(GameModel _myGameModel)
        {
            if (myGameModel == null)
            {
                myGameModel = _myGameModel;
            }
        }

        internal void setGameEventManager(GameEventManager _myGameEventManager)
        {
            if (myGameEventManager == null)
            {
                myGameEventManager = _myGameEventManager;
            }
        }

        //实现IUserAction接口
        public void heroMove(int dir)
        {
            myGameModel.HeroMove(dir);
        }

        //实现IAddAction接口
        public void addRandomMovement(GameObject sourceObj, bool isActive)
        {
            myGameModel.addRandomMovement(sourceObj, isActive);
        }

        public void addDirectMovement(GameObject sourceObj)
        {
            myGameModel.addDirectMovement(sourceObj);
        }

        //实现IGameStatusOp接口
        public int getHeroStandOnArea()
        {
            return myGameModel.GetHeroArea();
        }

        public void heroEscapeAndScore()
        {
            myGameEventManager.heroEscape();
        }

        public void patrolHitHeroAndGameover()
        {
            myGameEventManager.patrolHitHero();
        }
    }

    public class Direction
    {
        public const int UP = 0;
        public const int DOWN = 2;
        public const int LEFT = -1;
        public const int RIGHT = 1;
    }

    public class FenchLocation
    {
        public const float FenchHoriUp = 5.0f;
        public const float FenchHoriDown = 5.0f;
        public const float FenchVertLeft = -4.5f;
        public const float FenchVertRight = 4.5f;
    }

}
