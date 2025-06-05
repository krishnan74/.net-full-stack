using Notify.Models.DTOs;
using Notify.Models;

namespace Notify.Misc;

public static class HRAdminMapper
{
    public static HRAdmin ToHRAdmin(this HRAdminAddRequestDTO dto, string userName)
    {
        return new HRAdmin
        {
            Username = userName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Department = dto.Department,
            Position = dto.Position,
            PhoneNumber = dto.PhoneNumber
        };
    }

    public static HRAdminResponseDTO ToResponseDTO(this HRAdmin hrAdmin)
    {
        return new HRAdminResponseDTO
        {
            Id = hrAdmin.Id,
            Username = hrAdmin.Username,
            FirstName = hrAdmin.FirstName,
            LastName = hrAdmin.LastName,
            Department = hrAdmin.Department,
            Position = hrAdmin.Position,
            PhoneNumber = hrAdmin.PhoneNumber,
            Name = hrAdmin.Name
        };
    }
}