using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    private static float GRAVITY_FORCE = 1800;
    
    public Vector3 GravityDirection
    {
        get
        {
            if (_gravityAreas.Count == 0) {
				//Debug.Log("ZERO");
				return Vector3.down;
			}
            //_gravityAreas.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));
			Vector3 gravDir = new Vector3(0,0,0);
			foreach (GravityArea area in _gravityAreas) {
			float distance = Vector3.Distance(this.transform.position, area.transform.GetComponent<Collider>().bounds.center);
			float collider_size = area.transform.GetComponent<Collider>().bounds.size.x; //* area.transform.GetComponent<Collider>().bounds.size.y;
			Vector3 areaGravDir = area.GetGravityDirection(this);
			gravDir += areaGravDir *  1f/(distance * distance) * collider_size;
			//Debug.Log("collider center: " + area.transform.GetComponent<Collider>().bounds.center);
			//Debug.Log("player: " + this.transform.position);
			//Debug.Log("collider player dist: " + distance + " 1/dist: " + 1f/(distance * distance));
			//Debug.Log("collider size: " + collider_size);
			//Debug.Log("area grav dir: " + areaGravDir);
			Debug.Log("gravDir " + gravDir);
			//Debug.Log("collider position " +  area.transform.position + " collider center: " + area.transform.GetComponent<Collider>().bounds.center);
			//Debug.Log("closest dist " + area.transform.GetComponent<Collider>().ClosestPoint(this.transform.position) +" multiplier " + Vector3.Distance(this.transform.position, area.transform.GetComponent<Collider>().ClosestPoint(this.transform.position)));
			//Debug.Log("distance of player to center " + distance);
			}
			
            //return _gravityAreas.Last().GetGravityDirection(this).normalized;
			return gravDir.normalized;
        }
    }

    private Rigidbody _rigidbody;
    private List<GravityArea> _gravityAreas;

    void Start()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
        _gravityAreas = new List<GravityArea>();
    }
    
    void FixedUpdate()
    {
        _rigidbody.AddForce(GravityDirection * (GRAVITY_FORCE * Time.fixedDeltaTime), ForceMode.Acceleration);

        Quaternion upRotation = Quaternion.FromToRotation(transform.up, -GravityDirection);
        Quaternion newRotation = Quaternion.Slerp(_rigidbody.rotation, upRotation * _rigidbody.rotation, Time.fixedDeltaTime * 3f);;
        _rigidbody.MoveRotation(newRotation);
    }
	public List<GravityArea> GetGravityAreas => _gravityAreas;
    public void AddGravityArea(GravityArea gravityArea)
    {
        _gravityAreas.Add(gravityArea);
    }

    public void RemoveGravityArea(GravityArea gravityArea)
    {
        _gravityAreas.Remove(gravityArea);
    }
}