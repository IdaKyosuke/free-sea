using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyStatus", menuName = "ScriptableObject/Create EnemyStatus")]
public class Enemy_Status : ScriptableObject
{
	[SerializeField] int hp = 0;
	[SerializeField] int exp = 0;
	[SerializeField] int damage = 0;

	// ----- Getä÷êî -----
	public int GetHp() {  return hp; }
	public int GetExp() { return exp; }
	public int GetDamage() { return damage; }
}
