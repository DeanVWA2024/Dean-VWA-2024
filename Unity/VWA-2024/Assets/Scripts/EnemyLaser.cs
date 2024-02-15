using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : EnemyController
{
    private LineRenderer lineRenderer;
    public float deltaTemp;

    // Start is called before the first frame update
    public override void Start()
    {
        gameObject.SetActive(true);
        base.Start();
        // Get the LineRenderer component attached to this GameObject
        lineRenderer = GetComponentInChildren<LineRenderer>();

        // Set the positions of the line renderer (start and end points)
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        hitDir = (playerTransform.position - transform.position).normalized;
        Attack();

        // Set the positions of the line renderer
        if(CanSeePlayer() == true)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, playerTransform.position);
            lineRenderer.SetPosition(1, transform.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (CanSeePlayer() == true)
        {
            playerTransform.GetComponent<Ressources>().ChangeTemp(deltaTemp, 0, 0);
        }
    }
}
