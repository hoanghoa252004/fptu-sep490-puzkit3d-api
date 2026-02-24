using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Data;

public interface IUnitOfWork : IDisposable
{
    Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
}
