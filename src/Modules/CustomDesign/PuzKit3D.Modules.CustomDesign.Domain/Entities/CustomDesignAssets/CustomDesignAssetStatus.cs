using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;

public enum CustomDesignAssetStatus
{
    ImageProcessing, // AI is processing the image
    RoughModelGenerating, // AI is generating the 3D model
    Completed
}
