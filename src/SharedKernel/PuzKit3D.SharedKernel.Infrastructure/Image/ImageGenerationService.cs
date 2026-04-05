using PuzKit3D.SharedKernel.Application.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure.Image;

internal class ImageGenerationService : IImageGenerationService
{
    public async Task ProcessAsync(string taskId)
    {
        // 1. get task từ DB

        // 2. download ảnh từ S3 (byte[])

        // 3. gọi Gemini

        // 4. upload lại S3

        // 5. update DB status DONE
    }
}