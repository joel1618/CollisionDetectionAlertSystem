using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollisionDetectionAlertSystem.Domain.Models;
using System.Web.Mvc;

namespace CollisionDetectionAlertSystem.Models
{
    public class MovingObjectViewModel
    {
        //return to server
        public Int64 Fingerprint { get; set; }
        public double Speed { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Heading { get; set; }

        //send to client
        public string StatusMessage { get; set; }
        //0 = person, 1 = car, 2 = airplane
        public int ModeOfTransportation { get; set; }
        public List<MovingObject> OtherMovingObjects { get; set; }
        //public BoundingBox BoundingBox { get; set; }
        public MovingObject MovingObject { get; set; }

        public MovingObjectViewModel()
        {

        }
        public MovingObjectViewModel(MovingObject movingObject)
        {

        }
    }
}
