﻿using System.Collections;
using System.Collections.Generic;
using PicaVoxel;
using UnityEngine;

public class ExplodeWithMouse : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Left click
        if (Input.GetMouseButton(0))
        {
            // Cast a ray from the camera position outward
            Ray r = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(r.origin, r.direction * 100f, Color.red, 10f);

            RaycastHit hitInfo;
            if (Physics.Raycast(r, out hitInfo)
            ) // first, cast the ray using normal Unity physics. Don't forget to include a layer mask if needed
            {
                Volume pvo = hitInfo.collider.GetComponentInParent<Volume>();
                if (pvo != null
                ) // check to see if we have hit a PicaVoxel Volume. because the Hitbox is a child object on the Volume, we use GetComponentInParent
                {
                    r = new Ray(hitInfo.point,
                        r.direction); // now create a new ray starting at the hit position of the old ray
                    for (float d = 0;
                        d < 50f;
                        d += pvo.VoxelSize *
                             0.5f) // iterate along the ray. we're using a maximum distance of 50 units here, you should adjust this to a sensible value for your scene
                    {
                        Voxel? v = pvo
                            .GetVoxelAtWorldPosition(r.GetPoint(d)); // see if there's a voxel at the ray position
                        if (v.HasValue && v.Value.Active)
                        {
                            // We have a voxel, and it's active so cause an explosion at this location
                            Batch b = pvo.Explode(r.GetPoint(d), 0.1f, 0,
                                Exploder.ExplodeValueFilterOperation.GreaterThanOrEqualTo);

                            // The delegate function here calculates a random particle velocity based on the position of the explosion
                            VoxelParticleSystem.Instance.SpawnBatch(b, pos =>
                                (pos - r.GetPoint(d)) * Random.Range(0f, 2f));

                            // Try to split the volume!
                            pvo.GetComponent<VolumeSplitter>().Split();

                            break;
                        }
                    }
                }
            }

        }
    }
}
