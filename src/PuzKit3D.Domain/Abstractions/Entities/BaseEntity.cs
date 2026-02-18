using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PuzKit3D.Domain.Abstractions.Entities;

public abstract class BaseEntity<TKey>
{
    public virtual TKey Id { get; set; }
    /// <summary>
    /// HoaTHSE184053_Note:
    /// Tại sao cần virtual cho property Id ?
    /// 1. Entity Framework Core / ORM Requirements
    ///     EF Core cần virtual để tạo dynamic proxy cho:
    ///         - Lazy loading - Load related data khi cần
    ///         - Change tracking - Theo dõi thay đổi
    ///         - Navigation properties - Relationship handling
    /// 2. Class con có thể custom logic HOẶC computed value
    /// </summary>
    
    public bool IsTransient()
    {
        return Id.Equals(default(TKey));
    }
}
