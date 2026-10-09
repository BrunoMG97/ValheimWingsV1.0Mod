using UnityEngine;
using System.Reflection;

namespace ValheimWings
{
    public class FlightManager : MonoBehaviour
    {
        private Player player;
        private Rigidbody rb;
        private Animator anim;
        private GameObject leftWing;
        private GameObject rightWing;
        
        private bool isFlying = false;
        private Vector3 moveInput;
        private bool isAscending, isDescending, isSprinting;

        void Awake()
        {
            player = GetComponent<Player>();
            rb = GetComponent<Rigidbody>();
            anim = (Animator)ValheimWingsPlugin.AnimatorField.GetValue(player);
        }

        void Start()
        {
            ToggleFlight();
        }

        public void ToggleFlight()
        {
            isFlying = !isFlying;
            if (isFlying)
            {
                rb.useGravity = false;
                CreateWings();
                if (anim != null) anim.SetBool("swimming", true);
            }
            else
            {
                rb.useGravity = true;
                rb.drag = 0f;
                DestroyWings();
                if (anim != null) anim.SetBool("swimming", false);
                Destroy(this); // Remove component
            }
        }

        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.N)) ToggleFlight();

            // Input capturado en Update (suave)
            Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = Camera.main.transform.right;
            
            moveInput = Vector3.zero;
            if (UnityEngine.Input.GetKey(KeyCode.W)) moveInput += camForward;
            if (UnityEngine.Input.GetKey(KeyCode.S)) moveInput -= camForward;
            if (UnityEngine.Input.GetKey(KeyCode.A)) moveInput -= camRight;
            if (UnityEngine.Input.GetKey(KeyCode.D)) moveInput += camRight;
            
            isAscending = UnityEngine.Input.GetKey(KeyCode.Space);
            isDescending = UnityEngine.Input.GetKey(KeyCode.LeftControl);
            isSprinting = UnityEngine.Input.GetKey(KeyCode.LeftShift);
        }

        void FixedUpdate()
        {
            if (!isFlying) return;

            rb.drag = 5f;

            // Velocidad
            float baseSpeed = (float)ValheimWingsPlugin.RunSpeedField.GetValue(player);
            float currentSpeed = isSprinting ? baseSpeed * 2.5f : baseSpeed * 1.5f; // +100% bonus

            // Stamina
            if (isSprinting)
            {
                float currentStamina = (float)ValheimWingsPlugin.StaminaField.GetValue(player);
                if (currentStamina > 1f) player.UseStamina(0.05f); // -90% costo
                else currentSpeed = baseSpeed * 1.5f;
            }

            // Movimiento
            rb.AddForce(moveInput * currentSpeed * 10f, ForceMode.Acceleration);

            // Vertical
            if (isAscending) rb.AddForce(Vector3.up * baseSpeed * 10f, ForceMode.Acceleration);
            if (isDescending) rb.AddForce(Vector3.down * baseSpeed * 10f, ForceMode.Acceleration);

            // Hover
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f, LayerMask.GetMask("Default", "terrain", "piece")))
            {
                if (hit.distance < 0.7f) rb.AddForce(Vector3.up * (0.7f - hit.distance) * 50f, ForceMode.Acceleration);
            }

            // Inclinación visual (Pitch)
            if (moveInput.magnitude > 0.1f)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveInput) * Quaternion.Euler(20, 0, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.fixedDeltaTime * 5f);
            }
        }

        void CreateWings()
        {
            Transform spine = transform.Find("Visual/Armature/Root/Spine"); // Fallback a player transform
            Transform anchor = spine != null ? spine : transform;
            
            leftWing = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leftWing.transform.SetParent(anchor);
            leftWing.transform.localScale = new Vector3(0.5f, 0.1f, 1.5f);
            leftWing.transform.localPosition = new Vector3(-0.5f, 0.2f, 0f);
            
            rightWing = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightWing.transform.SetParent(anchor);
            rightWing.transform.localScale = new Vector3(0.5f, 0.1f, 1.5f);
            rightWing.transform.localPosition = new Vector3(0.5f, 0.2f, 0f);
        }

        void DestroyWings()
        {
            if (leftWing) Destroy(leftWing);
            if (rightWing) Destroy(rightWing);
        }
    }
}
