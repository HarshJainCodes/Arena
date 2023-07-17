using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class WeaponAnimationEventHandler : MonoBehaviour
    {

        /// <summary>
        /// Equipped Weapon.
        /// </summary>
        private Weapon weapon;



        private void Awake()
        {
            //Cache. We use this one to call things on the weapon later.
            weapon = GetComponent<Weapon>();
        }


        /// <summary>
        /// Ejects a casing from this weapon. This function is called from an Animation Event.
        /// </summary>
        private void OnEjectCasing()
        {
            //Notify.
            if (weapon != null)
                weapon.EjectCasing();
        }
    }
}

