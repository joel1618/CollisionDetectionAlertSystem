using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollisionDetectionAlertSystem.Domain.Interface;
using CollisionDetectionAlertSystem.Domain.Models;
using CollisionDetectionAlertSystem.Models;
using System.Diagnostics;

namespace CollisionDetectionAlertSystem.Domain.Services
{
    class MovingObjectService : IMovingObjectService
    {
        private static List<MovingObject> movingObjectList;
        private static Object thisLock;
        private MovingObject movingObject;
        public MovingObjectViewModel CheckStatus(MovingObjectViewModel movingObjectViewModel)
        {
            if (movingObjectList == null) movingObjectList = new List<MovingObject>();
            if (thisLock == null) thisLock = new Object();
            if (movingObjectViewModel.Fingerprint != 0 && movingObjectViewModel.Latitude != 0 && movingObjectViewModel.Longitude != 0)
            {
                lock (thisLock)
                {
                    movingObject = new MovingObject(movingObjectViewModel);
                    movingObjectList.RemoveAll(e => (e.Fingerprint == movingObject.Fingerprint || e.Fingerprint == 0 || e.LastPost < DateTime.Now.AddSeconds(-10)));
                    movingObjectList.Add(movingObject);
                    SortMovingObjectList();
                    movingObjectViewModel.OtherMovingObjects = GetMovingObjectNearPosition(movingObject.Position, movingObject.Fingerprint);

                    movingObjectViewModel.MovingObject = movingObject;
                }
                DetermineCollisions(movingObject, movingObjectViewModel.OtherMovingObjects);
            }            
            //movingObjectViewModel.BoundingBox = movingObject.BoundingBox;
            return movingObjectViewModel;
        }

        private void SortMovingObjectList()
        {
            if (movingObjectList != null && movingObjectList.ToList().Count > 0)
            {
                movingObjectList.Sort(delegate(MovingObject x, MovingObject y)
                {
                    return y.Position.CompareTo(x.Position);
                });
            }
        }

        private List<MovingObject> GetMovingObjectNearPosition(double position, double fingerprint)
        {
            return movingObjectList.Where(e => ((e.Position >= position - 100) || (e.Position <= position + 100)) && (e.Fingerprint != fingerprint)).ToList();
        }

