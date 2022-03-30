using UnityEngine;

public class WeaponBob: MonoBehaviour
{
    public float walkingBobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    public PlayerController controller;

    float defaultPosY = 0;
    float defaultPosX = 0;
    float timer = 0;

    float wbs;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<PlayerController>();
        defaultPosY = transform.localPosition.y;
        defaultPosX = transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        wbs = walkingBobbingSpeed * (controller.body.velocity.magnitude / 7f);

        if (controller.body.velocity.magnitude > 0.1f && controller.OnGround)
        {
            //Player is moving
            timer += Time.deltaTime * wbs;
            transform.localPosition = new Vector3(
                defaultPosX + Mathf.Sin(timer* 0.5f) * bobbingAmount,
                defaultPosY + Mathf.Sin(timer * 0.5f) * bobbingAmount,
                transform.localPosition.z);

        }
        else
        {
            //Idle
            timer = 0;
            transform.localPosition = new Vector3(
                Mathf.Lerp(transform.localPosition.x, defaultPosX, Time.deltaTime * 7f),
                Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * 7f),
                transform.localPosition.z);
        }
    }
}
