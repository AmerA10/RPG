using UnityEngine;
using System.Collections;

//cancel movement when combat
//cancel combat when movement

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;
        public void StartAction(IAction action)
        {
            if (currentAction == action)
            {
                return; //dont cancel if its the same thing running
            }
            if(currentAction != null)
            {
                Debug.Log("Cancelling: " + currentAction);
                currentAction.Cancel();
            } 
            currentAction = action;
           
        }
    
    }
}