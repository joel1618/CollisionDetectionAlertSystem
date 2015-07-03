using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollisionDetectionAlertSystem.Models;

namespace CollisionDetectionAlertSystem.Domain.Interface
{
    public interface IMovingObjectService
    {
        MovingObjectViewModel CheckStatus(MovingObjectViewModel viewModel);
    }
}
