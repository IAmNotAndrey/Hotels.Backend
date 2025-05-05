using Hotels.Application.Exceptions;
using System.Security.Claims;

namespace Hotels.Persistence.Interfaces.Repositories;

public interface IApplicationUserRepo
{
    /// <summary>
    /// Checks whether <paramref name="user"/>.Id == <paramref name="idToCompare"/>
    /// </summary>
    /// <returns><see langword="true"/> if <paramref name="user"/> is <see cref="Admin"/> or User.Id == <paramref name="idToCompare"/><br/>
    /// <see langword="false"/> else
    /// </returns>
    Task<bool> IsUserAllowedAsync(ClaimsPrincipal user, string idToCompare);

    /// <summary>
    /// Changes the name of an application user identified by the specified <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The unique identifier of the user whose name is to be changed.</param>
    /// <param name="name">The new name to assign to the user.</param>
    /// <exception cref="EntityNotFoundException">
    /// Thrown if a user with the specified <paramref name="id"/> is not found.
    /// </exception>
    Task ChangeNameAsync(string id, string name);
}
