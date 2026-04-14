using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Commands;

public record ToDto(
    string FullName,
    string PhoneNumber,
    string StreetAddress,
    string Ward,
    string District,
    string Province
    );

public record FromDto(
    string FullName,
    string PhoneNumber,
    string StreetAddress,
    string Ward,
    string District,
    string Province
    );
