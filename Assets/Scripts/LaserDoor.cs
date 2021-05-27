using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDoor : MonoBehaviour
{
    [SerializeField] float          timeBetweenSwitch = 2.0f;
    [SerializeField] LineRenderer   laserObject;
    [SerializeField] Collider2D     blockCollider;

    [SerializeField] string         blockLayer;
    [SerializeField] string         notBlockLayer;

    float switchTimer;

    // Start is called before the first frame update
    void Start()
    {
        switchTimer = timeBetweenSwitch;
    }

    // Update is called once per frame
    void Update()
    {
        switchTimer = switchTimer - Time.deltaTime;

        if (switchTimer <= 0)
        {
            laserObject.enabled = !laserObject.enabled;

            if (laserObject.enabled)
            {
                int layerId = LayerMask.NameToLayer(blockLayer);
                gameObject.layer = layerId;
            }
            else
            {
                int layerId = LayerMask.NameToLayer(notBlockLayer);
                gameObject.layer = layerId;
            }

//            blockCollider.enabled = laserObject.enabled;
            switchTimer = timeBetweenSwitch;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!laserObject.enabled) return;

        Player player = collision.GetComponentInParent<Player>();
        if (player != null)
        {
            player.DealDamage(1, (transform.position - player.transform.position).normalized);
        }
    }
}
