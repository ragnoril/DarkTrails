using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTrails.Combat
{
    public class AgentVisualizer : MonoBehaviour
    {
        public Text TitleText;
        public string TitleName;
        public Color AgentColor;

        public Renderer Renderer;
        public Animator Animator;

        public bool HasShield;
        public bool HasTwoHanded;
        public bool HasBow;
        public int AttackType; // 0 slash, 1 pierce

        public Transform MainHandTransform;
        public Transform OffHandTransform;
        
        // Start is called before the first frame update
        void Start()
        {
            TitleText.color = AgentColor;
            TitleText.text = TitleName;
            TitleText.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
 
        }

        public void SetItems(Character.CharacterData characterData)
        {
            if (characterData.Equipments[(int)Character.EQUIP.OffHand] != null && characterData.Equipments[(int)Character.EQUIP.OffHand].ItemType == ItemType.Shield)
                HasShield = true;
            else
                HasShield = false;

            if (characterData.Equipments[(int)Character.EQUIP.MainHand].ItemType == ItemType.Weapon)
            {
                HasTwoHanded = ((Weapon)characterData.Equipments[(int)Character.EQUIP.MainHand]).IsTwoHanded;

                if (HasTwoHanded)
                {
                    if (((Weapon)characterData.Equipments[(int)Character.EQUIP.MainHand]).WeaponRange > 0)
                    {
                        HasTwoHanded = false;
                        HasBow = true;
                    }
                }

                //AttackType = ((Weapon)characterData.Equipments[(int)Character.EQUIP.MainHand]).AttackType;
            }

            SetVariables();
        }

        public void SetVariables()
        {
            Animator.SetBool("hasShield", HasShield);
            Animator.SetBool("hasTwoHanded", HasTwoHanded);
        }

        public void Attack()
        {
            if (HasTwoHanded)
            {
                if (AttackType == 0)
                    Animator.SetTrigger("AttackTwoHandedSlash");
                else if (AttackType == 1)
                    Animator.SetTrigger("AttackTwoHandedStab");
            }
            else if (HasShield)
            {
                if (AttackType == 0)
                    Animator.SetTrigger("AttackSlash");
                else if (AttackType == 1)
                    Animator.SetTrigger("AttackStab");
                else
                    Animator.SetTrigger("AttackPunch");
            }
            else if (HasBow)
            {
                Animator.SetTrigger("ShootArrow");
            }
            else
            {
                if (AttackType == 0)
                    Animator.SetTrigger("AttackSlash");
                else if (AttackType == 1)
                    Animator.SetTrigger("AttackStab");
                else
                    Animator.SetTrigger("AttackPunch");
            }
            
        }

        public void Dodge()
        {
            if (HasShield)
                Animator.SetTrigger("Block");
            else
                Animator.SetTrigger("Dodge");
        }

        public void GotHit()
        {
            Animator.SetTrigger("GotHurt");
        }

        public void Die()
        {
            Animator.SetBool("isDead", true);
        }

        public void Walking()
        {
            Animator.SetBool("isWalking", true);
        }

        public void StopWalking()
        {
            Animator.SetBool("isWalking", false);
        }

        public void GotSelected(bool val)
        {

        }

        public void MoraleBoost()
        {
            if (Random.Range(0, 2) == 0)
                Animator.SetTrigger("Cheer_1");
            else
                Animator.SetTrigger("Cheer_2");
        }

        public void MoraleLost()
        {

            Animator.SetTrigger("Pain");
            
        }
    }
}
