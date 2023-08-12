using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG.Control
{
    public class POIBuildingState : AiState
    {
        public AiStateId GetId()
        {
            return AiStateId.POIBuilding;
        }

        public void Enter(StateMachineController controller)
        {
            //TODO: Remove POI from building action abr
            //TODO: Play Placing animation

            //TODO: Place POI on grid
            //IDEE: POI Grid erzeugen lassen, damit nur beim POI gebaut werden kann?
            //Beim aufheben kann dann einfach das grid mit allen gebauten objekten disabled werden
            GameObject Grid = GameObject.Find("Grid");
            BuildingSystem bs = Grid.GetComponent<BuildingSystem>();
            
        }

        public void Update(StateMachineController controller)
        {
        }

        public void Exit(StateMachineController controller)
        {
        }
    }
}
