using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableObject : MonoBehaviour
{    
    protected Collider2D _collider2D;
    [SerializeField]
    protected ContactFilter2D _filter2D;
    public List<Collider2D> _collidedObjects = new List<Collider2D>(1);

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        _collider2D.OverlapCollider(_filter2D, _collidedObjects);
        foreach(var o in _collidedObjects)
        {
            OnCollided(o.gameObject);
        }
    }

    protected virtual void OnCollided(GameObject collidedObject)
    {
        Debug.Log("Collided with " + collidedObject.name);
    }
}
