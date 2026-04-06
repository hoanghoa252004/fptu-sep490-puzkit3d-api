using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application._3DModel;

public interface I3DModelGenerationService
{
    Task ProcessAsync(string taskId);
}
