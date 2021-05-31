using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Control;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.SceneManagement
{

    public class Portal : MonoBehaviour
    {

        private void Start()
        {
           
        }

        enum DesitnationIdentified
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DesitnationIdentified destination;
        [SerializeField] float fadeOutTime = 4f;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float waitTime = 2f;

     

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag.Equals("Player"))
            {
                StartCoroutine(Transition());
               
            }
        }
        //transition to new scene , load scene
        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                //yield break breaks the coroutine and stops it
                yield break;
            }
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper saver = FindObjectOfType<SavingWrapper>();

            DontDestroyOnLoad(gameObject); //works only on root of scene, not child of another game object

            yield return fader.FadeOut(fadeOutTime);
       

            saver.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            saver.Load();
            
     



            //get hold of the other portal in the loaded level
            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            saver.Save();
            yield return new WaitForSeconds(waitTime); // wait for things to stablized
            
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject); //destry gameobject after scene is loaded
        }

        private Portal GetOtherPortal()
        {
            //grabs the portal in the loaded scene that has the same enum destination as the current
            Portal[] otherPortal = FindObjectsOfType<Portal>();
            foreach (Portal portal in otherPortal)
            {
                if (portal == this) continue; //just move on
                if (portal.destination == this.destination) return portal;
                else continue;

            }
            return null;
        }
        private void UpdatePlayer(Portal otherPortal)
        {
            //makes the player position equal to the other portals spawn point position. Rememeber that this script is running in the previous scene
            //as such the this.spawnPoint is the incorrect onoe
            //sometimes the navmesh agent conflicts with teleportation that is happening
            //   player.transform.position = otherPortal.spawnPoint.position; so the solution to this is to use the navmesh.warp method
            //rotation does not conflict
            //there will be another solution later on

            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false; //this simple just avoids some issues with the nav mesh agent
          
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;

        }
    }

}


