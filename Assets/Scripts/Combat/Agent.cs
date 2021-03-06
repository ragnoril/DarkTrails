using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DarkTrails;

namespace DarkTrails.Combat
{
	public class Agent : MonoBehaviour
	{
        Character.CharacterData Data;
        public AgentVisualizer ModelVisualizer;

		public int x, y;
		public int teamId;

		public bool isDead;

		public int initiative
		{
			get
			{
				return Data.Initiative;
			}
		}

		public int maxHitPoints
		{
			get
			{
				return Data.MaxHitPoints;
			}
		}

		public int curHitPoints;

		public int maxActionPoints
		{
			get
			{
				return Data.MaxActionPoints;
			}
		}

		public int curActionPoints;
		public int healingRate
		{
			get
			{
				return Data.HealingRate;
			}
		}

		public int criticalChance
		{
			get
			{
				return Data.CriticalChance;
			}
		}

		public int armorClass
		{
			get
			{
				/*
				int bodyArmor = 0;

				int helmet = 0;

				int boots = 0;

				return bodyArmor + helmet + boots;
				*/

				return Data.ArmorClass;
			}
		}

		public int baseDamage
		{
			get
			{
				return Data.BaseDamage;
			}
		}

		public int minDamage
		{
			get
			{
				if (Data.Equipments[(int)Character.EQUIP.MainHand] != null)
					return Data.BaseDamage + ((Weapon)Data.Equipments[(int)Character.EQUIP.MainHand]).MinDamage;
				else
					return Data.BaseDamage;
			}
		}

		public int maxDamage
		{
			get
			{
				if (Data.Equipments[(int)Character.EQUIP.MainHand] != null)
					return Data.BaseDamage + ((Weapon)Data.Equipments[(int)Character.EQUIP.MainHand]).MaxDamage;
				else
					return Data.BaseDamage;
			}
		}
        
		public string CharacterName
		{
			get
			{
				return Data.Name;
			}
		}

		//

		public bool doneMoving;
		public bool aiDoneMoving;
        public float moveSpeed;// = 1.3f;
		private int movesLeft;
		public bool isTurnEnded;

		// Use this for initialization
		void Start()
		{
            curActionPoints = maxActionPoints;
            curHitPoints = maxHitPoints;

            doneMoving = true;
			aiDoneMoving = false;
			isTurnEnded = false;
		}

		// Update is called once per frame
		void Update()
		{
			if (curHitPoints <= 0)
				isDead = true;

			if (isDead)
			{
				CombatManager.instance.initiativeList.Remove(this);
				gameObject.SetActive(false);
				if (CombatManager.instance.selectedAgent == this)
				{
					CombatManager.instance.PassTurn();
				}

				//CombatManager.instance.PassTurn();
			}

			if (doneMoving)
			{
				//animator.SetBool("isWalking", false);

			}

			if (isTurnEnded && doneMoving)
			{
				isTurnEnded = false;
				CombatManager.instance.PassTurn();
			}

            if (ModelVisualizer.TitleText.gameObject.activeSelf)
            {
                if (CombatManager.instance.selectedAgent != this && CombatManager.instance.cursorAgent != this)
                {
                    ModelVisualizer.TitleText.gameObject.SetActive(false);
                }
            }
		}

		public void AssignCharacterData(Character.CharacterData data)
		{
			Data = new Character.CharacterData();
			Data.CopyFrom(data);
		}

