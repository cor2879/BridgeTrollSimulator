using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Platform
{
    /// <summary>
    /// Lightweight WebGL-only diagnostics overlay. It intentionally uses IMGUI so
    /// it can remain visible even when the game's normal Canvas/UI hierarchy fails.
    /// </summary>
    public sealed class WebGLDiagnostics : MonoBehaviour
    {
        private const int MaximumEntries = 32;
        private static WebGLDiagnostics instance;

#if UNITY_WEBGL && !UNITY_EDITOR
        private readonly Queue<string> entries = new Queue<string>();
        private string displayText = string.Empty;
        private bool expanded = true;
        private Vector2 scrollPosition;
        private bool scrollToBottom;
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            if (instance != null)
            {
                return;
            }

            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);

            var diagnosticsObject = new GameObject(nameof(WebGLDiagnostics));
            DontDestroyOnLoad(diagnosticsObject);
            instance = diagnosticsObject.AddComponent<WebGLDiagnostics>();
#endif
        }

        public static void Trace(string message)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            EnsureInstance();

            if (instance == null)
            {
                return;
            }

            instance.AddEntry(message);
            Debug.Log($"[WebGL] {message}");
#endif
        }

        public static void SnapshotUI(string reason)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            EnsureInstance();

            if (instance == null)
            {
                return;
            }

            instance.AddEntry($"--- UI SNAPSHOT: {reason} ---");
            instance.AddEntry(
                $"Screen={Screen.width}x{Screen.height} safeArea={Screen.safeArea} " +
                $"mobile={Application.isMobilePlatform} touchControls={PlatformManager.UsesMobileTouchControls}");

            var state = GameStateSystem.Instance;
            instance.AddEntry(
                state == null
                    ? "GameStateSystem=NULL"
                    : $"GameState={state.CurrentState} paused={state.IsPaused}");

            instance.AddEntry(
                EventSystem.current == null
                    ? "EventSystem.current=NULL"
                    : $"EventSystem={EventSystem.current.name} active={EventSystem.current.gameObject.activeInHierarchy} enabled={EventSystem.current.enabled}");

            var canvases = FindObjectsByType<Canvas>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            instance.AddEntry($"Canvas count={canvases.Length}");

            foreach (var canvas in canvases)
            {
                var rect = canvas.transform as RectTransform;
                var rectDescription = rect == null
                    ? string.Empty
                    : $" rect={rect.rect.width:0}x{rect.rect.height:0} scale={rect.localScale}";

                instance.AddEntry(
                    $"Canvas '{GetPath(canvas.transform)}' enabled={canvas.enabled} " +
                    $"activeSelf={canvas.gameObject.activeSelf} activeHierarchy={canvas.gameObject.activeInHierarchy} " +
                    $"mode={canvas.renderMode} order={canvas.sortingOrder}{rectDescription}");
            }

            SnapshotNamedObject("GameplayUIRoot");
            SnapshotNamedObject("DialogPanel");
            SnapshotNamedObject("DialogButtonContainer");
            SnapshotNamedObject("CombatButtonContainer");
            SnapshotNamedObject("StartScreen");
