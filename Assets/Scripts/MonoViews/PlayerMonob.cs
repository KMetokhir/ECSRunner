using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;


namespace Client
{
    public class PlayerMonob : MonoBehaviour
    {         
        private  EcsWorld World;      

        public void  SetWorld(EcsWorld world)
        {
            World = world;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Coin"))
            {
                
                ref var eventComp = ref World.GetPool<GetCoinEvent>().Add(World.NewEntity());

                eventComp.Transform = other.transform;
                
            }

            if (other.CompareTag("Finish"))
            {
                ref var eventComp = ref World.GetPool<FinishEvent>().Add(World.NewEntity());

                Debug.Log("finish");
            }

        }
    }
} 
