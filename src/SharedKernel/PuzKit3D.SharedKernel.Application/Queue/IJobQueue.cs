using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Queue;

public interface IJobQueue
{
    Task EnqueueAsync(string taskId);
    Task<string> DequeueAsync(CancellationToken ct);
}
