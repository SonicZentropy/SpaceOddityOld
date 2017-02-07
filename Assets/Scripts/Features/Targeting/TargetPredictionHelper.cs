// // /**
// //  * TargetPredictionHelper.cs
// //  * Dylan Bailey
// //  * 20170206
// // */
namespace Features.Targeting
{
    using UnityEngine;

    public static class TargetPredictionHelper
    {
        public static Vector3 PredictFuturePosition(Vector3 currentPosition, Vector3 currentVelocity, float predictionTime)
        {
            return currentPosition + (currentVelocity * predictionTime);
        }

        public static Vector3 PredictFuturePosition(Transform obj, Vector3 currentVelocity, float predictionTime)
        {
            return obj.position + (currentVelocity * predictionTime);
        }

        public static Vector3 PredictFuturePosition(Transform obj, Rigidbody objRigidbody, float predictionTime)
        {
            return obj.position + (objRigidbody.velocity * predictionTime);
        }
    }
}

#if NEVER_DEFINED
if (_quarry == null)
            {
                enabled = false;
                return Vector3.zero;
            }

            var force = Vector3.zero;
            var offset = _quarry.Position - Vehicle.Position;
            var distance = offset.magnitude;
            var radius = Vehicle.Radius + _quarry.Radius + _acceptableDistance;

            if (!(distance > radius)) return force;

            var unitOffset = offset / distance;

            // how parallel are the paths of "this" and the quarry
            // (1 means parallel, 0 is pependicular, -1 is anti-parallel)
            var parallelness = Vector3.Dot(transform.forward, _quarry.transform.forward);

            // how "forward" is the direction to the quarry
            // (1 means dead ahead, 0 is directly to the side, -1 is straight back)
            var forwardness = Vector3.Dot(transform.forward, unitOffset);

            var directTravelTime = distance / Vehicle.Speed;
            // While we could parametrize this value, if we care about forward/backwards
            // these values are appropriate enough.
            var f = OpenSteerUtility.IntervalComparison(forwardness, -0.707f, 0.707f);
            var p = OpenSteerUtility.IntervalComparison(parallelness, -0.707f, 0.707f);

            float timeFactor = 0; // to be filled in below

            // Break the pursuit into nine cases, the cross product of the
            // quarry being [ahead, aside, or behind] us and heading
            // [parallel, perpendicular, or anti-parallel] to us.
            switch (f)
            {
                case +1:
                    switch (p)
                    {
                        case +1: // ahead, parallel
                            timeFactor = 4;
                            break;
                        case 0: // ahead, perpendicular
                            timeFactor = 1.8f;
                            break;
                        case -1: // ahead, anti-parallel
                            timeFactor = 0.85f;
                            break;
                    }
                    break;
                case 0:
                    switch (p)
                    {
                        case +1: // aside, parallel
                            timeFactor = 1;
                            break;
                        case 0: // aside, perpendicular
                            timeFactor = 0.8f;
                            break;
                        case -1: // aside, anti-parallel
                            timeFactor = 4;
                            break;
                    }
                    break;
                case -1:
                    switch (p)
                    {
                        case +1: // behind, parallel
                            timeFactor = 0.5f;
                            break;
                        case 0: // behind, perpendicular
                            timeFactor = 2;
                            break;
                        case -1: // behind, anti-parallel
                            timeFactor = 2;
                            break;
                    }
                    break;
            }

            // estimated time until intercept of quarry
            var et = directTravelTime * timeFactor;
            var etl = (et > _maxPredictionTime) ? _maxPredictionTime : et;

            // estimated position of quarry at intercept
            var target = _quarry.PredictFuturePosition(etl);

            force = Vehicle.GetSeekVector(target, _slowDownOnApproach);
#endif