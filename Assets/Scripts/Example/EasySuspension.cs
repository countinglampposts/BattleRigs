using UnityEngine;

public class EasySuspension : MonoBehaviour
{
    [Range(0.1f, 20f)]
    [Tooltip("Natural frequency of the suspension springs. Describes bounciness of the suspension.")]
    public float naturalFrequency = 10;

    [Range(0f, 3f)]
    [Tooltip("Damping ratio of the suspension springs. Describes how fast the spring returns back after a bounce. ")]
    public float dampingRatio = 0.8f;

    [Range(-1f, 1f)]
    [Tooltip("The distance along the Y axis the suspension forces application point is offset below the center of mass")]
    public float forceShift = 0.03f;

    [Tooltip("Adjust the length of the suspension springs according to the natural frequency and damping ratio. When off, can cause unrealistic suspension bounce.")]
    public bool setSuspensionDistance = true;

    [Tooltip("If you need the visual wheels to be attached automatically, drag the wheel shape here.")]
    public GameObject wheelShape;

    Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        var m_Wheels = GetComponentsInChildren<WheelCollider>();

        for (int i = 0; i < m_Wheels.Length; ++i)
        {
            var wheel = m_Wheels[i];

            // Create wheel shapes only when needed.
            if (wheelShape != null)
            {
                var ws = Instantiate(wheelShape);
                ws.transform.parent = wheel.transform;
            }
        }
    }

    void Update()
    {
        // Work out the stiffness and damper parameters based on the better spring model.
        foreach (WheelCollider wheel in GetComponentsInChildren<WheelCollider>())
        {
            JointSpring spring = wheel.suspensionSpring;

            float sqrtWcSprungMass = Mathf.Sqrt(wheel.sprungMass);
            spring.spring = sqrtWcSprungMass * naturalFrequency * sqrtWcSprungMass * naturalFrequency;
            spring.damper = 2f * dampingRatio * Mathf.Sqrt(spring.spring * wheel.sprungMass);

            wheel.suspensionSpring = spring;

            Vector3 wheelRelativeBody = transform.InverseTransformPoint(wheel.transform.position);
            float distance = m_Rigidbody.centerOfMass.y - wheelRelativeBody.y + wheel.radius;

            wheel.forceAppPointDistance = distance - forceShift;

            // Make sure the spring force at maximum droop is exactly zero
            if (spring.targetPosition > 0 && setSuspensionDistance)
                wheel.suspensionDistance = wheel.sprungMass * Physics.gravity.magnitude / (spring.targetPosition * spring.spring);


            // Update visual wheels if any.
            if (wheelShape)
            {
                Quaternion r;
                Vector3 p;
                wheel.GetWorldPose(out p, out r);

                // Assume that the only child of the wheelcollider is the wheel shape.
                Transform shapeTransform = wheel.transform.GetChild(0);
                shapeTransform.position = p;
                shapeTransform.rotation = r;
            }
        }
    }

    // Uncomment this to observe how parameters change.
    /*
    void OnGUI()
    {
        foreach (WheelCollider wc in GetComponentsInChildren<WheelCollider>()) {
            GUILayout.Label (string.Format("{0} sprung: {1}, k: {2}, d: {3}", wc.name, wc.sprungMass, wc.suspensionSpring.spring, wc.suspensionSpring.damper));
        }

        GUILayout.Label ("Inertia: " + m_Rigidbody.inertiaTensor);
        GUILayout.Label ("Mass: " + m_Rigidbody.mass);
        GUILayout.Label ("Center: " + m_Rigidbody.centerOfMass);
    }
    */

}
