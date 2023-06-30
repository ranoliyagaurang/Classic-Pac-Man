using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] Transform cameraFollow;
    [SerializeField] float speed;
    private Vector3 destination, target, turnAngle;
    private bool makeTurn, stopMovement = true, continousMovement = false;

    private void OnEnable()
    {
        GameManager.StateChanged += GameManager_StateChanged;
    }

    private void GameManager_StateChanged(GameState state)
    {
        switch(state)
        {
            case GameState.Running:
                stopMovement = false;
                continousMovement = true;
                target = transform.position + Offset((int)transform.eulerAngles.y);
                target = new Vector3(Mathf.RoundToInt(target.x), target.y, Mathf.RoundToInt(target.z));
                break;

            case GameState.GameOver:
                stopMovement = true;
                break;
        }
    }

    private void OnDisable()
    {
        GameManager.StateChanged -= GameManager_StateChanged;
    }

    void Update()
    {
        if (stopMovement)
            return;

        ContinousMovement();
        IntersectMovement();
    }

    void ContinousMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        cameraFollow.position = transform.position;

        if(continousMovement)
        {
            if (Vector3.Distance(transform.position, destination) < 0.1f && Vector3.Distance(transform.position, destination) > 0f)
            {
                target = transform.position + Offset((int)transform.eulerAngles.y);
                target = new Vector3(Mathf.RoundToInt(target.x), target.y, Mathf.RoundToInt(target.z));
            }
            else if (Vector3.Distance(destination, transform.position) < 0.1f && Vector3.Distance(destination, transform.position) > 0f)
            {
                target = transform.position + Offset((int)transform.eulerAngles.y);
                target = new Vector3(Mathf.RoundToInt(target.x), target.y, Mathf.RoundToInt(target.z));
            }

            if (Physics.Linecast(transform.position, target, ~ignoreLayer))
            {
                continousMovement = false;
            }
            else
            {
                destination = target;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, destination) < 0.1f && Vector3.Distance(transform.position, destination) >= 0f)
            {
                if (Physics.Linecast(destination, target, ~ignoreLayer))
                {
                    continousMovement = true;
                }
                else
                {
                    destination = target;
                    transform.eulerAngles = turnAngle;
                    continousMovement = true;
                }
            }
            else if (Vector3.Distance(destination, transform.position) < 0.1f && Vector3.Distance(destination, transform.position) >= 0f)
            {
                if (Physics.Linecast(destination, target, ~ignoreLayer))
                {
                    continousMovement = true;
                }
                else
                {
                    destination = target;
                    transform.eulerAngles = turnAngle;
                    continousMovement = true;
                }
            }
        }
    }

    bool IsIntersect(int angle)
    {
        if (!makeTurn && transform.eulerAngles.y - angle != 180 && transform.eulerAngles.y - angle != -180)
            return true;

        return false;
    }

    void IntersectMovement()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !IsIntersect(0))
        {
            turnAngle = Vector3.zero;
            target = destination + Offset((int)turnAngle.y);
            target = new Vector3(Mathf.RoundToInt(target.x), target.y, Mathf.RoundToInt(target.z));

            if(continousMovement)
                continousMovement = false;
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !IsIntersect(90))
        {
            turnAngle = new Vector3(0, 90, 0);
            target = destination + Offset((int)turnAngle.y);
            target = new Vector3(Mathf.RoundToInt(target.x), target.y, Mathf.RoundToInt(target.z));

            if (continousMovement)
                continousMovement = false;
        }
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !IsIntersect(270))
        {
            turnAngle = new Vector3(0, 270, 0);
            target = destination + Offset((int)turnAngle.y);
            target = new Vector3(Mathf.RoundToInt(target.x), target.y, Mathf.RoundToInt(target.z));

            if (continousMovement)
                continousMovement = false;
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !IsIntersect(180))
        {
            turnAngle = new Vector3(0, 180, 0);
            target = destination + Offset((int)turnAngle.y);
            target = new Vector3(Mathf.RoundToInt(target.x), target.y, Mathf.RoundToInt(target.z));

            if (continousMovement)
                continousMovement = false;
        }
    }

    Vector3Int Offset(int angle)
    {
        switch(angle)
        {
            case 0:
                return Vector3Int.forward;

            case 90:
                return -Vector3Int.left;

            case 180:
                return -Vector3Int.forward;

            case 270:
                return Vector3Int.left;

            default:
                return Vector3Int.left;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pellets"))
        {
            Destroy(other.gameObject);

            GameManager.Manager.UpdateScore();

            GameObject particle = PoolingManager.Manager.GetPool(PoolType.CollectParticle);
            particle.transform.SetParent(transform);
            particle.transform.localPosition = new Vector3(0, 0.65f, 0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Turn"))
            makeTurn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Turn"))
            makeTurn = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("AI_Ghost"))
        {
            GameManager.Manager.GameOver();

            GameObject particle = PoolingManager.Manager.GetPool(PoolType.BlastParticle);
            particle.transform.SetParent(transform);
            particle.transform.localPosition = new Vector3(0, 0.65f, 0);
        }
    }
}