        //check for overlaps in the bounding boxes of a list that will be grabbed where the position is within a certain area.
        //This filter will reduce the number of comparisons we need to make.
        //This is very inefficient.  Have to figure out a  better way.  
        //Also will only catch boxes that lines cross each other.
        private void DetermineCollisions(MovingObject movingObject, List<MovingObject> movingObjectList)
        {
            movingObject.Status = "green";
            bool intersection = false;
            foreach (MovingObject _movingObject in movingObjectList)
            {
                //FrontLeft to FrontRight
               if(IsIntersecting(movingObject.BoundingBox.FrontLeftCorner.Latitude,
                   movingObject.BoundingBox.FrontLeftCorner.Longitude,
                   movingObject.BoundingBox.FrontRightCorner.Latitude,
                   movingObject.BoundingBox.FrontRightCorner.Longitude,
                   _movingObject.BoundingBox.FrontLeftCorner.Latitude,
                   _movingObject.BoundingBox.FrontLeftCorner.Longitude,
                   _movingObject.BoundingBox.FrontRightCorner.Latitude,
                   _movingObject.BoundingBox.FrontRightCorner.Longitude))
               {
                   intersection = true;
                   break;
               }
               if (IsIntersecting(movingObject.BoundingBox.FrontLeftCorner.Latitude,
                  movingObject.BoundingBox.FrontLeftCorner.Longitude,
                  movingObject.BoundingBox.FrontRightCorner.Latitude,
                  movingObject.BoundingBox.FrontRightCorner.Longitude,
                  _movingObject.BoundingBox.FrontRightCorner.Latitude,
                  _movingObject.BoundingBox.FrontRightCorner.Longitude,
                  _movingObject.BoundingBox.BackRightCorner.Latitude,
                  _movingObject.BoundingBox.BackRightCorner.Longitude))
               {
                   intersection = true;
                   break;
               }
                if (IsIntersecting(movingObject.BoundingBox.FrontLeftCorner.Latitude,
                  movingObject.BoundingBox.FrontLeftCorner.Longitude,
                  movingObject.BoundingBox.FrontRightCorner.Latitude,
                  movingObject.BoundingBox.FrontRightCorner.Longitude,
                  _movingObject.BoundingBox.BackRightCorner.Latitude,
                  _movingObject.BoundingBox.BackRightCorner.Longitude,
                  _movingObject.BoundingBox.BackLeftCorner.Latitude,
                  _movingObject.BoundingBox.BackLeftCorner.Longitude))
                {
                    intersection = true;
                    break;
                }
                if (IsIntersecting(movingObject.BoundingBox.FrontLeftCorner.Latitude,
                  movingObject.BoundingBox.FrontLeftCorner.Longitude,
                  movingObject.BoundingBox.FrontRightCorner.Latitude,
                  movingObject.BoundingBox.FrontRightCorner.Longitude,
                  _movingObject.BoundingBox.BackRightCorner.Latitude,
                  _movingObject.BoundingBox.BackRightCorner.Longitude,
                  _movingObject.BoundingBox.BackLeftCorner.Latitude,
                  _movingObject.BoundingBox.BackLeftCorner.Longitude))
                {
                    intersection = true;
                    break;
                }

                //FrontRight to BackRight
                if (IsIntersecting(movingObject.BoundingBox.FrontRightCorner.Latitude,
                   movingObject.BoundingBox.FrontRightCorner.Longitude,
                   movingObject.BoundingBox.BackRightCorner.Latitude,
                   movingObject.BoundingBox.BackRightCorner.Longitude,
                   _movingObject.BoundingBox.FrontLeftCorner.Latitude,
                   _movingObject.BoundingBox.FrontLeftCorner.Longitude,
                   _movingObject.BoundingBox.FrontRightCorner.Latitude,
                   _movingObject.BoundingBox.FrontRightCorner.Longitude))
                {
                    intersection = true;
                    break;
                }
                if (IsIntersecting(movingObject.BoundingBox.FrontRightCorner.Latitude,
                   movingObject.BoundingBox.FrontRightCorner.Longitude,
                   movingObject.BoundingBox.BackRightCorner.Latitude,
                   movingObject.BoundingBox.BackRightCorner.Longitude,
                   _movingObject.BoundingBox.FrontRightCorner.Latitude,
                   _movingObject.BoundingBox.FrontRightCorner.Longitude,
                   _movingObject.BoundingBox.BackRightCorner.Latitude,
                   _movingObject.BoundingBox.BackRightCorner.Longitude))
                {
                    intersection = true;
                    break;
                }
                if (IsIntersecting(movingObject.BoundingBox.FrontRightCorner.Latitude,
                   movingObject.BoundingBox.FrontRightCorner.Longitude,
                   movingObject.BoundingBox.BackRightCorner.Latitude,
                   movingObject.BoundingBox.BackRightCorner.Longitude,
                  _movingObject.BoundingBox.BackRightCorner.Latitude,
                  _movingObject.BoundingBox.BackRightCorner.Longitude,
                  _movingObject.BoundingBox.BackLeftCorner.Latitude,
                  _movingObject.BoundingBox.BackLeftCorner.Longitude))
                {
                    intersection = true;
                    break;
                }
                if (IsIntersecting(movingObject.BoundingBox.FrontRightCorner.Latitude,
                   movingObject.BoundingBox.FrontRightCorner.Longitude,
                   movingObject.BoundingBox.BackRightCorner.Latitude,
                   movingObject.BoundingBox.BackRightCorner.Longitude,
                  _movingObject.BoundingBox.BackRightCorner.Latitude,
                  _movingObject.BoundingBox.BackRightCorner.Longitude,
                  _movingObject.BoundingBox.BackLeftCorner.Latitude,
                  _movingObject.BoundingBox.BackLeftCorner.Longitude))
                {
                    intersection = true;
                    break;
                }
                //BackRight to BackLeft
                if (IsIntersecting(movingObject.BoundingBox.BackRightCorner.Latitude,
                   movingObject.BoundingBox.BackRightCorner.Longitude,
                   movingObject.BoundingBox.BackLeftCorner.Latitude,
                   movingObject.BoundingBox.BackLeftCorner.Longitude,
                   _movingObject.BoundingBox.FrontLeftCorner.Latitude,
                   _movingObject.BoundingBox.FrontLeftCorner.Longitude,
                   _movingObject.BoundingBox.FrontRightCorner.Latitude,
                   _movingObject.BoundingBox.FrontRightCorner.Longitude))
                {
                    intersection = true;
                    break;
                }
                if (IsIntersecting(movingObject.BoundingBox.BackRightCorner.Latitude,
                   movingObject.BoundingBox.BackRightCorner.Longitude,
                   movingObject.BoundingBox.BackLeftCorner.Latitude,
                   movingObject.BoundingBox.BackLeftCorner.Longitude,
                   _movingObject.BoundingBox.FrontRightCorner.Latitude,
                   _movingObject.BoundingBox.FrontRightCorner.Longitude,
                   _movingObject.BoundingBox.BackRightCorner.Latitude,
                   _movingObject.BoundingBox.BackRightCorner.Longitude))
                {
                    intersection = true;
                    break;
                }
                if (IsIntersecting(movingObject.BoundingBox.BackRightCorner.Latitude,
                   movingObject.BoundingBox.BackRightCorner.Longitude,
                   movingObject.BoundingBox.BackLeftCorner.Latitude,
                   movingObject.BoundingBox.BackLeftCorner.Longitude,
                  _movingObject.BoundingBox.BackRightCorner.Latitude,
                  _movingObject.BoundingBox.BackRightCorner.Longitude,
                  _movingObject.BoundingBox.BackLeftCorner.Latitude,
                  _movingObject.BoundingBox.BackLeftCorner.Longitude))
                {
                    intersection = true;
                    break;
                }
                if (IsIntersecting(movingObject.BoundingBox.BackRightCorner.Latitude,
                   movingObject.BoundingBox.BackRightCorner.Longitude,
                   movingObject.BoundingBox.BackLeftCorner.Latitude,
                   movingObject.BoundingBox.BackLeftCorner.Longitude,
                  _movingObject.BoundingBox.BackRightCorner.Latitude,
                  _movingObject.BoundingBox.BackRightCorner.Longitude,
                  _movingObject.BoundingBox.BackLeftCorner.Latitude,
                  _movingObject.BoundingBox.BackLeftCorner.Longitude))
                {
                    intersection = true;
                    break;
                }
                //BackLeft to FrontLeft
                if (IsIntersecting(movingObject.BoundingBox.BackLeftCorner.Latitude,
                   movingObject.BoundingBox.BackLeftCorner.Longitude,
                   movingObject.BoundingBox.FrontLeftCorner.Latitude,
                   movingObject.BoundingBox.FrontLeftCorner.Longitude,
                   _movingObject.BoundingBox.FrontLeftCorner.Latitude,
                   _movingObject.BoundingBox.FrontLeftCorner.Longitude,
                   _movingObject.BoundingBox.FrontRightCorner.Latitude,
                   _movingObject.BoundingBox.FrontRightCorner.Longitude))
                {
                    intersection = true;
                    break;
                }
                if (IsIntersecting(movingObject.BoundingBox.BackLeftCorner.Latitude,
                   movingObject.BoundingBox.BackLeftCorner.Longitude,
                   movingObject.BoundingBox.FrontLeftCorner.Latitude,
                   movingObject.BoundingBox.FrontLeftCorner.Longitude,
                   _movingObject.BoundingBox.FrontRightCorner.Latitude,
                   _movingObject.BoundingBox.FrontRightCorner.Longitude,
                   _movingObject.BoundingBox.BackRightCorner.Latitude,
                   _movingObject.BoundingBox.BackRightCorner.Longitude))
                {
                    intersection = true;
                    break;
                }
                if (IsIntersecting(movingObject.BoundingBox.BackLeftCorner.Latitude,
                   movingObject.BoundingBox.BackLeftCorner.Longitude,
                   movingObject.BoundingBox.FrontLeftCorner.Latitude,
                   movingObject.BoundingBox.FrontLeftCorner.Longitude,
                  _movingObject.BoundingBox.BackRightCorner.Latitude,
                  _movingObject.BoundingBox.BackRightCorner.Longitude,
                  _movingObject.BoundingBox.BackLeftCorner.Latitude,
                  _movingObject.BoundingBox.BackLeftCorner.Longitude))
                {
                    intersection = true;
                    break;
                }
                if (IsIntersecting(movingObject.BoundingBox.BackLeftCorner.Latitude,
                   movingObject.BoundingBox.BackLeftCorner.Longitude,
                   movingObject.BoundingBox.FrontLeftCorner.Latitude,
                   movingObject.BoundingBox.FrontLeftCorner.Longitude,
                  _movingObject.BoundingBox.BackRightCorner.Latitude,
                  _movingObject.BoundingBox.BackRightCorner.Longitude,
                  _movingObject.BoundingBox.BackLeftCorner.Latitude,
                  _movingObject.BoundingBox.BackLeftCorner.Longitude))
                {
                    intersection = true;
                    break;
                }
            }
            if (intersection)
                movingObject.Status = "red";
        }

        bool IsIntersecting(double ax, double ay, double bx, double by, 
            double cx, double cy, double dx, double dy)
        {
            double denominator = ((bx - ax) * (dy - cy)) - ((by - ay) * (dx - cx));
            double numerator1 = ((ay - cy) * (dx - cx)) - ((ax - cx) * (dy - cy));
            double numerator2 = ((ay - cy) * (bx - ax)) - ((ax - cx) * (by - ay));

            // Detect coincident lines (has a problem, read below)
            if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

            double r = numerator1 / denominator;
            double s = numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }
    }
}
