using PuzKit3D.SharedKernel.Application.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure.Queue;

public class JobQueue : IJobQueue
{
    private readonly Channel<string> _queue = Channel.CreateUnbounded<string>();

    public async Task EnqueueAsync(string taskId)
    {
        await _queue.Writer.WriteAsync(taskId);
    }

    public async Task<string> DequeueAsync(CancellationToken ct)
    {
        return await _queue.Reader.ReadAsync(ct);
    }
}