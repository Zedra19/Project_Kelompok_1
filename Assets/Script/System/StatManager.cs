using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;
    [SerializeField] private List<AttackTrigger> _attackTriggers;
    [SerializeField] private AttackTrigger _initTrigger;
    [SerializeField] private AttackTrigger _currentTrigger;

    private void Start()
    {
        _attackTriggers.ForEach(t => t.gameObject.SetActive(false));
        _initTrigger.gameObject.SetActive(true);
    }

    public void UpgradePlayerHealth(int amount)
    {
        _playerHealth.maxHealth = amount;
    }

    public void UpgardeAttackRange(int index)
    {
        _currentTrigger.gameObject.SetActive(false);
        _attackTriggers[index].gameObject.SetActive(true);
        _currentTrigger = _attackTriggers[index];
    }
}
