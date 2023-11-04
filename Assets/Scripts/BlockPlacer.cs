using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NinjaTools;

public class BlockPlacer : NinjaMonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject previewPrefab;
    public LayerMask layersToIgnore;
    public LayerMask layersToHit;
    GameObject previewObj;

    [Header("Gizmos")]
    public float rayDistance = 5f;
    public float nearbyRadius = 0.5f;
    public float xDifference = 2f;
    public float zDifference = 2f;
    public Vector3 lastRayHitPos;

    private void OnDrawGizmos()
    {
        var cam = Camera.main.transform;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(cam.position, cam.forward * rayDistance);
        Gizmos.DrawWireSphere(lastRayHitPos, nearbyRadius);
    }

    private void Start()
    {
        previewObj = Instantiate(previewPrefab);
    }

    private void Update()
    {
        var cam = Camera.main.transform;
        RaycastHit rayCast;

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(cam.position, cam.forward * rayDistance, out rayCast, 10))
        {
            HandleBlockPlacement(rayCast);
        }

        if (Physics.Raycast(cam.position, cam.forward * rayDistance, out rayCast, 10, layersToHit))
        {
            HandleBlockPreview(rayCast);
        }
    }

    private void HandleBlockPlacement(RaycastHit rayCast)
    {
        lastRayHitPos = rayCast.point;
        var hit = rayCast.collider;
        if (previewObj.activeInHierarchy)
        {
            previewObj.SetActive(false);
            Instantiate(blockPrefab, previewObj.transform.position, Quaternion.identity);
        }
        else
        {
            previewObj.SetActive(true);
        }
    }

    private void HandleBlockPreview(RaycastHit rayCast)
    {
        if (previewObj.activeInHierarchy)
        {
            var hits = Physics.SphereCastAll(rayCast.point, nearbyRadius, rayCast.transform.forward);
            List<GroundBlock> nearbyBlocks = new List<GroundBlock>();

            foreach (var hit in hits)
            {
                var block = hit.collider.GetComponentInParent<GroundBlock>();
                if (block)
                {
                    nearbyBlocks.Add(block);
                }
            }

            if (nearbyBlocks.Count > 0)
            {
                var nearbyBlock = nearbyBlocks[0];
                var direction = (nearbyBlock.transform.position - rayCast.point).normalized;
                var attachPos = nearbyBlock.transform.position;

                if (HandleSnap(ref attachPos, direction))
                {
                    previewObj.transform.position = attachPos;
                    logd("", "RayCast=" + rayCast.collider.name + " => Direction=" + direction, true);
                }
            }
            else
            {
                previewObj.transform.position = rayCast.point;
            }
        }
        logd("", "RayCast=" + rayCast.collider.name, true);
    }
    private bool HandleSnap(ref Vector3 attachPos, Vector3 direction) {
        var dirX = direction.x;
        var dirZ = direction.z;

        if (dirX > 0.85f) {
            attachPos.x -= xDifference;
        } else if (dirX < -0.85f) {
            attachPos.x += xDifference;
        } else if (dirZ < 0.8f || dirZ > -0.8f) {
            if (dirX > 0.6f) {
                attachPos.x -= xDifference;
            } else if (dirX < -0.6f) {
                attachPos.x += xDifference;
            }
        }

        if (dirZ > 0.85f) {
            attachPos.z -= zDifference;
        } else if (dirZ < -0.85f) {
            attachPos.z += zDifference;
        } else if (dirX < 0.8f || dirX > -0.8f) {
            if (dirZ > 0.6f) {
                attachPos.z -= zDifference;
            } else if (dirZ < -0.6f) {
                attachPos.z += zDifference;
            }
        }

        return true;
        }
    }