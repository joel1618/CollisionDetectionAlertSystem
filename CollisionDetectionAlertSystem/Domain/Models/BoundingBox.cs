using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollisionDetectionAlertSystem.Domain.Models
{
    public class BoundingBox
    {
        public struct Corner 
        {
            public double Latitude;
            public double Longitude;
        }
        public Corner FrontLeftCorner;
        public Corner FrontRightCorner;
        public Corner BackLeftCorner;
        public Corner BackRightCorner;

        private Corner currentPosition;
        //distance in kilometers, heading in degrees
        private double distanceForwardBackward, distanceLeftRight, heading;
        public BoundingBox(double latitude, double longitude, double heading, double speed, int modeOfTransportation)
        {
            //convert from mph -> kph -> mps
            this.heading = heading;
            double metersPerSecond = speed * 1.60934 * .277778;
            currentPosition = new Corner();
            currentPosition.Latitude = latitude;
            currentPosition.Longitude = longitude;
            
            //person
            if (modeOfTransportation == 0)
            {
                //distance = (speed x time) / (convert to kilometers) + base kilomoters
                distanceForwardBackward = (metersPerSecond * 5) / 1000 + .001;
                distanceLeftRight = .001;                
            }
            //car
            else if (modeOfTransportation == 1)
            {
                distanceForwardBackward = (metersPerSecond * 5) / 1000 + .005;
                distanceLeftRight = .005;
            }
            //airplane
            else
            {
                distanceForwardBackward = (metersPerSecond * 5) / 1000 + .04;
                distanceLeftRight = .04;
            }
            CalculateBoundingBoxCorners();
        }

        private void CalculateBoundingBoxCorners()
        {
                Corner tempCorner = FindPointAtDistanceFrom(currentPosition, DegreesToRadians(heading), distanceForwardBackward);
                FrontLeftCorner = FindPointAtDistanceFrom(tempCorner, DegreesToRadians(heading - 90), distanceLeftRight);
                FrontRightCorner = FindPointAtDistanceFrom(tempCorner, DegreesToRadians(heading + 90), distanceLeftRight);
                tempCorner = FindPointAtDistanceFrom(currentPosition, DegreesToRadians(heading - 180), distanceForwardBackward);
                BackLeftCorner = FindPointAtDistanceFrom(tempCorner, DegreesToRadians(heading - 90), distanceLeftRight);
                BackRightCorner = FindPointAtDistanceFrom(tempCorner, DegreesToRadians(heading + 90), distanceLeftRight);
        }

        //http://stackoverflow.com/questions/3225803/calculate-endpoint-given-distance-bearing-starting-point
        private static Corner FindPointAtDistanceFrom(Corner startPoint, double initialBearingRadians, double distanceKilometres)
        {
            const double radiusEarthKilometres = 6371.01;
            var distRatio = distanceKilometres / radiusEarthKilometres;
            var distRatioSine = Math.Sin(distRatio);
            var distRatioCosine = Math.Cos(distRatio);

            var startLatRad = DegreesToRadians(startPoint.Latitude);
            var startLonRad = DegreesToRadians(startPoint.Longitude);

            var startLatCos = Math.Cos(startLatRad);
            var startLatSin = Math.Sin(startLatRad);

            var endLatRads = Math.Asin((startLatSin * distRatioCosine) + (startLatCos * distRatioSine * Math.Cos(initialBearingRadians)));

            var endLonRads = startLonRad
                + Math.Atan2(
                    Math.Sin(initialBearingRadians) * distRatioSine * startLatCos,
                    distRatioCosine - startLatSin * Math.Sin(endLatRads));

            return new Corner
            {
                Latitude = RadiansToDegrees(endLatRads),
                Longitude = RadiansToDegrees(endLonRads)
            };
        }

        private static double DegreesToRadians(double degrees)
        {
            const double degToRadFactor = Math.PI / 180;
            return degrees * degToRadFactor;
        }

        private static double RadiansToDegrees(double radians)
        {
            const double radToDegFactor = 180 / Math.PI;
            return radians * radToDegFactor;
        }
    }
}
