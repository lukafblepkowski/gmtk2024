using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
public enum VineState {
    Wet,
    Dry,
    Burned,
    Bloom
}
public class VineControl : MonoBehaviour
{
    //Public
    public float jointDistance = 0.1f;
    public float dJointGap = 0.05f;
    public float sizePerUnit = 0.05f;
    public float initalsize = 0.02f;
    public float maxGrowth = 0.3f;
    public float jointDistanceShrink = 0.02f;

	public List<GameObject> startJoints = new List<GameObject>();
    public float energyGain = 0.5f;
    public float energy = 0f;

    Tank tank;

    public bool moist { get {
        if(state == VineState.Wet) return true;
        if(state == VineState.Bloom) return true;
        return false;
    } }

    public VineState state = VineState.Wet;

    float hplerper = 1;
    float bloomlerper = 0;

    bool bloomHasTriggeredUnlock = false;

    //Dynamic
    [HideInInspector]
    public List<Joint> joints = new List<Joint>();

    MeshGenerator meshGenerator;
    [HideInInspector] public BudMovement budMovement;

    public Joint jointHead { get => joints[joints.Count - 1];  }
	float v2Distance2(Vector2 v1,Vector2 v2) {
		return Mathf.Pow(v1.x - v2.x,2) + Mathf.Pow(v1.y - v2.y,2);
	}

	//Static
	public static List<VineControl> Vines = new List<VineControl>();
    void Start()
    {
        //Instantiate joints
        InstantiateJoints();


        meshGenerator = GetComponentInChildren<MeshGenerator>();
        budMovement = GetComponentInChildren<BudMovement>();
        budMovement.vine = this;
        tank = GetComponentInChildren<Tank>();

        Vines.Add(this);

		joints.Add(new Joint(budMovement.transform.position,0f,budMovement.facing));

		meshGenerator.SetJoints(joints);

        hplerper = moist ? 1f : 0f;
        bloomlerper = state == VineState.Bloom ? 1f : 0f;
    }

    void Update()
    {
        Debug.Log(state);
        UpdateJoints();
        UpdateDisplayJoints();

        if(state == VineState.Bloom && !bloomHasTriggeredUnlock) {
            bloomHasTriggeredUnlock = true;
            RoomManager.UnlockNext();
            RoomManager.CompleteCurrent();
        }

        //Dry stuff
        if(energy <= 0f) {
            if(tank.Siphon()) {
                energy += energyGain;
            } else {
                state = VineState.Dry;
            }
        } else {
            if(state == VineState.Dry) state = VineState.Wet;
        }

        hplerper = Mathf.Lerp(hplerper, moist ? 0f : 1f, Time.deltaTime);
        bloomlerper = Mathf.Lerp(bloomlerper,state == VineState.Bloom ? 1f : 0f,2f * Time.deltaTime);

        budMovement.material.SetFloat("_Wet", hplerper);
        meshGenerator.material.SetFloat("_Wet", hplerper);
        budMovement.material.SetFloat("_Bloom", bloomlerper);
        meshGenerator.material.SetFloat("_Bloom", bloomlerper);
    }

    void InstantiateJoints() {
        int i = 0;
        float traveled = 0;
        startJoints.Add(gameObject);
        while(i < startJoints.Count - 1) {
            float dist = Mathf.Sqrt(v2Distance2(startJoints[i].transform.position,startJoints[i+1].transform.position));

            traveled += jointDistance;

            if(traveled >= dist) {
                traveled -= dist;

                i += 1;

                if(i == startJoints.Count - 1) break;
            }

            float lerper = traveled / dist;

            Vector2 p1 = startJoints[i].transform.position;
            Vector2 p2 = startJoints[i+1].transform.position;
            Vector3 f1 = startJoints[i].transform.right;
            Vector3 f2 = startJoints[i+1].transform.right;

            Vector2 pos = Vector2.Lerp(p1, p2, lerper);
            Vector2 facing = Vector3.Slerp(f1, f2, lerper);
            joints.Add(new Joint(pos, 0.1f, facing));
        }
    }

    void UpdateJoints() {
        if(joints.Count == 0) return;

        float distance = Mathf.Sqrt(v2Distance2(jointHead.pos,budMovement.gameObject.transform.position));

		if(distance >= jointDistance && !budMovement.blighted) {
            float jointDistanceDebt = distance - jointDistance;

            Vector2 jointPos = Vector2.Lerp(budMovement.gameObject.transform.position,jointHead.pos,jointDistanceDebt/distance);

			joints.Add(new Joint(jointPos, 0.1f, budMovement.facing));

            energy -= 1f;
		}

        if(distance < jointDistanceShrink && budMovement.blighted) {
            joints.Remove(jointHead);
            
            energy -= 1f;
        }
	}

    void UpdateDisplayJoints() {
        if(joints.Count == 0) { return; };

        List<Joint> djoints = new List<Joint>();

        //Add tip
        djoints.Add(new Joint(budMovement.gameObject.transform.position - transform.position, 0f, budMovement.facing));

		float zed = budMovement.gameObject.transform.position.z + 0.01f;

		djoints[0].z = zed;

        float length = jointDistance - Mathf.Sqrt(v2Distance2(budMovement.gameObject.transform.position, jointHead.pos));
        float size = initalsize;

        int i = joints.Count - 1;
        while(i > 0) {
            zed += 0.001f;
            length += dJointGap;
            size += dJointGap * sizePerUnit;
            if(size > maxGrowth) size = maxGrowth;

            if(length > jointDistance) {
                //We are past the joint
                length -= jointDistance;
                i -= 1;
            } else {
                //We are not ! place djoint
                float lerper = length/jointDistance;

                Joint toAdd = Joint.Lerp(joints[i],joints[i-1],lerper);
                toAdd.z = zed;

                //Remember to pitch up the joints
                toAdd.pos -= (Vector2)transform.position;

				toAdd.width = size;
				djoints.Add(toAdd);
            }
        }

        float UVtraveled = 0;
        float currentUVsize = djoints[0].width * 4;
        for(i = 0; i < djoints.Count - 1; i++) {
            UVtraveled += dJointGap;

            float ouvsize = currentUVsize;
			if(UVtraveled >= currentUVsize) {
                //We have traveled the entire sprite
                float debted = currentUVsize;

                float lerper = UVtraveled / currentUVsize - 1f;
                currentUVsize = Mathf.Lerp(djoints[i+1].width,djoints[i].width,1f-lerper) * 4;

                UVtraveled -= debted;
            }

			djoints[i].uv = UVtraveled/ouvsize;
			djoints[i+1].uvprev = UVtraveled/ouvsize;
		}

		meshGenerator.SetJoints(djoints);
	}
}
