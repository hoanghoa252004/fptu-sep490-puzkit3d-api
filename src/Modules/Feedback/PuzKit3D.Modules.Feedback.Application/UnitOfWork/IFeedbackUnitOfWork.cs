namespace PuzKit3D.Modules.Feedback.Application.UnitOfWork;

/// <summary>
/// Unit of Work specific for Feedback module
/// </summary>
public interface IFeedbackUnitOfWork
{
    Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
