using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DarkTrails;

namespace DarkTrails.Combat
{
	public class Agent : MonoBehaviour
	{
		CharacterData Data;
		public PlaceholderCharGen ModelAgent;

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
				if (Data.Equipments[(int)EQUIP.MainHand] != null)
					return Data.BaseDamage + ((Weapon)Data.Equipments[(int)EQUIP.MainHand]).MinDamage;
				else
					return Data.BaseDamage;
			}
		}

		public int maxDamage
		{
			get
			{
				if (Data.Equipments[(int)EQUIP.MainHand] != null)
					return Data.BaseDamage + ((Weapon)Data.Equipments[(int)EQUIP.MainHand]).MaxDamage;
				else
					return Data.BaseDamage;
			}
		}

		//  for placeholder chargen only, remove later

		public int WeaponType
		{
			get
			{
				if (Data.Equipments[(int)EQUIP.MainHand] != null)
					return ((Weapon)Data.Equipments[(int)EQUIP.MainHand]).WeaponType;
				else
					return -1;
			}
		}

		public int ShieldType
		{
			get
			{
				if (Data.Equipments[(int)EQUIP.OffHand] != null)
					return 1;
				else
					return 0;
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
		private float moveSpeed = 1.3f;
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
		}

		public void AssignCharacterData(CharacterData data)
		{
			Data = new CharacterData();
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

				if (((Weapon)Data.Equipments[(int)EQUIP.MainHand]).WeaponRange > 0)
				{
					attackRange = ((Weapon)Data.Equipments[(int)EQUIP.MainHand]).WeaponRange;
					//range attack
				}
				else
				{
					//melee attack
					attackRange = ((Weapon)Data.Equipments[(int)EQUIP.MainHand]).WeaponReach;
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

		public void AttackTo(Agent agentNear)
		{
			Debug.Log(this.name + " attacks to " + agentNear.name);

			int attackSkill = this.Data.GetSkillTotal((int)SKILLS.Melee);

			if (Data.Equipments[(int)EQUIP.MainHand] != null)
			{
				if (((Weapon)Data.Equipments[(int)EQUIP.MainHand]).WeaponRange > 0)
				{
					attackSkill = this.Data.GetSkillTotal((int)SKILLS.Ranged);
				}
			}

			int defenseSkill = agentNear.Data.GetSkillTotal((int)SKILLS.Dodge);

			if (agentNear.Data.Equipments[(int)EQUIP.OffHand] != null)
			{
				defenseSkill = agentNear.Data.GetSkillTotal((int)SKILLS.Shield);
			}

			if ((50 - (attackSkill - defenseSkill)) <= UnityEngine.Random.Range(0, 100))
			{
				//successfully hit
				int damage = UnityEngine.Random.Range(minDamage, (maxDamage + 1)) - agentNear.Data.ArmorClass;
				if (damage < 0) damage = 0;

				ModelAgent.Attack();
				agentNear.GotHurt(damage);
				Debug.Log(this.name + " hits " + agentNear.name + " " + damage + " damage.");
			}
			else
			{
				//fail to hit
				Debug.Log(this.name + " fails to hit " + agentNear.name);
				ModelAgent.Attack();
				agentNear.ModelAgent.Dodge();
			}

			curActionPoints -= 4;
		}

		public void GotHurt(int damage)
		{
			this.curHitPoints -= damage;
			ModelAgent.GotHit();
		}

		public void MovePath(List<Node> path)
		{
			//animator.SetBool("isWalking", true);
			ModelAgent.Run();

			movesLeft = path.Count - 1;
			curActionPoints -= movesLeft;

			doneMoving = false;
			StartCoroutine(MoveToTile(path));

		}

		public void MovePathAndAttack(List<Node> path, Agent agent)
		{
			//animator.SetBool("isWalking", true);
			ModelAgent.Run();

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

			Vector3 targetPos = new Vector3(path[i].x - CombatManager.instance.mapManager.halfMapWidth, 0f, path[i].y - CombatManager.instance.mapManager.halfMapHeight);

			if (doneMoving == false)
			{
				yield return null;
			}

			while (movesLeft > 0)
			{
				Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
				transform.position = newPos;

				transform.LookAt(targetPos);
				if ((transform.position - targetPos).sqrMagnitude < float.Epsilon)
				{
					i -= 1;
					//transform.LookAt(targetPos);
					if (i < 0)
					{
						/*
						Vector3 lastPos = new Vector3(path[0].x - CombatManager.instance.mapManager.halfMapWidth, 0f, path[0].y - CombatManager.instance.mapManager.halfMapHeight);
						transform.position = lastPos;
						*/
						//animator.SetBool("isWalking", false);
						ModelAgent.Idle();
						doneMoving = true;

						Vector3 tilePos = transform.position;
						tilePos.y = 0.015f;
						CombatManager.instance.agentTile.transform.position = tilePos;
						this.x = path[0].x;
						this.y = path[0].y;

						yield break;
					}
					else
					{
						targetPos = new Vector3(path[i].x - CombatManager.instance.mapManager.halfMapWidth, 0f, path[i].y - CombatManager.instance.mapManager.halfMapHeight);
					}
				}
				yield return null;
			}

		}
	}
}
