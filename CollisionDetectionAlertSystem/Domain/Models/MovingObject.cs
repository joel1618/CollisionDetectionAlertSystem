using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollisionDetectionAlertSystem.Models;
using System.Web.Mvc;

namespace CollisionDetectionAlertSystem.Domain.Models
{
    public class MovingObject
    {
        public Int64 Fingerprint { get; set; }
        public double Position { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }
        public double Heading { get; set; }
        public BoundingBox BoundingBox { get; set; }
        public string Status { get; set; }
        //0 = person, 1 = car, 2 = airplane
        public int ModeOfTransportation { get; set; }
        public DateTime LastPost { get; set; }

        public MovingObject(MovingObjectViewModel viewModel)
        {
            this.Fingerprint = viewModel.Fingerprint;
            this.Position = viewModel.Latitude * viewModel.Longitude;
            this.Latitude = viewModel.Latitude;
            this.Longitude = viewModel.Longitude;
            this.Speed = viewModel.Speed;
            this.Heading = viewModel.Heading;
            this.ModeOfTransportation = viewModel.ModeOfTransportation;
            this.BoundingBox = new BoundingBox(this.Latitude, this.Longitude, this.Heading, this.Speed, this.ModeOfTransportation);
            this.Status = "green";
            this.LastPost = DateTime.Now;
        }
    }
}
