using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.entityframework;

public abstract class EntityBase
{
    public string Id { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}
