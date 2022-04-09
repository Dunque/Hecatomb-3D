using UnityEngine;

    /// <summary>
    /// This script is used to display real-time statistics to help with tweaking and debugging.
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class PlayerDebug : MonoBehaviour
    {
        [Tooltip("How many times per second to update stats")]
        [SerializeField] private float m_RefreshRate = 4;

        private int m_FrameCount = 0;
        private float m_Time = 0;
        private float m_FPS = 0;
        private float m_TopSpeed = 0;
        private PlayerController m_Player;

        private void Start()
        {
            m_Player = GetComponent<PlayerController>();
        }

        private void LateUpdate()
        {
            // Calculate frames-per-second.
            m_FrameCount++;
            m_Time += Time.deltaTime;
            if (m_Time > 1.0 / m_RefreshRate)
            {
                m_FPS = Mathf.Round(m_FrameCount / m_Time);
                m_FrameCount = 0;
                m_Time -= 1.0f / m_RefreshRate;
            }

            // Calculate top velocity.
            if (m_Player.body.velocity.magnitude > m_TopSpeed)
            {
                m_TopSpeed = m_Player.body.velocity.magnitude;
            }
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(0, 0, 130, 60),
                "FPS: " + m_FPS + "\n" +
                "Speed: " + Mathf.Round(m_Player.body.velocity.magnitude * 100) / 100 + " (ups)\n" +
                "Top: " + Mathf.Round(m_TopSpeed * 100) / 100 + " (ups)");
        }
    }