using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Image;

public interface IImageGenerationService
{
    Task ProcessAsync(string taskId);
}