		public void MakeAIMove()
		{
			Agent agentNearest = GetNearestEnemyAgent();

			if (agentNearest != null)
			{
				CombatManager.instance.pathFinder.SetStartNode(this.x, this.y);
				CombatManager.instance.pathFinder.SetGoalNode(agentNearest.x, agentNearest.y);
				CombatManager.instance.pathFinder.StartSearch(CombatManager.instance.CreateUpdatedMap(-1, -1));
				CombatManager.instance.pathFinder.GetPath();
				int distToWalk = CombatManager.instance.pathFinder.GetGoalScore();

				int attackRange = 1;

				if (((Weapon)Data.Equipments[(int)Character.EQUIP.MainHand]).WeaponRange > 0)
				{
					attackRange = ((Weapon)Data.Equipments[(int)Character.EQUIP.MainHand]).WeaponRange;
					//range attack
				}
				else
				{
					//melee attack
					attackRange = ((Weapon)Data.Equipments[(int)Character.EQUIP.MainHand]).WeaponReach;
				}

				//float dist = Vector3.Distance(transform.position, agentNearest.transform.position);
				if (distToWalk <= attackRange)
				{
					// get action point to attack from weapon later
					if (curActionPoints >= 4)
					{
						AttackTo(agentNearest);
					}
					else
					{
						isTurnEnded = true;
					}
				}
				else
				{
					if (this.curActionPoints == 0)
					{
						isTurnEnded = true;
						return;
					}
					else if (distToWalk > this.curActionPoints)
					{
						float posX = CombatManager.instance.pathFinder.finalPath[CombatManager.instance.pathFinder.finalPath.Count - 1 - this.curActionPoints].x;
						float posY = CombatManager.instance.pathFinder.finalPath[CombatManager.instance.pathFinder.finalPath.Count - 1 - this.curActionPoints].y;

						CombatManager.instance.pathFinder.SetStartNode(this.x, this.y);
						CombatManager.instance.pathFinder.SetGoalNode((int)posX, (int)posY);
						CombatManager.instance.pathFinder.StartSearch(CombatManager.instance.CreateUpdatedMap(-1, -1));
						CombatManager.instance.pathFinder.GetPath();

						this.x = (int)posX;
						this.y = (int)posY;

						MovePath(CombatManager.instance.pathFinder.finalPath);
						isTurnEnded = true;
					}
					else
					{
						int newX = CombatManager.instance.pathFinder.finalPath[1].x;
						int newY = CombatManager.instance.pathFinder.finalPath[1].y;

						CombatManager.instance.pathFinder.SetStartNode(this.x, this.y);
						CombatManager.instance.pathFinder.SetGoalNode(newX, newY);
						CombatManager.instance.pathFinder.StartSearch(CombatManager.instance.CreateUpdatedMap(-1, -1));
						CombatManager.instance.pathFinder.GetPath();

						if (curActionPoints >= CombatManager.instance.pathFinder.GetGoalScore() + 4)
						{
							MovePathAndAttack(CombatManager.instance.pathFinder.finalPath, agentNearest);
						}
						else if (curActionPoints >= CombatManager.instance.pathFinder.GetGoalScore())
						{
							MovePath(CombatManager.instance.pathFinder.finalPath);
						}

						isTurnEnded = true;
					}

				}
			}
			else
			{
				isTurnEnded = true;
				doneMoving = true;
			}

			CombatManager.instance.UpdateUI();

			if (isTurnEnded == false && doneMoving)
			{
				MakeAIMove();
			}

		}

		private Agent GetNearestEnemyAgent()
		{
			Agent nearest = null;
			float nearestDist = 9999f;

			if (teamId == 0)
			{
				foreach (Agent agent in CombatManager.instance.teamEnemy)
				{
					if (agent.isDead) continue;

					float dist = Vector3.Distance(transform.position, agent.transform.position);
					if (dist <= nearestDist) nearest = agent;
				}
			}
			else if (teamId == 1)
			{
				foreach (Agent agent in CombatManager.instance.teamPlayer)
				{
					if (agent.isDead) continue;

					float dist = Vector3.Distance(transform.position, agent.transform.position);
					if (dist <= nearestDist)
					{
						nearest = agent;
						nearestDist = dist;
					}
				}
			}

			return nearest;
		}

        public int CalculateToHitPossibility(Agent agentNear)
        {
            int attackSkill = this.Data.GetSkillTotal((int)Character.SKILLS.Melee);

            if (Data.Equipments[(int)Character.EQUIP.MainHand] != null)
            {
                if (((Weapon)Data.Equipments[(int)Character.EQUIP.MainHand]).WeaponRange > 0)
                {
                    attackSkill = this.Data.GetSkillTotal((int)Character.SKILLS.Ranged);
                }
            }

            int defenseSkill = agentNear.Data.GetSkillTotal((int)Character.SKILLS.Dodge);

            if (agentNear.Data.Equipments[(int)Character.EQUIP.OffHand] != null)
            {
                defenseSkill = agentNear.Data.GetSkillTotal((int)Character.SKILLS.Shield);
            }

            int possibility = 50;
            possibility += (attackSkill - defenseSkill);

            return possibility;
        }

