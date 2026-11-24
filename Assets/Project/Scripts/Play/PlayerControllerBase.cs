using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBase : MonoBehaviour
{
    private const float MoveSpeed = 14f;
    private const float RotateSpeed = 500f;
    private const int InitialEnergy = 3;

    private Rigidbody2D rb;
    private bool isTricking = false;
    private float moveInput = 0f;
    private float rotateInput = 0f;
    private int energy;

    [SerializeField] private GameObject[] energyLampOn;
    [SerializeField] private GameObject[] energyLampOff;

    [SerializeField] protected KeyCode moveUpKey;
    [SerializeField] protected KeyCode moveDownKey;
    [SerializeField] protected KeyCode rotateRightKey;
    [SerializeField] protected KeyCode rotateLeftKey;
    [SerializeField] protected KeyCode trickKey;

    public bool IsTricking => isTricking;
    public float MoveInput => moveInput;
    public int Energy => energy;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        energy = InitialEnergy;
    }

    protected virtual void Update()
    {
        isTricking = Input.GetKey(trickKey);
        moveInput = PaddleInput.ResolveAxis(Input.GetKey(moveUpKey), Input.GetKey(moveDownKey));
        // 右回転は時計回り（角速度がマイナス）
        rotateInput = PaddleInput.ResolveAxis(Input.GetKey(rotateLeftKey), Input.GetKey(rotateRightKey));
    }

    protected virtual void FixedUpdate()
    {
        rb.velocity = new Vector2(0, moveInput * MoveSpeed);
        rb.angularVelocity = rotateInput * RotateSpeed;
    }

    public virtual void DecreaseEnergy()
    {
        energy--;
        energyLampOn[energy].SetActive(false);
        energyLampOff[energy].SetActive(true);
    }
}
