using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class CharacterController2D : MonoBehaviour
{
	public LayerMask collideLayer;
	[HideInInspector] public CircleCollider2D characterCollider;

	private Vector2 moveDirection;
	private RaycastHit2D[] hits = new RaycastHit2D[20];

	public delegate void colliderDelegate(Collider2D collider);
	public colliderDelegate OnControllerColliderHit;

	private RaycastHit2D hit;

	private void Awake()
    {
		if(characterCollider == null)
        {
			characterCollider = GetComponent<CircleCollider2D>();
		}
	}
	public void move(Vector2 _moveDirection)
    {
		Vector2 normalVector;
		float dotResult;
		Vector2 dotResultVector = Vector2.zero;
		Vector2 newMoveVector;
		Vector2 dotResultSum = Vector2.zero;
		int hitNum;
		float moveSpeed = _moveDirection.magnitude;
		moveDirection = _moveDirection.normalized;

		newMoveVector = moveDirection*moveSpeed;

		hitNum = Physics2D.CircleCastNonAlloc((Vector2)characterCollider.transform.position+characterCollider.offset,characterCollider.radius+0.05f,moveDirection,hits,Time.deltaTime*moveSpeed,collideLayer);
		for(int i=0 ; i<hits.Length && i<hitNum;i++)
        {
			hit = hits[i];

			if((hit.collider!= null & hit.collider != characterCollider)){
				OnControllerColliderHit(hit.collider);	
				normalVector = hit.normal;
				dotResult = Vector2.Dot(normalVector,moveSpeed*moveDirection);
				if(dotResult<0){
					dotResultVector = normalVector*(dotResult);
					dotResultSum = dotResultSum + dotResultVector;
				}
			}
		}

		newMoveVector = newMoveVector - dotResultVector;

		hitNum = Physics2D.CircleCastNonAlloc((Vector2)characterCollider.transform.position+characterCollider.offset,characterCollider.radius+0.03f,newMoveVector,hits,Time.deltaTime*moveSpeed,collideLayer);

		for(int i=0 ; i<hits.Length && i< hitNum;i++)
        {
			hit = hits[i];

			if((hit.collider != characterCollider))
            {
				OnControllerColliderHit(hit.collider);
				normalVector = hit.normal;
				dotResult = Vector2.Dot(normalVector,moveSpeed*newMoveVector);

				if(dotResult<-0.01f)
                {
					newMoveVector = Vector2.zero;
					break;
				}
			}
		}
			
		transform.position = (Vector2)transform.position+(newMoveVector*Time.deltaTime);
	}
}
