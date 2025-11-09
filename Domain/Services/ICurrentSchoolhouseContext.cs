using System;
using Azorian.Data;

namespace Azorian.Domain.Services;

public interface ICurrentSchoolhouseContext
{
    Guid? SchoolhouseId { get; }
    Schoolhouse? Schoolhouse { get; }
}
