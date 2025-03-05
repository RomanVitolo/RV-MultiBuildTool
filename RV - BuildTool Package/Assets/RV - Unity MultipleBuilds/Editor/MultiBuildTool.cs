#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace RV_Unity_MultipleBuilds.Editor
{
    public class MultiBuildTool : EditorWindow
    {
        [MenuItem("RV - Template Tool/Builds Tool")]
        public static void OnShowTools()
        {
            GetWindow<MultiBuildTool>("RV - Build Tool");
        }

        private readonly Dictionary<BuildTarget, bool> targetsToBuild = new Dictionary<BuildTarget, bool>();
        private readonly List<BuildTarget> availableTargets = new List<BuildTarget>();

        private BuildTargetGroup GetTargetGroupForTarget(BuildTarget target) => target switch
        {
            BuildTarget.StandaloneOSX => BuildTargetGroup.Standalone,
            BuildTarget.StandaloneWindows => BuildTargetGroup.Standalone,
            BuildTarget.iOS => BuildTargetGroup.iOS,
            BuildTarget.Android => BuildTargetGroup.Android,
            BuildTarget.StandaloneWindows64 => BuildTargetGroup.Standalone,
            BuildTarget.WebGL => BuildTargetGroup.WebGL,
            BuildTarget.StandaloneLinux64 => BuildTargetGroup.Standalone,
            _ => BuildTargetGroup.Unknown
        };
        
        private void OnEnable()
        {
            availableTargets.Clear();
            var buildTargets = System.Enum.GetValues(typeof(BuildTarget));
            foreach(var buildTargetValue in buildTargets)
            {
                BuildTarget target = (BuildTarget)buildTargetValue;
                
                if (!BuildPipeline.IsBuildTargetSupported(GetTargetGroupForTarget(target), target))
                    continue;
        
                availableTargets.Add(target);
               
                targetsToBuild.TryAdd(target, false);
            }

            if (targetsToBuild.Count <= availableTargets.Count) return;
            {
                List<BuildTarget> targetsToRemove = new List<BuildTarget>();
                foreach(var target in targetsToBuild.Keys)
                {
                    if (!availableTargets.Contains(target))
                        targetsToRemove.Add(target);
                }
                
                foreach(var target in targetsToRemove)
                    targetsToBuild.Remove(target);
            }
        }
        
        private void OnGUI()
        {
            GUILayout.Label("Platforms to Build", EditorStyles.boldLabel);
        
            int numEnabled = 0;
            foreach(var target in availableTargets)
            {
                targetsToBuild[target] = EditorGUILayout.Toggle(target.ToString(), targetsToBuild[target]);
        
                if (targetsToBuild[target])
                    numEnabled++;
            }
        
            if (numEnabled > 0)
            {
                string prompt = numEnabled == 1 ? "Build 1 Platform" : $"Build {numEnabled} Platforms";
                if (GUILayout.Button(prompt))
                {
                    List<BuildTarget> selectedTargets = new List<BuildTarget>();
                    foreach (var target in availableTargets)
                    {
                        if (targetsToBuild[target])
                            selectedTargets.Add(target);
                    }
        
                    EditorCoroutineUtility.StartCoroutine(DoBuild(selectedTargets), this);
                }
            }
        }
        
        IEnumerator DoBuild(List<BuildTarget> platformsToBuild)
        {
            int buildAllProgressID = Progress.Start("Build All", "Building all selected platforms",
                Progress.Options.Sticky);
            Progress.ShowDetails();
            yield return new EditorWaitForSeconds(1f);
        
            BuildTarget originalTarget = EditorUserBuildSettings.activeBuildTarget;
           
            for (int targetIndex = 0; targetIndex < platformsToBuild.Count; ++targetIndex)
            {
                var buildTarget = platformsToBuild[targetIndex];
        
                Progress.Report(buildAllProgressID, targetIndex + 1, platformsToBuild.Count);
                int buildTaskProgressID = Progress.Start($"Build {buildTarget.ToString()}", 
                    null, Progress.Options.Sticky, buildAllProgressID);
                yield return new EditorWaitForSeconds(1f);
        
                if (!BuildIndividualTarget(buildTarget))
                {
                    Progress.Finish(buildTaskProgressID, Progress.Status.Failed);
                    Progress.Finish(buildAllProgressID, Progress.Status.Failed);
        
                    if (EditorUserBuildSettings.activeBuildTarget != originalTarget)
                        EditorUserBuildSettings.SwitchActiveBuildTargetAsync(GetTargetGroupForTarget(originalTarget), originalTarget);
        
                    yield break;
                }
        
                Progress.Finish(buildTaskProgressID);
                yield return new EditorWaitForSeconds(1f);
            }
        
            Progress.Finish(buildAllProgressID);
        
            if (EditorUserBuildSettings.activeBuildTarget != originalTarget)
                EditorUserBuildSettings.SwitchActiveBuildTargetAsync(GetTargetGroupForTarget(originalTarget), originalTarget);
        
            yield return null;
        }
        
        bool BuildIndividualTarget(BuildTarget target)
        {
            
            BuildPlayerOptions options = new BuildPlayerOptions();
           
            List<string> scenes = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                    scenes.Add(scene.path);
            }
            if (scenes.Count == 0)
            {
                Debug.LogError("No valid scenes found in build settings.");
                return false;
            }
            
            options.scenes = scenes.ToArray();
            options.target = target;
            options.targetGroup = GetTargetGroupForTarget(target);
        
            switch (target)
            {
                case BuildTarget.Android:
                {
                    string apkName = PlayerSettings.productName + ".apk";
                    options.locationPathName = System.IO.Path.Combine("Builds", target.ToString(), apkName);
                    break;
                }
                case BuildTarget.StandaloneWindows64:
                    options.locationPathName = System.IO.Path.Combine("Builds", target.ToString(), 
                        PlayerSettings.productName+".exe");
                    break;
                case BuildTarget.StandaloneLinux64:
                    options.locationPathName = System.IO.Path.Combine("Builds", target.ToString(),
                        PlayerSettings.productName+".x86_64");
                    break;
                default:
                    options.locationPathName = System.IO.Path.Combine("Builds", target.ToString(),
                        PlayerSettings.productName);
                    break;
            }
        
            options.options = BuildPipeline.BuildCanBeAppended(target, options.locationPathName) == CanAppendBuild.Yes
                ? BuildOptions.AcceptExternalModificationsToPlayer : BuildOptions.ForceEnableAssertions;
            
            EditorUserBuildSettings.SwitchActiveBuildTarget(GetTargetGroupForTarget(target), target);
            EditorApplication.RepaintHierarchyWindow();
            EditorApplication.RepaintProjectWindow();
            
            BuildReport report = BuildPipeline.BuildPlayer(options);
           
            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build for {target.ToString()} completed in {report.summary.totalTime.Seconds} seconds");
                return true;
            }
        
            Debug.LogError($"Build for {target.ToString()} failed");
            
            return false;
        }
    }
}
#endif