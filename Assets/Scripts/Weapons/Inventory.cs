using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arena
{
    public class Inventory : MonoBehaviour
    {
        private Weapon[] weapons;
        private Weapon equipped;
        private int equippedIndex = -1;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(int equippedAtStart = 0)
        {
            weapons = GetComponentsInChildren<Weapon>(true);
            foreach (Weapon weapon in weapons)
                weapon.gameObject.SetActive(false);

            Equip(equippedAtStart);
        }

        public Weapon Equip(int index)
        {
            if (weapons == null)
                return equipped;

            if (index > weapons.Length - 1)
                return equipped;
            if (equippedIndex == index)
                return equipped;
            if (equipped != null)
                equipped.gameObject.SetActive(false);
            equippedIndex = index;
            equipped = weapons[equippedIndex];
            equipped.gameObject.SetActive(true);
            return equipped;

        }

        public int GetLastIndex()
        {
            //Get last index with wrap around.
            int newIndex = equippedIndex - 1;
            if (newIndex < 0)
                newIndex = weapons.Length - 1;

            //Return.
            return newIndex;
        }

        public int GetNextIndex()
        {
            //Get next index with wrap around.
            int newIndex = equippedIndex + 1;
            if (newIndex > weapons.Length - 1)
                newIndex = 0;

            //Return.
            return newIndex;
        }

        public Weapon GetEquipped() => equipped;
        public int GetEquippedIndex() => equippedIndex;
    }
}

