using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySlime : EnemyController
{
    // Start is called before the first frame update
    public override void Start()
    {
        gameObject.SetActive(true);
        base.Start();
        agent = GetComponentInChildren<NavMeshAgent2D>();
        agent.enabled = true;
        SetNextWaypoint();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        MoveTowardsTarget();
        hitDir = agent.velocity.normalized;
        Attack();
    }

    


}