		public void AttackTo(Agent agentNear)
		{
            string logMsg = this.name + " attacks to " + agentNear.name;

            Vector3 targetPos = agentNear.transform.position;
            targetPos.y = 0.015f;
            transform.LookAt(targetPos);

            targetPos = transform.position;
            targetPos.y = 0.015f;
            agentNear.transform.LookAt(targetPos);

            int attackSkill = this.Data.GetSkillTotal((int)Character.SKILLS.Melee);

			if (Data.Equipments[(int)Character.EQUIP.MainHand] != null)
			{
				if (((Weapon)Data.Equipments[(int)Character.EQUIP.MainHand]).WeaponRange > 0)
				{
					attackSkill = this.Data.GetSkillTotal((int)Character.SKILLS.Ranged);
				}
			}

			int defenseSkill = agentNear.Data.GetSkillTotal((int)Character.SKILLS.Dodge);

			if (agentNear.Data.Equipments[(int)Character.EQUIP.OffHand] != null)
			{
				defenseSkill = agentNear.Data.GetSkillTotal((int)Character.SKILLS.Shield);
			}

			if ((50 - (attackSkill - defenseSkill)) <= UnityEngine.Random.Range(0, 100))
			{
				//successfully hit
				int damage = UnityEngine.Random.Range(minDamage, (maxDamage + 1)) - agentNear.Data.ArmorClass;
				if (damage < 0) damage = 0;

				ModelVisualizer.Attack();
				agentNear.GotHurt(damage);
                logMsg += " and hits " + agentNear.name + " " + damage + " damage.";
			}
			else
			{
                //fail to hit
                logMsg += " but fails to hit.";

                ModelVisualizer.Attack();
				agentNear.ModelVisualizer.Dodge();
			}

            CombatManager.instance.uiManager.AddCombatLog(logMsg);
			curActionPoints -= 4;
		}

		public void GotHurt(int damage)
		{
			this.curHitPoints -= damage;
            ModelVisualizer.GotHit();
		}

		public void MovePath(List<Node> path)
		{
            //animator.SetBool("isWalking", true);
            ModelVisualizer.Walking();

			movesLeft = path.Count - 1;
			curActionPoints -= movesLeft;

			doneMoving = false;
			StartCoroutine(MoveToTile(path));

		}

		public void MovePathAndAttack(List<Node> path, Agent agent)
		{
			//animator.SetBool("isWalking", true);
			ModelVisualizer.Walking();

			movesLeft = path.Count - 1;

			doneMoving = false;
			curActionPoints -= (path.Count - 1);
			StartCoroutine(MoveToTileToAttack(path, agent));
		}

		protected IEnumerator MoveToTileToAttack(List<Node> path, Agent agent)
		{
			yield return StartCoroutine(MoveToTile(path));
			yield return null;

			AttackTo(agent);
		}

		protected IEnumerator MoveToTile(List<Node> path)
		{
			int i = movesLeft - 1;

            //for 2d
            //Vector3 targetPos = new Vector3(path[i].x - CombatManager.instance.mapManager.halfMapWidth, path[i].y - CombatManager.instance.mapManager.halfMapHeight, 0f);
            //for 3d
            Vector3 targetPos = new Vector3(path[i].x - CombatManager.instance.mapManager.halfMapWidth, 0.015f, path[i].y - CombatManager.instance.mapManager.halfMapHeight);

            if (doneMoving == false)
			{
				yield return null;
			}

			while (movesLeft > 0)
			{

				Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
				transform.position = newPos;
                
				transform.LookAt(targetPos);
				if ((transform.position - targetPos).sqrMagnitude < 0.0001f /*float.Epsilon*/)
				{
					i -= 1;
                    //for 3d
					transform.LookAt(targetPos);
					if (i < 0)
					{
						//for 3d
						Vector3 lastPos = new Vector3(path[0].x - CombatManager.instance.mapManager.halfMapWidth, 0.015f, path[0].y - CombatManager.instance.mapManager.halfMapHeight);
						transform.position = lastPos;					

						ModelVisualizer.StopWalking();
						doneMoving = true;

                        //for 2d
                        /*
						Vector3 tilePos = transform.position;
						tilePos.y -= 0.25f;
						CombatManager.instance.agentTile.transform.position = tilePos;
                        */
                        //for 3d
						Vector3 tilePos = transform.position;
						tilePos.y = 0.015f;
						CombatManager.instance.agentTile.transform.position = tilePos;

                        this.x = path[0].x;
						this.y = path[0].y;

						yield break;
					}
					else
					{
                        //for 2d
						//targetPos = new Vector3(path[i].x - CombatManager.instance.mapManager.halfMapWidth, path[i].y - CombatManager.instance.mapManager.halfMapHeight, 0f);
                        //for 3d
                        targetPos = new Vector3(path[i].x - CombatManager.instance.mapManager.halfMapWidth, 0.015f, path[i].y - CombatManager.instance.mapManager.halfMapHeight);
                    }
				}
				yield return null;
			}

		}
	}
}
