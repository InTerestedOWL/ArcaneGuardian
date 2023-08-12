using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AG.Control
{
    public class POIBuildingState : AiState
    {
        Boolean arrived = false;
        public AiStateId GetId()
        {
            return AiStateId.POIBuilding;
        }

        public void Enter(StateMachineController controller)
        {
            //TODO: Remove POI from building action abr
            //TODO: Play Placing animation
            Debug.Log("Building state");
            //TODO: Place POI on grid
            GameObject poi_building = GameObject.FindWithTag("POI_Building");
            Vector3 tar_pos = poi_building.transform.position;
            controller.movement.navMeshAgent.stoppingDistance = 0;
            controller.movement.DoMovement(tar_pos);

            //IDEE: POI Grid erzeugen lassen, damit nur beim POI gebaut werden kann?
            //Beim aufheben kann dann einfach das grid mit allen gebauten objekten disabled werden
            GameObject Grid = GameObject.Find("Grid");
            BuildingSystem bs = Grid.GetComponent<BuildingSystem>();
            
        }

        public void Update(StateMachineController controller)
        {
            if(!arrived && controller.movement.navMeshAgent.remainingDistance < 0.01){
                Debug.Log("I have arrived in yo mama");
                arrived = true;
            }
            
        }

        public void Exit(StateMachineController controller)
        {
            arrived = false;
        }
    }
}
