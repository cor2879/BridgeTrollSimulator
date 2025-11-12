/**************************************************
 *  CameraPositionRounder.cs
 *  
 *  copyright (c) 2019 Old School Games
 **************************************************/

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities
{
    using UnityEngine;
    using Unity.Cinemachine;

    using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;

    /// <summary>
    /// Extends Cinemachine to smooth / round camera positioning for pixel-perfect rendering.
    /// </summary>
    [CameraPipeline(CinemachineCore.Stage.Body)]
    [ExecuteAlways]
    public class CameraPositionRounder : CinemachineComponentBase
    {
        // Cinemachine 3.x requires these abstract members to be implemented
        public override bool IsValid => true;

        public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Body;

        // Called by the pipeline to mutate the camera state
        public override void MutateCameraState(ref CameraState state, float deltaTime)
        {
            var exactPosition = state.RawPosition;

            var roundedPosition = new Vector3(
                Mathf.Round(exactPosition.x * Constants.PixelsPerUnit) / Constants.PixelsPerUnit,
                Mathf.Round(exactPosition.y * Constants.PixelsPerUnit) / Constants.PixelsPerUnit,
                Mathf.Round(exactPosition.z * Constants.PixelsPerUnit) / Constants.PixelsPerUnit
            );

            state.PositionCorrection += roundedPosition - exactPosition;
        }
    }
}
