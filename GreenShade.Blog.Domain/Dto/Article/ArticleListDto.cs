using System;
using System.Collections.Generic;
using System.Text;

namespace GreenShade.Blog.Domain.Dto
{
   public class ArticleListDto
    {
        public int PageTotal { get; set; }
        public List<ArticleDto> Arts { get; set; }
    }
}