#endif
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        private static void EnsureInstance()
        {
            if (instance != null)
            {
                return;
            }

            var diagnosticsObject = new GameObject(nameof(WebGLDiagnostics));
            DontDestroyOnLoad(diagnosticsObject);
            instance = diagnosticsObject.AddComponent<WebGLDiagnostics>();
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLogMessage;
            SceneManager.sceneLoaded += HandleSceneLoaded;
            SceneManager.activeSceneChanged += HandleActiveSceneChanged;

            AddEntry("Diagnostics initialized before scene load.");
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLogMessage;
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            SceneManager.activeSceneChanged -= HandleActiveSceneChanged;
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            AddEntry($"Scene loaded: {scene.name} ({mode})");
            SnapshotUI("scene loaded");
        }

        private void HandleActiveSceneChanged(Scene previousScene, Scene nextScene)
        {
            AddEntry($"Active scene: {previousScene.name} -> {nextScene.name}");
        }

        private void HandleLogMessage(string condition, string stackTrace, LogType type)
        {
            if (type != LogType.Error &&
                type != LogType.Exception &&
                type != LogType.Assert &&
                type != LogType.Warning)
            {
                return;
            }

            var builder = new StringBuilder();
            builder.Append('[').Append(type).Append("] ").Append(condition);

            if (!string.IsNullOrWhiteSpace(stackTrace) &&
                (type == LogType.Error || type == LogType.Exception || type == LogType.Assert))
            {
                builder.AppendLine();
                builder.Append(stackTrace);
            }

            AddEntry(builder.ToString());
        }

        private void AddEntry(string message)
        {
            entries.Enqueue($"[{Time.frameCount}] {message}");

            while (entries.Count > MaximumEntries)
            {
                entries.Dequeue();
            }

            var builder = new StringBuilder();
            builder.AppendLine("BRIDGE TROLL WEBGL DIAGNOSTICS");

            foreach (var entry in entries)
            {
                builder.AppendLine(entry);
            }

            displayText = builder.ToString();
            scrollToBottom = true;
        }

        private static void SnapshotNamedObject(string objectName)
        {
            var transforms = FindObjectsByType<Transform>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            foreach (var transform in transforms)
            {
                if (transform.name != objectName)
                {
                    continue;
                }

                var rect = transform as RectTransform;
                var rectDescription = rect == null
                    ? string.Empty
                    : $" rect={rect.rect.width:0}x{rect.rect.height:0} anchored={rect.anchoredPosition} scale={rect.localScale}";

                instance.AddEntry(
                    $"Object '{GetPath(transform)}' activeSelf={transform.gameObject.activeSelf} " +
                    $"activeHierarchy={transform.gameObject.activeInHierarchy}{rectDescription}");
            }
        }

        private static string GetPath(Transform transform)
        {
            var path = transform.name;
            var parent = transform.parent;

            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            return path;
        }

        private void OnGUI()
        {
            GUI.depth = -10000;

            const float margin = 8f;
            const float buttonHeight = 34f;
            const float toggleWidth = 150f;
            const float snapshotWidth = 110f;

            if (GUI.Button(
                new Rect(Screen.width - toggleWidth - margin, margin, toggleWidth, buttonHeight),
                expanded ? "Hide Diagnostics" : "Show Diagnostics"))
            {
                expanded = !expanded;
            }

            if (!expanded)
            {
                return;
            }

            if (GUI.Button(
                new Rect(Screen.width - toggleWidth - snapshotWidth - (margin * 2f), margin, snapshotWidth, buttonHeight),
                "Snapshot UI"))
            {
                SnapshotUI("manual");
            }

            var width = Mathf.Max(320f, Screen.width * 0.78f);
            var height = Mathf.Min(620f, Screen.height * 0.62f);
            var panel = new Rect(margin, margin, width, height);

            GUI.Box(panel, GUIContent.none);

            var viewRect = new Rect(
                panel.x + 8f,
                panel.y + buttonHeight + 12f,
                panel.width - 16f,
                panel.height - buttonHeight - 20f);

            var style = new GUIStyle(GUI.skin.textArea)
            {
                wordWrap = true
            };

            var contentWidth = Mathf.Max(280f, viewRect.width - 24f);
            var contentHeight = Mathf.Max(
                viewRect.height,
                style.CalcHeight(new GUIContent(displayText), contentWidth) + 12f);

            if (scrollToBottom)
            {
                scrollPosition.y = contentHeight;
                scrollToBottom = false;
            }

            scrollPosition = GUI.BeginScrollView(
                viewRect,
                scrollPosition,
                new Rect(0f, 0f, contentWidth, contentHeight));

            GUI.TextArea(
                new Rect(0f, 0f, contentWidth, contentHeight),
                displayText,
                style);

            GUI.EndScrollView();
        }
#endif
    }
}
